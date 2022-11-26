using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models
{
    public class Offer : BaseEntity
    {
        public string Description { get; set; }
        public double PaymentAmount { get; set; }

        public int CreationUserId { get; set; }
        public User CreationUser { get; set; }

        public int JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; }
        public bool Available { get; set; }

    }
}
