using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Models
{
    public class WriterAndCity
    {
        public List<SelectListItem> Cities { get; set; }
        public string City { get; set; }
        public Writer Writer { get; set; }
    }
}
