﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageSharingWithModel.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id {get;set;}
        [Required]
        [MaxLength(20)]
        public virtual string Username {get;set;}
        public virtual bool ADA { get; set; }

        public virtual ICollection<Image> Images {get;set;}

        public User() { }

        public User(string u, bool a)
        {
            Username = u;
            ADA = a;
            Images = new List<Image>();
        }
    }
}