using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace ImageSharingWithSecurity.Models
{
    public class UserView
    {
        public string Id { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z0-9_]+")]
        public String Username { get; set; }

        public String Password { get; set; }

        [Required]
        public bool ADA { get; set; }
    }
}