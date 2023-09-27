using System;

namespace EntityLayer.DTO
{
    public class MessageReceiverUserDto
    {
        public int MessageID { get; set; }

        public string Subject { get; set; }

        public string Details { get; set; }

        public DateTime MessageDate { get; set; }

        public bool MessageStatus { get; set; }

        public int? SenderUserId { get; set; }

        public int? ReceiverUserId { get; set; }

        public string ReceiverUserName { get; set; }

        public string ReceiverNameSurname { get; set; }

        public string ReceiverImageUrl { get; set; }
    }
}
