using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models
{
    public class Skill:BaseEntity
    {
        public string SkillName { get; set; }

        public string Description { get; set; }

        public User CreationUser { get; set; }
        public int? CreationUserId { get; set; }

        public User ModificationUser { get; set; }
        public int? ModificationUserId { get; set; }

        public ICollection<FreelancersSkills> FreelancersSkills { get; set; }

        public ICollection<JobOffersSkills> JobOffersSkills { get; set; }

    }
}
