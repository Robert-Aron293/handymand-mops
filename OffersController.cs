using Handymand.Models;
using Handymand.Models.DTOs;
using Handymand.Services;
using Handymand.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Handymand.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IOffersService _offerService;

        public OffersController(IOffersService offersService)
        {
            this._offerService = offersService;
        }

        [Authorization(Role.User,Role.Admin)]
        [HttpPost("accept")]
        public async Task<ActionResult<ServiceResponse<bool>>> AccepOffer([FromBody] AcceptOfferDTO dto)
        {
            int id = Convert.ToInt32(HttpContext.User.FindFirstValue("id"));
            if (id == 0)
            {
                return BadRequest("Logged in user is null or empty");
            }
            else
            {
                var response = new ServiceResponse<bool>();
                try
                {
                    var result = await _offerService.AcceptOffer(dto,id);
                    response.Data = result;
                }
                catch (Exception e)
                {
                    response.Success = false;
                    response.Message = e.Message;
                }

                return response;
            }
        }

        [Authorization(Role.User, Role.Admin)]
        [HttpGet("getratingfreelancer/{userId}")]
        public async Task<ActionResult<ServiceResponse<RatingDTO>>> GetRatingForFreelancer([FromRoute] int userId)
        {
            var response = new ServiceResponse<RatingDTO>();

            try
            {
                var result = await _offerService.GetRatingForFreelancer(userId);
                response.Data = result;

                return Ok(response);

            }
            catch(Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
                return BadRequest(response);
            }
        }


        [Authorization(Role.User, Role.Admin)]
        [HttpGet("getratingcustomer/{userId}")]
        public async Task<ActionResult<ServiceResponse<RatingDTO>>> GetRatingForCustomer([FromRoute] int userId)
        {
            var response = new ServiceResponse<RatingDTO>();

            try
            {
                var result = await _offerService.GetRatingForCustomer(userId);
                response.Data = result;

                return Ok(response);

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
                return BadRequest(response);
            }
        }

        [Authorization(Role.User, Role.Admin)]
        [HttpGet("getall/{id}")]
        public async Task<ActionResult<ServiceResponse<List<OfferGetDTO>>>> GetAll([FromRoute] int? id, [FromQuery] int pageNr, [FromQuery] int noElements, [FromQuery] int sortOption)
        {

            if (id == null || id == 0)
            {
                return BadRequest("Id is null or empty !");
            }
            else
            {
                var response = new ServiceResponse<List<OfferGetDTO>>();
                try
                {
                    var allOffers = await _offerService.GetAllOffers((int)id, pageNr, noElements, sortOption);
                    response.Data = allOffers;
                }
                catch (Exception e)
                {
                    response.Success = false;
                    response.Message = e.Message;
                }

                return response;
            }
        }

        [Authorization(Role.User, Role.Admin)]
        [HttpGet("getallacceptedforloggedin")]
        public async Task<ActionResult<ServiceResponse<List<OffersForLoggedInDTO>>>> GetAllAcceptedOffers()
        {
            int id = Convert.ToInt32(HttpContext.User.FindFirstValue("id"));
            if (id == 0)
            {
                return BadRequest("Logged in user is null or empty");
            }
            else
            {
                var response = new ServiceResponse<List<OffersForLoggedInDTO>>();
                try
                {
                    var allJobOffers = await _offerService.GetAllMyAcceptedOffersOrderByDateCreated(id);
                    response.Data = allJobOffers;
                }
                catch (Exception e)
                {
                    response.Success = false;
                    response.Message = e.Message;
                }

                return response;
            }
        }


        [Authorization(Role.User, Role.Admin)]
        [HttpGet("getallforloggedin")]
        public async Task<ActionResult<ServiceResponse<List<OffersForLoggedInDTO>>>> GetAllActiveOffers()
        {
            int id = Convert.ToInt32(HttpContext.User.FindFirstValue("id"));
            if (id == 0)
            {
                return BadRequest("Logged in user is null or empty");
            }
            else
            {
                var response = new ServiceResponse<List<OffersForLoggedInDTO>>();
                try
                {
                    var allOffers = await _offerService.GetAllOffersForLoggedIn(id);
                    response.Data = allOffers;
                }
                catch(Exception e)
                {
                    response.Success = false;
                    response.Message = e.Message;
                }

                return response;
            }
        }

        [Authorization(Role.User, Role.Admin)]
        [HttpGet("total/{id}")]
        public async Task<ActionResult<ServiceResponse<int>>> GetTotalNrOfOffers([FromRoute] int? id)
        {
            var result = new ServiceResponse<int>();

            try
            {

                if (id == null || id == 0)
                {
                    return BadRequest("Id is null or empty !");
                }

                int number = await _offerService.GetTotalNrOfJobOffers((int)id);
                result.Data = number;
                

            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;
                return BadRequest(result);

            }

            return Ok(result);
        }




        [Authorization(Role.Admin, Role.User)]
        [HttpPost("create")]
        public async Task<ActionResult<ServiceResponse<bool>>> Create(OfferCreateDTO offerdto)
        {
            var result = new ServiceResponse<bool>();


            if (offerdto != null)
            {
                try
                {
                    int id = Convert.ToInt32(HttpContext.User.FindFirstValue("id"));
                    if (id == 0)
                    {
                        return BadRequest("Loggedin Id is 0!");
                    }

                    offerdto.CreationUserId = id;
                    await _offerService.Create(offerdto);
                    result.Message = "Offer created!";
                }
                catch (Exception e)
                {
                    result.Message = e.Message;
                    result.Success = false;
                    return BadRequest(result);
                }

                return Ok(result);
            }

            return BadRequest("Object sent for create Offer is null!");


        }

    }
}
