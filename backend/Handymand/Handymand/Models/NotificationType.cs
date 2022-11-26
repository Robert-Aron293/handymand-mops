using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models
{
    public class NotificationType:BaseEntity
    {
        public string Description { get; set; }

        public ICollection<Notification> Notifications { get; set; }
        public ICollection<NotificationAffectedList> NotificationAffectedLists { get; set; }
    }
}
