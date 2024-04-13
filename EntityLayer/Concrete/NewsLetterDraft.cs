using CoreLayer.Entities;
using System;

namespace EntityLayer.Concrete
{
    public class NewsLetterDraft : IEntity
    {
        public int NewsLetterDraftId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public DateTime TimeToAdd { get; set; }
    }
}
