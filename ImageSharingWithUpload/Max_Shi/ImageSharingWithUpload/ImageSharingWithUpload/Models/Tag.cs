using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageSharingWithUpload.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Tag() { }

        public Tag(int i, string n) { Id=i; Name=n; }
    }
}