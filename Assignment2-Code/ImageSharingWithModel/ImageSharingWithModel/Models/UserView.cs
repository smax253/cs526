using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace ImageSharingWithModel.Models
{
    public class UserView
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z0-9_]+")]
        public String Username { get; set; }

        [Required]
        public bool ADA { get; set; }

        public bool IsADA()
        {
            // return "on".Equals(ADA);
            return ADA;
        }
    }
}