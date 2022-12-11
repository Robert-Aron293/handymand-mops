using Handymand.Data;
using Handymand.Models;
using Handymand.Models.DTOs;
using Handymand.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Repository.DatabaseRepositories
{
    public class SkillRepository:GenericRepository<Skill>, ISkillRepository
    {
        public SkillRepository(HandymandContext context) : base(context)
        {

        }

        public async Task<Skill> FindByNameAsync(string name)
        {
            return await _table.FirstOrDefaultAsync(x => x.SkillName.ToLower().Equals(name.ToLower()));
        }

        public async Task<List<Skill>> GetAllSkillsIncludeAsync()
        {
            return await _table.Include("CreationUser").Include("ModificationUser").ToListAsync();

        }


        public async Task<List<SkillShortDTO>> GetSkillShortDTOsAsync()
        {
            return await _table.Select(s =>
            new SkillShortDTO()
            {
                Id = s.Id,
                SkillName = s.SkillName
            }).ToListAsync();
        }
    }
}
