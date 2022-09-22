using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace ImageSharingWithUpload.Models
{
    public class Image
    {
        [Required(ErrorMessage = "Please provide an image ID.")]
        [RegularExpression(@"[a-zA-Z0-9_]+", ErrorMessage = "Not a valid image ID.")]
        public String Id {get; set; }
        [Required(ErrorMessage = "Please provide a caption.")]
        [StringLength(40, ErrorMessage = "Caption is too long.")]
        public String Caption {get; set;}
        [StringLength(200, ErrorMessage = "Description is too long.")]
        public String Description { get; set; }
        [Required(ErrorMessage = "Please provide a date.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime DateTaken { get; set; }
        public String Userid { get; set; }

        public Image()
        {
        }
    }
}