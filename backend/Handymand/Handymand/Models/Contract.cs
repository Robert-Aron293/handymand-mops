using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models
{
    public class Contract: BaseEntity
    {
        public int CreationUserId { get; set;}
        public User CreationUser { get; set;}

        public int RefferedUserId { get; set; }
        public User RefferedUser { get; set; }

        public double PaymentAmountPerHour { get; set; }

        public int JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; }

        public bool Valid { get; set; }


    }
}
