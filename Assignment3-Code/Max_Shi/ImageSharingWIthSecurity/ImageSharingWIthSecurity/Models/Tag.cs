using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageSharingWithSecurity.Models
{
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id {get; set; }
        [MaxLength(20)]
        public virtual string Name {get; set;}

        public virtual ICollection<Image> Images {get;set;}
    }
}