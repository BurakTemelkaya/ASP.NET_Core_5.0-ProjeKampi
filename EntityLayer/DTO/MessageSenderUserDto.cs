using System;

namespace EntityLayer.DTO
{
    public class MessageSenderUserDto
    {
        public int MessageID { get; set; }

        public string Subject { get; set; }

        public string Details { get; set; }

        public DateTime MessageDate { get; set; }

        public bool MessageStatus { get; set; }

        public int? SenderUserId { get; set; }

        public int? ReceiverUserId { get; set; }

        public string SenderUserName { get; set; }

        public string SenderNameSurname { get; set; }

        public string SenderImageUrl { get; set; }
    }
}
