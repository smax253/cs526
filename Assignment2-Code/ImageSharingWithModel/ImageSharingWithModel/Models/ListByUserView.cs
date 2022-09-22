using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ImageSharingWithModel.Models
{
    public class ListByUserView
    {
        public int Id { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
    }
}
