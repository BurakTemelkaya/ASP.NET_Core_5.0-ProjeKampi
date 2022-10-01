using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.DTO;

namespace EntityLayer.Concrete
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public DateTime MessageDate { get; set; }
        public bool MessageStatus { get; set; }
        public AppUser SenderUser { get; set; }
        public int? SenderUserId { get; set; }
        public AppUser ReceiverUser { get; set; }
        public int? ReceiverUserId { get; set; }
    }
}
