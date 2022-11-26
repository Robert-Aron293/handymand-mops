using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models
{
    public class Feedback:BaseEntity
    {
        public int CreationUserId { get; set; }
        public User CreationUser { get; set; }

        public int  RefferedUserId { get; set; }
        public User RefferedUser { get; set; }

        public int JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; }

        public int Grade { get; set; }

        public bool FromClientToFreelancer { get; set; }
        public bool FromFreelancerToClient { get; set; }

    }
}
