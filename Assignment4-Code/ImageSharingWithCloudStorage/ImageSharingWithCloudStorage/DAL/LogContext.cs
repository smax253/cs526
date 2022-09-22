using System;
using System.Collections.Generic;

using ImageSharingWithCloudStorage.Models;

using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ImageSharingWithCloudStorage.DAL
{
    public class LogContext : ILogContext
    {
        public const string LOG_TABLE_NAME = "imageviews";

        protected CloudTable table;

        protected ILogger<LogContext> logger;

        public LogContext(IOptions<StorageOptions> options, ILogger<LogContext> logger)
        {
            this.logger = logger;

           
            String tableName = "ImageViews";
            String tableConnectionString = options.Value.LogEntryDb;
            CloudStorageAccount account = CloudStorageAccount.Parse(tableConnectionString);
            CloudTableClient tableClient = account.CreateCloudTableClient(new TableClientConfiguration());
            table = tableClient.GetTableReference(tableName);
            
        }


        protected CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                logger.LogError("Invalid storage account information format: {0}", storageConnectionString);
                throw;
            }
            catch (ArgumentException)
            {
                logger.LogError("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                throw;
            }

            return storageAccount;
        }

        protected CloudTable GetTable(string storageConnectionString)
        {
            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(storageConnectionString);

            CloudTable table = null;
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            table = client.GetTableReference("ImageViews");

            return table;
        }

        public async Task<CloudTable> CreateTableAsync()
        {
            if (await table.CreateIfNotExistsAsync())
            {
                logger.LogInformation("Created Table named: {0}", LOG_TABLE_NAME);
            }
            else
            {
                logger.LogInformation("Table {0} already exists", LOG_TABLE_NAME);
            }

            return table;
        }

        public async Task AddLogEntryAsync(string user, ImageView image)
        {
            LogEntry entry = new LogEntry(image.Id);
            entry.Username = user;
            entry.Caption = image.Caption;
            entry.ImageId = image.Id;
            entry.Uri = image.Uri;

            logger.LogInformation("Adding log entry for image: {0}", image.Id);

            TableResult result = null;

            TableOperation insertOrMergeOp = TableOperation.InsertOrMerge(entry);
            result = await table.ExecuteAsync(insertOrMergeOp);
            
            if (result.RequestCharge.HasValue)
            {
                logger.LogInformation("Added log entry with charge {0}", result.RequestCharge.Value);
            }
        }

        public IEnumerable<LogEntry> LogsToday()
        {
            TableQuery<LogEntry> query = new TableQuery<LogEntry>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", 
                                    QueryComparisons.Equal, 
                                    DateTime.UtcNow.ToString("MMddyyyy"))).OrderBy("timestamp");
            
            IEnumerable<LogEntry> queries = table.ExecuteQuery(query);
            List<LogEntry> list = queries.ToList();
            list.Sort(delegate (LogEntry x, LogEntry y)
            { 
                return (x.EntryDate - y.EntryDate).Milliseconds;
            });
            return list;
        }

    }
}