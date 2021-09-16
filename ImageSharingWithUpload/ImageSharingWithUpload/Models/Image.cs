using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace ImageSharingWithUpload.Models
{
    public class Image
    {
        [Required]
        [RegularExpression(@"[a-zA-Z0-9_]+")]
        public String Id {get; set; }
        [Required]
        [StringLength(40)]
        public String Caption {get; set;}
        [StringLength(200)]
        public String Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateTaken { get; set; }
        public String Userid { get; set; }

        public Image()
        {
        }
    }
}