using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models
{
    public class ContractsSkills
    {

        public int IdContract { get; set; }
        public Contract Contract { get; set; }

        public int IdSkill { get; set; }
        public Skill Skill { get; set; }
    }
}
