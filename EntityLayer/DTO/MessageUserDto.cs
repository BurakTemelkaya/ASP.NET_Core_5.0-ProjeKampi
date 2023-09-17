using EntityLayer.Concrete;

namespace EntityLayer.DTO
{
    public class MessageUserDto
    {
        public Message Message { get; set; }

        public string ReceiverUserName { get; set; }

        public string ReceiverNameSurname { get; set; }

        public string ReceiverImageUrl { get; set; }

        public string SenderUserName { get; set; }

        public string SenderNameSurname { get; set; }

        public string SenderImageUrl { get; set; }
    }
}
