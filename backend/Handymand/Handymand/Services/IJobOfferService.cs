using Handymand.Models;
using Handymand.Models.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Services
{
    public interface IJobOfferService
    {
        Task<List<JobOfferDTO>> GetAllJobOffers(int pageNr, int noElements, FilterJobOffersDTO filter, int? userId = null);

        Task<ServiceResponse<JobOfferDTO>> Create(JobOfferDTO dto);

        Task<ServiceResponse<JobOfferDTO>> GetById(int Id);

        Task SaveJobOffersImages(List<IFormFile> files, int N);
        Task<int> GetTotalNrOfJobOffers(FilterJobOffersDTO filter, int? userId = null);

        Task<List<CityShortDTO>> GetAllCities();
        Task<List<byte[]>> GetImages(int id);

        Task<List<JobOfferQuickViewDTO>> GetAllActiveJobOffersForLoggedIn(int id);
        Task<List<JobOfferQuickViewDTO>> GetAllPendingJobOffersForLoggedInOrderByDateCreate(int id);
        Task<List<JobOfferQuickViewDTO>>  GetAllClosedForFeedback(int id);
        Task<bool> CloseContract(int idJoboffer,int loggedinId, int feedbackVal);
        Task<bool> SendFeedback(int idJoboffer, int loggedinId, int feedbackVal);

        Task<bool> DeleteJobOffer(int idJoboffer, int loggedinId);

        Task<string> GetCustomerName(int idJobOffer);
        Task<string> GetFreelancerName(int idJobOffer);

    }
}
