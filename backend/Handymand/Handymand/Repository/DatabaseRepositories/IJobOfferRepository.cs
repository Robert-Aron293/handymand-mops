using Handymand.Models;
using Handymand.Models.DTOs;
using Handymand.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Repository.DatabaseRepositories
{
    public interface IJobOfferRepository: IGenericRepository<JobOffer>
    {
        Task<JobOffer> GetById(int Id);
        Task AddSkillsOnThisJobOfferAsync(int id, List<int> IdSkills);
        Task<List<JobOffer>> GetAllJobOffersInclude(int skip, int noElements, FilterJobOffersDTO filter, int? userId = null);
        Task<int> GetTotalNrOfJobOffers(FilterJobOffersDTO filter, int? userId = null);
        Task<List<CityShortDTO>> GetAllCities();
        Task<List<JobOffer>> GetAllActiveJobOffersForLoggedIn(int id);
        Task<List<JobOffer>> GetAllPendingJobOffersForLoggedInOrderByDateCreate(int id);
        Task<List<JobOffer>> GetAllClosedForFeedback(int id);


        Task<Contract> CloseContract(int idJobOffer, int loggedinId,int feedbackVal);
        Task<Feedback> SendFeedback(int idJobOffer, int loggedinId, int feedbackVal);

        Task<bool> DeleteJobOffer(int idJobOffer, int loggedinId);
        Task<string> GetCustomerName(int idJobOffer);
        Task<string> GetFreelancerName(int idJobOffer);
        Task<int> GetAllNotification(int notificationTypeId, int jobOfferId, int userId);
    }
}
