using Handymand.Data;
using Handymand.Models;
using Handymand.Models.DTOs;
using Handymand.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Repository.DatabaseRepositories
{
    public class JobOfferRepository : GenericRepository<JobOffer>, IJobOfferRepository
    {
        private HandymandContext _context;

        public JobOfferRepository(HandymandContext context) : base(context)
        {
            _context = context;
        }


        public async Task<JobOffer> GetById(int Id)
        {

            var result = await _table.Where(j => j.Id == Id).Include("JobOffersSkills.Skill")
                                     .Include("CreationUser")
                                     .Include("City")
                                     .Include("Contract")
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync();
            if(result == null)
            {
                throw new NullReferenceException("There is no job offer with specified id!");
            }

            if(result.Contract != null && result.Contract.Valid == false)
            {
                throw new EntryPointNotFoundException("Acest job offer a fost inchis!");
            }


            return result;
        }

        public async Task AddSkillsOnThisJobOfferAsync(int id, List<int> IdSkills)
        {
            await Task.Run(() =>
            {
                var existingSkills = _context.Skills.Where(s => IdSkills.Contains(s.Id)).ToList();


                foreach (var s in existingSkills)
                {
                    JobOffersSkills jos = new JobOffersSkills();
                    jos.IdJobOffer = id;
                    jos.IdSkill = s.Id;
                    _context.Add<JobOffersSkills>(jos);
                }
            });


            await _context.SaveChangesAsync();

        }

        private bool VerifyNames(string firstName, string lastName, string[] nameSplitted)
        {
            string[] user_fullname = new string[] { firstName.ToLower(), lastName.ToLower() };

            string[] lowerNameSplitted = new string[nameSplitted.Length];

            for(int i = 0; i<nameSplitted.Length; i++)
            {
                lowerNameSplitted[i] = nameSplitted[i].ToLower();
            }


            var intersect = user_fullname.Intersect(lowerNameSplitted);

            if (intersect.Count() == nameSplitted.Length)
            {
                return true;
            }

            return false;

        }


        private bool VerifyKeywords(string description, string[] keywords)
        {
            string[] allDescriptionLower = description.ToLower().Split(' ', '.', '!', ':', ',');

            string[] lowerKeywords = new string[keywords.Length];

            for (int i = 0; i < keywords.Length; i++)
            {
                lowerKeywords[i] = keywords[i].ToLower().Trim();
            }

            return allDescriptionLower.Intersect(lowerKeywords).Any();

        }

        private async Task<List<JobOffer>> GetJobOffersFiltered(FilterJobOffersDTO filter, int? userId = null)
        {
            var filteredList = await _table.Where(j => j.Available == true)
                         .Include("JobOffersSkills.Skill")
                         .Include("CreationUser")
                         .Include("City")
                         .AsNoTracking()
                         .ToListAsync();


            await Task.Run(() =>
            {
                if (userId != null)
                {
                    int id = (int)userId;

                    filteredList = filteredList.Where(j => j.CreationUserId == id).ToList();
                }
                string CreatorName = filter.CreatorName;

                if (CreatorName != "" && CreatorName != null)
                {
                    string[] inputNamesSplitted = filter.CreatorName.TrimStart().TrimEnd().Split(" ");
                    filteredList = filteredList.Where(j => VerifyNames(j.CreationUser.LastName, j.CreationUser.FirstName, inputNamesSplitted)).ToList();
                }


                int? CityId = filter.CityId;
                if (CityId != null && CityId != 0)
                {
                    filteredList = filteredList.Where(j => j.CityId == CityId).ToList();
                }


                string fKeywords = filter.Keywords;

                if (fKeywords != null && fKeywords != "" && fKeywords.Length > 2)
                {
                    var keywords = fKeywords.Split(" ");
                    filteredList = filteredList.Where(j => VerifyKeywords(j.Description, keywords)).ToList();
                }


                List<SkillShortDTO> fSkills = (List<SkillShortDTO>)filter.Skills;

                if (fSkills != null && fSkills.Count() > 0)
                {
                    int[] ids = new int[fSkills.Count()];

                    for (int i = 0; i < fSkills.Count(); i++)
                    {
                        ids[i] = fSkills[i].Id.Value;
                    }


                    var jobOffersIdsForGivenSkills = _context.JobOffersSkills.Where(jos => ids.Contains(jos.IdSkill)).Select(jos => jos.IdJobOffer).ToArray();

                    filteredList = filteredList.Where(j => jobOffersIdsForGivenSkills.Contains(j.Id)).ToList();
                }


                int? lowPriceRange = filter.lowPriceRange;
                if (lowPriceRange.HasValue && lowPriceRange != null)
                {
                    filteredList = filteredList.Where(j => j.LowPriceRange >= lowPriceRange).ToList();
                }

                int? highPriceRange = filter.highPriceRange;
                if (highPriceRange.HasValue && highPriceRange != null)
                {
                    filteredList = filteredList.Where(j => j.HighPriceRange <= highPriceRange).ToList();
                }



            });

            return filteredList;

        }

        public async Task<List<JobOffer>> GetAllJobOffersInclude(int skip, int noElements, FilterJobOffersDTO filter, int? userId = null)
        {

            var filteredList = await GetJobOffersFiltered(filter,userId);



            if (skip < 0 || noElements == 0)
            {
                return filteredList.ToList();
            }
            else
            {
                return filteredList.Skip(skip).Take(noElements).ToList();
            }
        }

        public async Task<int> GetTotalNrOfJobOffers(FilterJobOffersDTO filter, int? userId = null)
        {
            var filteredList = await GetJobOffersFiltered(filter, userId);

            return filteredList.Count();
        }

        public async Task<List<CityShortDTO>> GetAllCities()
        {
            return await _context.Cities.Select(c => new CityShortDTO()
            {
                Id = c.Id,
                Name = c.Name
            }).ToListAsync();
        }

        public async Task<List<JobOffer>> GetAllActiveJobOffersForLoggedIn(int id)
        {
            return await _table.Where(j => j.CreationUserId == id && j.Available == true).OrderByDescending(j => j.DateCreated).ToListAsync();
        }

        public async Task<List<JobOffer>> GetAllPendingJobOffersForLoggedInOrderByDateCreate(int id)
        {
            try
            {
                return await _table.Where(j => j.CreationUserId == id && j.Available == false).OrderByDescending(j => j.DateCreated).Include("Contract").ToListAsync();
            }
            catch(Exception e)
            {
                throw;
            }
            
        }

        public async Task<Contract> CloseContract(int idJoboffer, int loggedinId, int feedbackVal)
        {
            var jobOffer = await _table.Include("Contract").FirstAsync(jo => jo.Id == idJoboffer);
            if (jobOffer == null)
            {
                throw new Exception("There is no joboffer with specified id!");
            }

            if (jobOffer.CreationUserId != loggedinId)
            {
                throw new Exception("Joboffer found by specified id has different creation user than logged in user!");

            }

            Contract contract = jobOffer.Contract;
            if (contract == null)
            {
                return null;
            }

            contract.Valid = false;
            _context.Contracts.Update(contract);
            await _context.SaveChangesAsync();

            //Trebuie inchisa oferta.

            Feedback feedback = new Feedback();
            feedback.CreationUserId = loggedinId;
            feedback.Grade = feedbackVal;
            feedback.FromClientToFreelancer = true;
            feedback.FromFreelancerToClient = false;
            feedback.RefferedUserId = jobOffer.Contract.RefferedUserId;
            feedback.DateCreated = DateTime.Now;
            feedback.JobOfferId = jobOffer.Id;

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            //Trebuie sterse toate notificarile care au fost create pe acest jobOffer


            await CreateContractNotifications(loggedinId, jobOffer);

            DeleteJobOfferFolder(jobOffer.Id);
            await DeleteAllNotificationsForJobOffer(jobOffer.Id);

            return contract;


        }
        
        private async Task DeleteAllNotificationsForJobOffer(int idJobOffer)
        {
            var notifications = await _context.Notifications.Where(n => n.JobOfferId == idJobOffer && n.NotificationTypeId != 3).ToListAsync();

            _context.Notifications.RemoveRange(notifications);
            await _context.SaveChangesAsync();
        }


        private async Task CreateContractNotifications(int loggedinId, JobOffer jobOffer)
        {
            Notification notification = new Notification();
            notification.CreationUserId = loggedinId;
            notification.JobOfferId = jobOffer.Id;
            notification.ReferredUserId = jobOffer.Contract.RefferedUserId;
            notification.Viewed = false;
            notification.NotificationTypeId = 3;
            notification.DateCreated = DateTime.Now;

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();
        }

        public async Task<Feedback> SendFeedback(int idJoboffer, int loggedinId, int feedbackVal)
        {
            var jobOffer = await _table.Include("Contract").FirstAsync(jo => jo.Id == idJoboffer);
            if (jobOffer == null)
            {
                throw new Exception("There is no joboffer with specified id!");
            }



            Contract contract = jobOffer.Contract;
            if (contract == null)
            {
                return null;
            }

            if (jobOffer.Contract.RefferedUserId != loggedinId)
            {
                throw new Exception("Contract found by reffered user id has different reffered user than logged in user!");

            }

            Feedback feedback = new Feedback();
            feedback.CreationUserId = loggedinId;
            feedback.Grade = feedbackVal;
            feedback.FromClientToFreelancer = false;
            feedback.FromFreelancerToClient = true;
            feedback.RefferedUserId = jobOffer.Contract.CreationUserId;
            feedback.DateCreated = DateTime.Now;
            feedback.JobOfferId = jobOffer.Id;

            try
            {
                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                throw;
            }


            return feedback;


        }


        public async Task<bool> DeleteJobOffer(int idJobOffer, int loggedinId)
        {
            var jobOffer = await _table.Include("Contract").FirstAsync(jo => jo.Id == idJobOffer);
            if (jobOffer == null)
            {
                throw new Exception("There is no joboffer with specified id!");
            }

            if (jobOffer.CreationUserId != loggedinId)
            {
                throw new Exception("Joboffer found by specified id has different creation user than logged in user!");

            }

            if(jobOffer.Contract != null)
            {
                throw new Exception("There is an open contract for this job offer!");
            }

            var OffersList = await _context.Offers.Where(o => o.JobOfferId == jobOffer.Id).ToListAsync();
            bool offersDeleted = false;
            try
            {
                _context.Offers.RemoveRange(OffersList);
                await _context.SaveChangesAsync();
                offersDeleted = true;
            }
            catch(Exception e)
            {
                throw;
            }


            if (offersDeleted)
            {
                try
                {
                    _table.Remove(jobOffer);
                    await _context.SaveChangesAsync();

                    await DeleteAllNotificationsForJobOffer(jobOffer.Id);

                    //Delete all files:
                    int jobOfferId = jobOffer.Id;

                    DeleteJobOfferFolder(jobOfferId);

                }
                catch (Exception e)
                {
                    throw;
                }

                return true;

            }
            else
            {
                return false;
            }



        }

        private void DeleteJobOfferFolder(int jobOfferId)
        {
            string folderPath = "JobOffers_Images\\" + jobOfferId;
            string currentDirectory = Directory.GetCurrentDirectory();
            var folderPathComplete = Path.Combine(currentDirectory, folderPath);
            if (Directory.Exists(folderPathComplete))
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(folderPathComplete);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }

        }

        public async Task<string> GetCustomerName(int idJobOffer)
        {
            var jobOffer = await _table.Include("Contract.CreationUser").FirstAsync(jo => jo.Id == idJobOffer);
            if (jobOffer == null)
            {
                throw new Exception("There is no joboffer with specified id!");
            }


            Contract contract = jobOffer.Contract;
            if (contract == null)
            {
                throw new Exception("There is no contract for this Job Offer!");

            }

            return jobOffer.Contract.CreationUser.LastName + " " + jobOffer.Contract.CreationUser.FirstName;
           
        }

        public async Task<string> GetFreelancerName(int idJobOffer)
        {
            var jobOffer = await _table.Include("Contract.RefferedUser").FirstAsync(jo => jo.Id == idJobOffer);
            if (jobOffer == null)
            {
                throw new Exception("There is no joboffer with specified id!");
            }

            Contract contract = jobOffer.Contract;
            if (contract == null)
            {
                throw new Exception("There is no contract for this Job Offer!");

            }

            return jobOffer.Contract.RefferedUser.LastName + " " + jobOffer.Contract.RefferedUser.FirstName;

        }

        public async Task<List<JobOffer>> GetAllClosedForFeedback(int id)
        {
            //Extragem toate JobOfferIds din Feedback pe care acest freelancer a primit feedback.

            var all_JobOfferIds_On_Feedbacks_RefferedToThisFreelancer = await _context.Feedbacks.Where(f => f.RefferedUserId == id && f.FromClientToFreelancer == true).Select(f => f.JobOfferId).ToListAsync();

            //Extragem toate JobOfferIds din Feedback pe care freelancerul a dat feedback.

            var all_JobOfferIds_On_Feedbacks_CreatedByThisFreelancer = await _context.Feedbacks.Where(f => f.CreationUserId == id && f.FromFreelancerToClient == true).Select(f => f.JobOfferId).ToListAsync();


            //Ramanem doar cu feedback-ul pe care freelancerul l-a primit

            var jobOfferIds_On_Which_ThisFreelancer_Only_GetFeeback = all_JobOfferIds_On_Feedbacks_RefferedToThisFreelancer.Where(fr => !all_JobOfferIds_On_Feedbacks_CreatedByThisFreelancer.Any(fc => fc == fr)).ToList();


            //Verificam daca pe toate aceste jobOfferIds exista un contract inchis

            var closed_Contracts_Refered_To_This_Freelancer = await _context.Contracts.Where(c => c.RefferedUserId == id && c.Valid == false).Select(c => c.JobOfferId).ToListAsync();

            var jobOffersId_WithClosedContract = new List<int>();

            foreach(var j in jobOfferIds_On_Which_ThisFreelancer_Only_GetFeeback)
            {
                if (closed_Contracts_Refered_To_This_Freelancer.Contains(j))
                {
                    jobOffersId_WithClosedContract.Add(j);
                }
            }

            //Extragem toate jobofferurile cu aceste Id-uri

            var final_JobOffers = await _table.Where(i => jobOffersId_WithClosedContract.Contains(i.Id)).Include("CreationUser").ToListAsync();

            return final_JobOffers;



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
