using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImageSharingWithSecurity.Models
{
    public class ImageView
        /*
         * View model for an image.
         */
    {
        [Required]
        [StringLength(40)]
        public String Caption {get; set;}
        [Required]
        public int TagId { get; set; }
        [Required]
        [StringLength(200)]
        public String Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}",ApplyFormatInEditMode=true)]
        public DateTime DateTaken { get; set; }

        [ScaffoldColumn(false)]
        // Do not call this Id, it will confuse model binding when posting back to controller
        // because of the default route {controller}/{action}/{?id}
        public int Id;
        [ScaffoldColumn(false)]
        public String TagName { get; set; }
        [ScaffoldColumn(false)]
        public String Username { get; set; }

        public IFormFile ImageFile { get; set; }

        public IEnumerable<SelectListItem> Tags { get; set; }

        public ImageView()
        {
        }
    }
}