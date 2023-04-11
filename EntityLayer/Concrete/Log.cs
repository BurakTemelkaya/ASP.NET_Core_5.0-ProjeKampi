using Castle.Components.DictionaryAdapter;
using CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Log : IEntity
    {
        public int Id { get; set; }

        public string Details { get; set; }

        public DateTime Log_Date { get; set; }

        public string Audit { get; set; }
    }
}
