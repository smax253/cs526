using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImageSharingWithSecurity.Models
{
    public class ListByTagModel
    {
        public int Id { get; set; }
        public IEnumerable<SelectListItem> Tags { get; set; }
    }
}
