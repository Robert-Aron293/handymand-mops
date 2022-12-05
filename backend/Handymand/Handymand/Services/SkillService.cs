using Handymand.Models;
using Handymand.Models.DTOs;
using Handymand.Repository.DatabaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;

        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        private SkillDTO ConvertToDTOforAdminTable(Skill skill)
        {
            SkillDTO dto = new SkillDTO();
            dto.Id = skill.Id;
            dto.SkillName = skill.SkillName;
            dto.CreationUserEmail = skill.CreationUser.Email;
            dto.CreationUserName = skill.CreationUser.LastName + " " + skill.CreationUser.FirstName;
            dto.ModificationUserEmail = skill.ModificationUser != null ? skill.ModificationUser.Email : null;
            dto.ModificationuserName = skill.ModificationUser != null ? skill.ModificationUser.LastName + " " + skill.ModificationUser.FirstName : null;
            dto.Description = skill.Description;
            dto.DateCreated = skill.DateCreated.ToString("MM/dd/yyyy HH:mm");
            dto.DateModified = skill.DateModified != null ? ((DateTime)skill.DateModified).ToString("MM/dd/yyyy HH:mm") : null;
            return dto;
        }

        private SkillShortDTO ConvertToShortDTO(Skill skill)
        {
            SkillShortDTO dto = new SkillShortDTO();
            dto.Id = skill.Id;
            dto.SkillName = skill.SkillName;
            return dto;
        }

        private Skill ConvertFromDTOForCreate(SkillDTO skill)
        {
            Skill item = new Skill();
            item.SkillName = skill.SkillName;
            item.Description = skill.Description;
            item.CreationUserId = skill.CreationUserId;

            item.DateCreated = DateTime.Now;

            return item;
        }

        public async Task<bool> Create(SkillDTO skill)
        {
            var alreadyExist = await _skillRepository.FindByNameAsync(skill.SkillName);

            if (alreadyExist != null)
            {
                throw new Exception("A skill with this name already exists. Use a different name!");
            }


            var forCreate = ConvertFromDTOForCreate(skill);

            await _skillRepository.CreateAsync(forCreate);
            return await _skillRepository.SaveAsync();


        }

        public async Task<bool> Update(SkillDTO skill)
        {
            if (skill.Id == null || skill.Id == 0)
            {
                throw new Exception("Can not update a skill with id null or 0!");
            }

            var existingSkill = await _skillRepository.FindByIdAsync((int)skill.Id);

            if (existingSkill == null)
            {
                throw new Exception("There is no skill with this id!");
            }

            if(skill.SkillName == null && skill.Description == null)
            {
                throw new Exception("Skill can not be updated with null SkillName or Description!");
            }
            else
            {
                if (skill.SkillName != null)
                {
                    skill.SkillName = skill.SkillName.Trim();
                }

                if (skill.Description != null)
                {
                    skill.Description = skill.Description.Trim();
                }
            }


            if ( skill.SkillName == "" &&  skill.Description == "")
            {
                throw new Exception("Skill can not be updated with empty SkillName or Description!");
            }


            existingSkill.SkillName = skill.SkillName;
            existingSkill.DateModified = DateTime.Now;
            existingSkill.Description = skill.Description;
            existingSkill.ModificationUserId = skill.ModificationUserId;

            _skillRepository.Update(existingSkill);
            return await _skillRepository.SaveAsync();
        }

        public async Task<bool> Delete(SkillDTO skill)
        {
            if (skill.Id == null || skill.Id == 0)
            {
                throw new Exception("Can not delete a skill with id null or 0!");
            }


            var existingSkill = await _skillRepository.FindByIdAsync((int)skill.Id);

            if (existingSkill == null)
            {
                throw new Exception("There is no skill with this id!");
            }


            _skillRepository.Delete(existingSkill);

            return await _skillRepository.SaveAsync();



        }


        public async Task<List<SkillDTO>> GetAllForAdmin()
        {
            var query = await _skillRepository.GetAllSkillsIncludeAsync();

            var result = new List<SkillDTO>();

            foreach(var elem in query)
            {
                result.Add(ConvertToDTOforAdminTable(elem));

            }

            return result;
        }

        public async Task<List<SkillShortDTO>> GetAll()
        {
            return await _skillRepository.GetSkillShortDTOsAsync();
        }


    }
}
