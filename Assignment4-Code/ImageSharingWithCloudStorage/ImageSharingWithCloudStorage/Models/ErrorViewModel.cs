using System;

namespace ImageSharingWithCloudStorage.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public string ErrId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}