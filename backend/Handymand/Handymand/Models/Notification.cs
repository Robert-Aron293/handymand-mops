using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models
{
    public class Notification: BaseEntity
    {
        public int CreationUserId { get; set; }
        public User CreationUser { get; set; }

        public int ReferredUserId { get; set; }
        public User ReferredUser { get; set; }

        public int JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; }

        public int NotificationTypeId { get; set; }
        public NotificationType NotificationType { get; set; }

        public bool Viewed { get; set; } 

    }
}
