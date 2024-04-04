using CoreLayer.Entities;

namespace EntityLayer.Concrete
{
    public class MessageDraft : IEntity
    {
        public int MessageDraftID { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public string ReceiverUser { get; set; }
        public int UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
