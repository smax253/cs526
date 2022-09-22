using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageSharingWithModel.Models
{
    public class Image
        /*
         * Entity model for an image.
         */
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }
        [MaxLength(40)]
        public virtual string Caption { get; set; }
        [MaxLength(200)]
        public virtual string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public virtual DateTime DateTaken { get; set; }

        [ForeignKey("User")]
        public virtual int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Tag")]
        public virtual int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}