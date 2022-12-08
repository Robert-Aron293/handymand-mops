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
    public class OffersRepository:GenericRepository<Offer>, IOffersRepository
    {
        private readonly HandymandContext _context;

        public OffersRepository(HandymandContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Offer> FinByIdUserAndIdJobOffer(OfferCreateDTO dto)
        {
            return await _table.Where(o => o.CreationUserId == dto.CreationUserId && o.JobOfferId == dto.JobOfferId).FirstOrDefaultAsync();

        }

        public async Task<List<Offer>> GetAllOffersIncludeUsers(int id, int skip, int noElements, int sortOption)
        {
            var query = _table.Where(o => o.JobOfferId == id).Include("CreationUser").OrderByDescending(o => o.Available);

            switch (sortOption)
            {
                case 1:
                    {
                        query = query.OrderByDescending(o => o.DateCreated);
                        break;
                    }
                case 2:
                    {
                        query = query.OrderBy(o => o.DateCreated);
                        break;
                    }
                case 3:
                    {
                        query = query.OrderByDescending(o => o.PaymentAmount);
                        break;
                    }
                case 4:
                    {
                        query = query.OrderBy(o => o.PaymentAmount);
                        break;
                    }
            }

            if (skip < 0 || noElements == 0)
            {
                return await query.ToListAsync();
            }
            else
            {
                return await query.Skip(skip).Take(noElements).ToListAsync();
            }

        }

        public async Task<int> GetTotalNrOfJobOffers(int id)
        {
            return await _table.CountAsync(j => j.JobOfferId == id);

        }

        public async Task<List<Offer>> GetAllOffersForLoggedIn(int id)
        {
            return await _table.Where(o => o.CreationUserId == id && o.Available == true).Include("JobOffer").Where(o => o.JobOffer.Available == true).OrderByDescending(o => o.DateCreated).ToListAsync();
        }

        public async Task<bool> JobOfferCreationUserTryToCreateOffer(OfferCreateDTO dto)
        {
            var jobOffer = await _context.JobOffer.Where(jo => jo.Id == dto.JobOfferId).FirstOrDefaultAsync();
            if (jobOffer != null)
            {
                int? creationIdJo = jobOffer.CreationUserId;
                if (creationIdJo != null)
                {
                    if(creationIdJo == dto.CreationUserId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    throw new Exception("Exception when try to verify if job offer creation user is trying to create an offer. Creation User Id for Job Offer is null");

                }
            }
            else
            {
                throw new Exception("Exception when try to verify if job offer creation user is trying to create an offer. JobOffer is null !");

            }
        }

        public async Task<bool> AcceptOffer(AcceptOfferDTO dto, int loggedInId)
        {
            var jobOffer = await _context.JobOffer.Where(jo => jo.Id == dto.JobOfferId).FirstOrDefaultAsync();
        
            if(jobOffer == null)
            {
                throw new Exception("There is no job offer with specified id!");
            }

            if(jobOffer.CreationUserId != loggedInId)
            {
                throw new Exception("Creation Id of Job Offer differ from logged in id !");
            }

            var existentOffer = await FindByIdAsync(dto.OfferId);
            if (existentOffer == null)
            {
                throw new Exception("There is no offer with this id!");
            }

            //Cream un Contract!

            Contract contract = new Contract();
            contract.CreationUserId = loggedInId;
            contract.RefferedUserId = existentOffer.CreationUserId;
            contract.PaymentAmountPerHour = existentOffer.PaymentAmount;
            contract.DateCreated = DateTime.Now;
            contract.JobOfferId = jobOffer.Id;
            contract.Valid = true;

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();


            //Setam toate ofertele de pe ace job offer pe unavailable cu exceptia asteia

            var allOffers = await _context.Offers.Where(o => o.Id != dto.OfferId && o.JobOfferId == dto.JobOfferId).ToListAsync();

            allOffers.ForEach(o =>
            {
                o.Available = false;
                o.DateModified = DateTime.Now;
            });

            _table.UpdateRange(allOffers);
            await _context.SaveChangesAsync();


            //Setam job-offer-ul pe unavailable
            jobOffer.Available = false;
            jobOffer.DateModified = DateTime.Now;
            _context.JobOffer.Update(jobOffer);
            await _context.SaveChangesAsync();

            //Notificam userul caruia i-am acceptat oferta
            await AcceptOfferNotifications(loggedInId, jobOffer.Id, existentOffer.CreationUserId);

            return true;

        
        }

        public async Task<List<Offer>> GetAllMyAcceptedOffersOrderByDateCreated(int id)
        {
            return await _table.Where(o => o.CreationUserId == id && o.Available == true).Include("JobOffer").Include("JobOffer.Contract").OrderByDescending(o => o.DateCreated).ToListAsync();
        }

        public async Task<RatingDTO> GetRatingForFreelancer(int userId)
        {
            var query = await _context.Feedbacks.Where(f => f.RefferedUserId == userId && f.FromClientToFreelancer == true).ToListAsync();
            var result = new RatingDTO();
            result.Grade = 0;
            result.NrOfFeedbacks = 0;

            int length = query.Count;
            
            if(length > 0)
            {
                double mediumGrade = 0;

                for (int i = 0; i < length; i++)
                {
                    mediumGrade += query[i].Grade;
                }

                mediumGrade = mediumGrade / length;

                result.Grade = Math.Round(mediumGrade,1);
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

                result.Grade = Math.Round(mediumGrade, 1);
                result.NrOfFeedbacks = length;
            }

            return result;

        }

        public async Task CreateOfferNotifications(int creationUserId, int jobOfferId)
        {
            //Lista de Useri pentru care trebuie sa cream notificare
            // Sunt cei care au oferte active pe acest job offer

            //Plus creatorul job offerului.

            var notifiedUsers = await _table.Where(o => o.JobOfferId == jobOfferId && o.CreationUserId != creationUserId).Select(o => o.CreationUserId).ToListAsync();

            int? customer = await _context.JobOffer.Where(j => j.Id == jobOfferId).Select(j => j.CreationUserId).FirstOrDefaultAsync();

            if(customer == null)
            {
                throw new NullReferenceException("Nu exista un creator pentru acest job offer!");
            }


            notifiedUsers.Add((int)customer);

            //Parcurgem intreaga lista de utilizatori si le cream notificare fiecaruia
            for(int i = 0; i< notifiedUsers.Count; i++)
            {
                Notification notification = new Notification();
                notification.CreationUserId = creationUserId;
                notification.JobOfferId = jobOfferId;
                notification.ReferredUserId = notifiedUsers[i];
                notification.Viewed = false;
                notification.NotificationTypeId = 1;
                notification.DateCreated = DateTime.Now;

                _context.Notifications.Add(notification);
            }

            await _context.SaveChangesAsync();



        }

        public async Task AcceptOfferNotifications(int creationUserId, int jobOfferId, int referredUserId)
        {
            Notification notification = new Notification();
            notification.CreationUserId = creationUserId;
            notification.JobOfferId = jobOfferId;
            notification.ReferredUserId = referredUserId;
            notification.Viewed = false;
            notification.NotificationTypeId = 2;
            notification.DateCreated = DateTime.Now;

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetAllNotification(int notificationTypeId, int jobOfferId, int userId)
        {
           return await _context.Notifications.Where(n => n.JobOfferId == jobOfferId &&
                                              n.NotificationTypeId == notificationTypeId &&
                                              n.ReferredUserId == userId &&
                                              n.Viewed == false).CountAsync();
        }
    }
}
