using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Handymand.Models.DTOs
{
    public class SkillDTO
    {
        public int? Id { get; set; }
        public string SkillName { get; set; }
        public string Description { get; set; }
        public int? CreationUserId { get; set; }
        public int? ModificationUserId { get; set; }
        public string CreationUserEmail { get; set; }
        public string CreationUserName { get; set; }
        public string ModificationUserEmail { get; set; }
        public string ModificationuserName { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }

    }
}
