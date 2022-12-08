using Handymand.Models;
using Handymand.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Services
{
    public interface ISkillService
    {

        Task<bool> Create(SkillDTO skill);
        Task<bool> Update(SkillDTO skill);
        Task<bool> Delete(SkillDTO skill);


        Task<List<SkillDTO>> GetAllForAdmin();
        Task<List<SkillShortDTO>> GetAll();

    }
}
