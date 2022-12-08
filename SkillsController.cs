using Handymand.Models;
using Handymand.Models.DTOs;
using Handymand.Services;
using Handymand.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Handymand.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;
        
        public SkillsController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [Authorization(Role.User,Role.Admin)]
        [HttpGet("all")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<SkillShortDTO>>>> GetAll()
        {
            var result = new ServiceResponse<List<SkillShortDTO>>();

            try
            {
                var skillList = await _skillService.GetAll();
                result.Data = skillList;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;
                return BadRequest(result);

            }

            return Ok(result);
        }

        [Authorization(Role.Admin)]
        [HttpGet("allforadmin")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<SkillDTO>>>> GetAllForAdmin()
        {
            var result = new ServiceResponse<List<SkillDTO>>();

            try
            {
                var skillList = await _skillService.GetAllForAdmin();
                result.Data = skillList;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;
                return BadRequest(result);

            }

            return Ok(result);
        }

        [Authorization(Role.Admin)]
        [HttpPost("create")]
        public async Task<ActionResult<ServiceResponse<bool>>> Create(SkillDTO skill)
        {
            var result = new ServiceResponse<bool>();


            if (skill != null)
            {
                try
                {
                    int id = Convert.ToInt32(HttpContext.User.FindFirstValue("id"));
                    if (id == 0)
                    {
                        return BadRequest("Id used for create a skill is null!");
                    }

                    skill.CreationUserId = id;
                    await _skillService.Create(skill);
                    result.Message = "Skill created!";
                }
                catch (Exception e)
                {
                    result.Message = e.Message;
                    result.Success = false;
                    return BadRequest(result);
                }
            }

            return Ok(result);

        }

        [Authorization(Role.Admin)]
        [HttpPut("update")]
        public async Task<ActionResult<ServiceResponse<bool>>> Update(SkillDTO skill)
        {
            var result = new ServiceResponse<bool>();


            if (skill != null)
            {
                try
                {
                    int id = Convert.ToInt32(HttpContext.User.FindFirstValue("id"));
                    if (id == 0)
                    {
                        return BadRequest("Id used for update a skill is null!");
                    }

                    skill.ModificationUserId = id;
                    await _skillService.Update(skill);
                    result.Message = "Skill updated!";
                }
                catch (Exception e)
                {
                    result.Message = e.Message;
                    result.Success = false;
                    return BadRequest(result);
                }
            }

            return Ok(result);

        }


        [Authorization(Role.Admin)]
        [HttpDelete("delete")]
        public async Task<ActionResult<ServiceResponse<bool>>> Delete(SkillDTO skill)
        {
            var result = new ServiceResponse<bool>();


            if (skill != null)
            {
                try
                {

                    await _skillService.Delete(skill);
                    result.Message = "Skill "+ skill.SkillName +" deleted!";
                }
                catch (Exception e)
                {
                    result.Message = e.Message;
                    result.Success = false;
                    return BadRequest(result);
                }
            }

            return Ok(result);

        }

    }
}
