using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models
{
    public class FreelancersSkills
    {
        public int IdFreelancer { get; set; }

        public Freelancer Freelancer { get; set; }

        public int IdSkill { get; set; }
        
        public Skill Skill { get; set; }

        public double Grade { get; set; }
    }
}
