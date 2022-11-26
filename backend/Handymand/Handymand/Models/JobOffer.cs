using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models
{
    public class JobOffer: BaseEntity
    {
        public User CreationUser { get; set; }
        public int? CreationUserId { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public double LowPriceRange { get; set; }
        public double HighPriceRange { get; set; }
        public string Title { get; set; }
        public int? NoImages { get; set; }
        public bool Available { get; set; }

        public ICollection<JobOffersSkills> JobOffersSkills { get; set; }
        public ICollection<Offer> Offers { get; set; }
        public ICollection<Notification> ReferredNotifications { get; set; }
        
        public Contract Contract { get; set; }

        public Feedback Feedback { get; set; }

        public int? CityId { get; set; }
        public City City { get; set; }
    }
}
