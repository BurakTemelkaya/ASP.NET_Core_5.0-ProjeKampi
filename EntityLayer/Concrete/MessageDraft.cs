using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class MessageDraft
    {
        public int MessageDraftID { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public string ReceiverUser { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
