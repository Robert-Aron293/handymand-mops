using Handymand.Models;
using Handymand.Models.DTOs;
using Handymand.Repository.GenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Handymand.Repository.DatabaseRepositories
{
    public interface IOffersRepository:IGenericRepository<Offer>
    {
        Task<List<Offer>> GetAllOffersIncludeUsers(int id, int skip, int noElements, int sortOption);
        Task<Offer> FinByIdUserAndIdJobOffer(OfferCreateDTO dto);
        Task<bool> JobOfferCreationUserTryToCreateOffer(OfferCreateDTO dto);

        Task<int> GetTotalNrOfJobOffers(int id);

        Task<List<Offer>> GetAllOffersForLoggedIn(int id);
        Task<bool> AcceptOffer(AcceptOfferDTO dto, int loggedInId);
        Task<List<Offer>> GetAllMyAcceptedOffersOrderByDateCreated(int id);

        Task<RatingDTO> GetRatingForFreelancer(int userId);

        Task<RatingDTO> GetRatingForCustomer(int userId);

        Task CreateOfferNotifications(int creationUserId, int jobOfferId);
        Task<int> GetAllNotification(int notificationTypeId, int jobOfferId, int userId);

    }
}
