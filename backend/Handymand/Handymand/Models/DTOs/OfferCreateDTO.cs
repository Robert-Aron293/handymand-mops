using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.DTOs
{
    public class OfferCreateDTO
    {
        public string Description { get; set; }
        public double PaymentAmount { get; set; }
        public int JobOfferId { get; set; }
        public int CreationUserId { get; set; }
    }
}
