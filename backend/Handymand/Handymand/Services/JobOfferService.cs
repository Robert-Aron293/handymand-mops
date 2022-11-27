using Handymand.Models;
using Handymand.Models.DTOs;
using Handymand.Repository.DatabaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Handymand.Services
{
    public class JobOfferService: IJobOfferService
    {
        private IJobOfferRepository _jobOfferRepository;
        public JobOfferService(IJobOfferRepository jobOfferRepository)
        {
            _jobOfferRepository = jobOfferRepository;

        }
        
        public JobOffer ConvertFromDTO(JobOfferDTO dto)
        {
            JobOffer jobOffer = new JobOffer();

            jobOffer.Description = dto.Description;
            jobOffer.Location = dto.Location;
            jobOffer.LowPriceRange = dto.LowPriceRange;
            jobOffer.HighPriceRange = dto.HighPriceRange;
            jobOffer.Title = dto.Title;
            jobOffer.CreationUserId = dto.IdCreationUser;
            jobOffer.CityId = dto.CityId;

            return jobOffer;
        }

        public JobOfferQuickViewDTO ConvertToJobOfferQuickViewDTO(JobOffer jobOffer)
        {
            var result = new JobOfferQuickViewDTO();
            result.Id = jobOffer.Id;
            result.Title = jobOffer.Title;
            result.DateCreated = jobOffer.DateCreated;

            return result;
        }

        public JobOfferDTO ConvertToDTO(JobOffer jobOffer)
        {
            JobOfferDTO dto = new JobOfferDTO();

            dto.IdJobOffer = jobOffer.Id;
            dto.IdCreationUser = (int)jobOffer.CreationUserId;
            if (jobOffer.CreationUser != null)
            {
                dto.FirstName = jobOffer.CreationUser.FirstName;
                dto.LastName = jobOffer.CreationUser.LastName;
                dto.Email = jobOffer.CreationUser.LastName;
            }
            dto.Description = jobOffer.Description;
            dto.Location = jobOffer.Location;
            dto.LowPriceRange = jobOffer.LowPriceRange;
            dto.HighPriceRange = jobOffer.HighPriceRange;
            dto.DateCreated = jobOffer.DateCreated;
            dto.Title = jobOffer.Title;
            dto.NoImages = jobOffer.NoImages;
            
            return dto;
        }

        public async Task<int> GetTotalNrOfJobOffers(FilterJobOffersDTO filter, int? userId = null)
        {
            return await _jobOfferRepository.GetTotalNrOfJobOffers(filter,userId);
        }


        public async Task<List<byte[]>> GetImages(int Id)
        {
            var response = new List<byte[]>();

            string folderPath = "JobOffers_Images\\" + Id;
            string currentDirectory = Directory.GetCurrentDirectory();
            var folderPathComplete = Path.Combine(currentDirectory, folderPath);



            if (Directory.Exists(folderPathComplete))
            {
                string[] allFiles = Directory.GetFiles(folderPathComplete);


                foreach (var file in allFiles)
                {
                        byte[] arr = await File.ReadAllBytesAsync(file);
                        response.Add(arr);
                }

            }


            return response;
        }


        public async Task<List<JobOfferDTO>> GetAllJobOffers(int pageNr, int noElements, FilterJobOffersDTO filter, int? userId = null)
        {

            
            int skip = (pageNr) * noElements;

            var initialList = await _jobOfferRepository.GetAllJobOffersInclude(skip,noElements,filter, userId);

            var result = new List<JobOfferDTO>();

            await Task.Run(() =>
            {
                foreach (var joboffer in initialList)
                {
                    var jobdto = new JobOfferDTO()
                    {
                        IdJobOffer = joboffer.Id,
                        IdCreationUser = joboffer.CreationUserId,
                        FirstName = joboffer.CreationUser != null ? joboffer.CreationUser.FirstName : null,
                        LastName = joboffer.CreationUser != null ? joboffer.CreationUser.LastName : null,
                        Email = joboffer.CreationUser != null ? joboffer.CreationUser.Email : null,
                        Description = joboffer.Description,
                        Location = joboffer.Location,
                        Title = joboffer.Title,
                        LowPriceRange = joboffer.LowPriceRange,
                        HighPriceRange = joboffer.HighPriceRange,
                        DateCreated = joboffer.DateCreated,
                        NoImages = joboffer.NoImages
                    };

                    if(joboffer.JobOffersSkills.Count > 0)
                    {
                        jobdto.Skills = joboffer.JobOffersSkills.Select(jos => new SkillShortDTO()
                        {
                            Id = jos.IdSkill,
                            SkillName = jos.Skill.SkillName
                        }).ToList();
                    }
                    if(joboffer.City != null)
                    {
                        jobdto.City = new CityShortDTO()
                        {
                            Id = joboffer.City.Id,
                            Name = joboffer.City.Name
                        };
                    }

                    result.Add(jobdto);
                };
            });


            return result;
        }

        public async Task<ServiceResponse<JobOfferDTO>> Create(JobOfferDTO dto)
        {
            var response = new ServiceResponse<JobOfferDTO>();

            JobOffer jobOffer = ConvertFromDTO(dto);
            jobOffer.Available = true;
            

            jobOffer.DateCreated = DateTime.Now;
            if(dto.Files != null)
            {
                jobOffer.NoImages = dto.Files.Count;
            }


            await _jobOfferRepository.CreateAsync(jobOffer);
            await _jobOfferRepository.SaveAsync();

            if(jobOffer.Id != 0)
            {
                response.Data = ConvertToDTO(jobOffer);
                if(dto.Files != null)
                {
                    try
                    {
                        await SaveJobOffersImages(dto.Files, jobOffer.Id);
                    }
                    catch (Exception e)
                    {
                        response.Message = e.Message;
                    }
                }

                try
                {
                    if(dto.IdSkills != null && dto.IdSkills.Count > 0)
                    {
                        await _jobOfferRepository.AddSkillsOnThisJobOfferAsync(jobOffer.Id, dto.IdSkills);

                    }
                }
                catch(Exception e)
                {
                    response.Message += "... Exception when bind joboffer with skills : " + e.Message + "\n";
                }

            }
            else
            {
                response.Success = false;
            }

            return response;
        }

        public async Task SaveJobOffersImages(List<IFormFile> files, int Id)
        {

            string folderPath = "JobOffers_Images\\" + Id;
            string currentDirectory = Directory.GetCurrentDirectory();
            var folderPathComplete = Path.Combine(currentDirectory, folderPath);


            Directory.CreateDirectory(folderPathComplete);

            //Aici e path-ul folderului cu numele ID-ului jobOfferului
            //Mai jos generam numele fiecarui file

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    var filePath = Path.Combine(folderPathComplete,formFile.FileName);

                    if (formFile.Length > 0)
                    {

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }  
                }
            }

        }

        public async Task<ServiceResponse<JobOfferDTO>> GetById(int Id)
        {
            var initialQuery = await _jobOfferRepository.GetById(Id);

            var result = new ServiceResponse<JobOfferDTO>();

            if(initialQuery != null)
            {

                var jobdto = new JobOfferDTO()
                {
                    IdJobOffer = initialQuery.Id,
                    IdCreationUser = initialQuery.CreationUserId,
                    FirstName = initialQuery.CreationUser != null ? initialQuery.CreationUser.FirstName : null,
                    LastName = initialQuery.CreationUser != null ? initialQuery.CreationUser.LastName : null,
                    Email = initialQuery.CreationUser != null ? initialQuery.CreationUser.Email : null,
                    Description = initialQuery.Description,
                    Location = initialQuery.Location,
                    Title = initialQuery.Title,
                    LowPriceRange = initialQuery.LowPriceRange,
                    HighPriceRange = initialQuery.HighPriceRange,
                    DateCreated = initialQuery.DateCreated,
                    NoImages = initialQuery.NoImages,
                    Available = initialQuery.Available
                };

                if (initialQuery.JobOffersSkills.Count > 0)
                {
                    jobdto.Skills = initialQuery.JobOffersSkills.Select(jos => new SkillShortDTO()
                    {
                        Id = jos.IdSkill,
                        SkillName = jos.Skill.SkillName
                    }).ToList();
                }
                if (initialQuery.City != null)
                {
                    jobdto.City = new CityShortDTO()
                    {
                        Id = initialQuery.City.Id,
                        Name = initialQuery.City.Name
                    };
                }


                result.Data = jobdto;
                return result;
            }
            else
            {
                result.Success = false;
                result.Message = "There is no JobOffer with given id.";
                return result;
            }


        }


        public async Task<List<CityShortDTO>> GetAllCities()
        {
            return await _jobOfferRepository.GetAllCities();
        }

        public async Task<List<JobOfferQuickViewDTO>> GetAllActiveJobOffersForLoggedIn(int id)
        {
            var jobList = await _jobOfferRepository.GetAllActiveJobOffersForLoggedIn(id);

            var result = new List<JobOfferQuickViewDTO>();

            foreach(var joboffer in jobList)
            {
                var addDto = ConvertToJobOfferQuickViewDTO(joboffer);

                var allNotificationOnThisJobOffer = await _jobOfferRepository.GetAllNotification(1, joboffer.Id, id);

                //A vazut notificarea daca nu are nicio notificare
                if (allNotificationOnThisJobOffer == 0)
                {
                    addDto.Viewed = true;
                }
                else
                {
                    addDto.Viewed = false;
                    addDto.NotViewedNotifications = allNotificationOnThisJobOffer;
                }

                result.Add(addDto);
            }

            return result;


        }

        public async Task<List<JobOfferQuickViewDTO>> GetAllPendingJobOffersForLoggedInOrderByDateCreate(int id)
        {
            var list = await _jobOfferRepository.GetAllPendingJobOffersForLoggedInOrderByDateCreate(id);

            var result = list.Where(jo => jo.Contract.Valid == true).Select(jo => new JobOfferQuickViewDTO()
            {
                Id = jo.Id,
                DateCreated = jo.DateCreated,
                Title = jo.Title
            }).ToList();

            return result;

        }

        public async Task<bool> CloseContract(int idJoboffer, int loggedinId, int feedbackVal)
        {
            var result = await _jobOfferRepository.CloseContract(idJoboffer, loggedinId,feedbackVal);

            if(result == null)
            {
                return false;
            }
            
            if(result.Valid == true)
            {
                return false;
            }

            return true;
            
        }

        public async Task<bool> SendFeedback(int idJoboffer, int loggedinId, int feedbackVal)
        {
            var result = await _jobOfferRepository.SendFeedback(idJoboffer, loggedinId, feedbackVal);

            if (result == null)
            {
                return false;
            }

            if (result.CreationUserId != loggedinId)
            {
                return false;
            }

            return true;

        }

        public async Task<bool> DeleteJobOffer(int idJoboffer, int loggedinId)
        {
            return await _jobOfferRepository.DeleteJobOffer(idJoboffer, loggedinId);
        }

        public async Task<string> GetCustomerName(int idJobOffer)
        {
            return await _jobOfferRepository.GetCustomerName(idJobOffer);
        }

        public async Task<string> GetFreelancerName(int idJobOffer)
        {
            return await _jobOfferRepository.GetFreelancerName(idJobOffer);
        }

        public async Task<List<JobOfferQuickViewDTO>> GetAllClosedForFeedback(int id)
        {
            var final_JobOffers =  await _jobOfferRepository.GetAllClosedForFeedback(id);

            var response = final_JobOffers.Where(jo => jo.CreationUserId != null).Select(jo => new JobOfferQuickViewDTO()
            {
                Id = jo.Id,
                CreationUserId = (int)jo.CreationUserId,
                CreationUserName = jo.CreationUser != null ? jo.CreationUser.LastName + " " + jo.CreationUser.FirstName : null,
                Title = jo.Title,
                DateCreated = jo.DateCreated,


            }).ToList();

            for (int i = 0; i < response.Count; i++)
            {

                var allNotificationOnThisJobOffer = await _jobOfferRepository.GetAllNotification(3, response[i].Id, id);

                //A vazut notificarea daca nu are nicio notificare
                if (allNotificationOnThisJobOffer == 0)
                {
                    response[i].Viewed = true;
                }
                else
                {
                    response[i].Viewed = false;
                    response[i].NotViewedNotifications = allNotificationOnThisJobOffer;
                }
            }

            return response;
        }
    }
}
