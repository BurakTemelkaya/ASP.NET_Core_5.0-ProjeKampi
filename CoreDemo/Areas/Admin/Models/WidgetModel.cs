using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoreDemo.Areas.Admin.Models
{
    public class WidgetModel
    {
        public int BlogCount { get; set; }
        public int MessageCount { get; set; }
        public int CommentCount { get; set; }
        public string Temparature { get; set; }
    }
}
