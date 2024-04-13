using CoreLayer.Entities;
using System;

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
