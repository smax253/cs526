using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageSharingWithCloudStorage.Models
{
    public class ApproveModel
    { 
        public IList<SelectListItem> Images
        {
            get; set;
        }
    }
}
