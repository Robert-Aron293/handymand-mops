using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.DTOs
{
    public class JobOfferQuickViewDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Viewed { get; set; }
        public int NotViewedNotifications { get; set; }
        public int CreationUserId { get; set; }
        public string CreationUserName { get; set; }

    }
}
