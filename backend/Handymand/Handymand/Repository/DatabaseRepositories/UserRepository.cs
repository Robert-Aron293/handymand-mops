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
    public class UserRepository: GenericRepository<User>, IUserRepository
    {
        private readonly HandymandContext _context;
        public UserRepository (HandymandContext context) : base(context)
        {
            _context = context;
        }

        public List<User> GetAllWithInclude()
        {
            return _table.Include(x => x.FreelancerAccount)
                .Include(x => x.ClientAccount)
                .ToList();
        }

        public async Task<User> GetById(int Id)
        {
            return await _table.Where(i => i.Id == Id).FirstOrDefaultAsync();
        }

        public User GetByIdIncludingAll(int Id)
        {
            return _table.Where(i => i.Id == Id)
                .Include(x => x.ClientAccount)
                .Include(x => x.FreelancerAccount).FirstOrDefault();

        }

        public async Task<User> GetByEmail(string email)
        {
            return await _table.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
        }

        public async Task<List<User>> GetAllWithoutAdmin()
        {
            return await _table.Where(u => u.Role == Role.User).ToListAsync();

        }


        public async Task<RatingDTO> GetRatingForFreelancer(int userId)
        {
            var query = await _context.Feedbacks.Where(f => f.RefferedUserId == userId && f.FromClientToFreelancer == true).ToListAsync();
            var result = new RatingDTO();
            result.Grade = 0;
            result.NrOfFeedbacks = 0;

            int length = query.Count;

            if (length > 0)
            {
                double mediumGrade = 0;

                for (int i = 0; i < length; i++)
                {
                    mediumGrade += query[i].Grade;
                }

                mediumGrade = mediumGrade / length;

                result.Grade = mediumGrade;
                result.NrOfFeedbacks = length;
            }

            return result;

        }

        public async Task<RatingDTO> GetRatingForCustomer(int userId)
        {
            var query = await _context.Feedbacks.Where(f => f.RefferedUserId == userId && f.FromFreelancerToClient == true).ToListAsync();
            var result = new RatingDTO();
            result.Grade = 0;
            result.NrOfFeedbacks = 0;

            int length = query.Count;

            if (length > 0)
            {
                double mediumGrade = 0;

                for (int i = 0; i < length; i++)
                {
                    mediumGrade += query[i].Grade;
                }

                mediumGrade = mediumGrade / length;

                result.Grade = mediumGrade;
                result.NrOfFeedbacks = length;
            }

            return result;

        }



        public async Task<int> GetOpenedContracts(int id)
        {
            return await _context.Contracts.Where(c => c.CreationUserId == id).CountAsync();
        }

        public async Task<int> GetClosedContracts(int id)
        {
            return await _context.Contracts.Where(c => c.CreationUserId == id && c.Valid == false).CountAsync();
        }

        public async Task<int> GetOpenedOffers(int id)
        {
            return await _context.Offers.Where(c => c.CreationUserId == id).CountAsync();
        }

        public async Task<int> GetOpenedJobOffers(int id)
        {
            return await _context.JobOffer.Where(c => c.CreationUserId == id).CountAsync();
        }

        public async Task<int> GetNrOfNotifications(int id)
        {
            return await _context.Notifications.Where(n => n.ReferredUserId == id && n.Viewed == false).CountAsync();
        }

        public async Task<bool> ViewNotificationOnJobOffer(int loggedinId, int jobOfferId, int notificationTypeId)
        {
            var notifications = await _context.Notifications.Where(n => n.ReferredUserId == loggedinId &&
                                                                 n.JobOfferId == jobOfferId &&
                                                                 n.NotificationTypeId == notificationTypeId)
                                                     .ToListAsync();

            if (notifications == null || notifications.Count == 0)
            {
                return false;
            }

            if(notificationTypeId == 3)
            {
                _context.Notifications.RemoveRange(notifications);
                await _context.SaveChangesAsync();
            }
            else
            {
                notifications.ForEach(n => n.Viewed = true);

                _context.Notifications.UpdateRange(notifications);
                await _context.SaveChangesAsync();
            }

            return true;


        }
    }
}
