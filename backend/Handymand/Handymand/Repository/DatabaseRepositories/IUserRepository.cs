using Handymand.Models;
using Handymand.Models.DTOs;
using Handymand.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Repository.DatabaseRepositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User> GetById(int Id);

        User GetByIdIncludingAll(int Id);

        List<User> GetAllWithInclude();

        Task<List<User>> GetAllWithoutAdmin();

        Task<User> GetByEmail(string email);

        Task<int>  GetOpenedContracts(int id);
        Task<int>  GetClosedContracts(int id);
        Task<int>  GetOpenedOffers(int id);
        Task<int>  GetOpenedJobOffers(int id);

        Task<int> GetNrOfNotifications(int id);
        Task<bool> ViewNotificationOnJobOffer(int loggedinId, int jobOfferId, int notificationTypeId);


        Task<RatingDTO> GetRatingForFreelancer(int userId);
        Task<RatingDTO> GetRatingForCustomer(int userId);


        // Task<User> UpdateUser(User userForUpdate, User newVariant);
    }
}
