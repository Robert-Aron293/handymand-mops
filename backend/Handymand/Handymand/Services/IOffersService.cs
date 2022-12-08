using Handymand.Models;
using Handymand.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Handymand.Services
{
    public interface IOffersService
    {
        Task<List<OfferGetDTO>> GetAllOffers(int id,int pageNr, int noElements, int sortOption);
        Task<bool> Create(OfferCreateDTO offerdto);

        Task<int> GetTotalNrOfJobOffers(int id);

        Task<List<OffersForLoggedInDTO>> GetAllOffersForLoggedIn(int id);
        Task<List<OffersForLoggedInDTO>> GetAllMyAcceptedOffersOrderByDateCreated(int id);

        Task<bool> AcceptOffer(AcceptOfferDTO dto, int loggedInId);

        Task<RatingDTO> GetRatingForFreelancer(int userId);
        Task<RatingDTO> GetRatingForCustomer(int userId);



    }
}
