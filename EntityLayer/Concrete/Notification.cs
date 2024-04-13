using CoreLayer.Entities;
using System;

namespace EntityLayer.Concrete
{
    public class Notification : IEntity
    {
        public int NotificationID { get; set; }
        public string NotificationType { get; set; }
        public string NotificationTypeSymbol { get; set; }
        public string NotificationDetails { get; set; }
        public DateTime NotificationDate { get; set; }
        public string NotificationColor { get; set; }
        public bool NotificationStatus { get; set; }
    }
}
