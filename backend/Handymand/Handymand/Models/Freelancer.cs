using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models
{
    public class Freelancer: BaseEntity
    {
        public string Overview { get; set; }

        public User User { get; set; }

        public int IdUser { get; set; }

        public int Score { get; set; }

        public ICollection<FreelancersSkills> FreelancersSkills { get; set; }

       // public ICollection<Contract> SubscribedContracts { get; set; }
    }
}
