using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models
{
    public class NotificationAffectedList: BaseEntity
    {
        public string Name { get; set; }

        public int NotificationTypeId { get; set; }
        public NotificationType NotificationType { get; set; }


    }
}
