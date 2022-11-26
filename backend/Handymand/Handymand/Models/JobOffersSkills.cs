using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models
{
    public class JobOffersSkills
    {
        public int IdSkill { get; set; }
        public Skill Skill { get; set; }

        public int IdJobOffer { get; set; }
        public JobOffer JobOffer { get; set; }
    }
}
