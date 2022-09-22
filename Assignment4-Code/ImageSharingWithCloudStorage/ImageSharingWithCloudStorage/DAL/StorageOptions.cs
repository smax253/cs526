namespace ImageSharingWithCloudStorage.DAL
{
    /*
     * https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options
     */

    public class StorageOptions
    {
        public const string Storage = "ConnectionStrings";

        public string ApplicationDb { get; set; }

        public string LogEntryDb { get; set; }

        public string ImageDb { get; set; }
    }
}
