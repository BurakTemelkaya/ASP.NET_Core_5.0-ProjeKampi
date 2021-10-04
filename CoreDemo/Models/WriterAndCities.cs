using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Models
{
    public class WriterAndCities
    {
        public List<SelectListItem> Cities { get; set; }
        public Writer Writers { get; set; }
    }
}
