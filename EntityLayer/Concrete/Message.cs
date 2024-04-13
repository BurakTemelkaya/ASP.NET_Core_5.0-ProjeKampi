using CoreLayer.Entities;
using System;

namespace EntityLayer.Concrete
{
    public class Message : IEntity
    {
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
