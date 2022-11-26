using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.DTOs
{
    public class OffersForLoggedInDTO
    {
        public int Id { get; set; }
        public string JobOfferTitle { get; set; }
        public int JobOfferId { get; set; }
        public double PaymentAmount { get; set; }
        public bool Viewed { get; set; }
        public int NotViewedNotifications { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
