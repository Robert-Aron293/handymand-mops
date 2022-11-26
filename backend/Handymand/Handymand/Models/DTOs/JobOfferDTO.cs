using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.DTOs
{
    public class JobOfferDTO
    {
        public int? IdJobOffer { get; set; }
        public int? IdCreationUser { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
        public double LowPriceRange { get; set; }
        public double HighPriceRange { get; set; }
        public int? NoImages { get; set; }
        public DateTime DateCreated { get; set; }
        public List<int> IdSkills { get; set; }
        public List<SkillShortDTO> Skills { get; set; }
        public int? CityId { get; set; }
        public CityShortDTO City { get; set; }
        public List<IFormFile> Files { get; set; }
        public bool Available { get; set; }

        public override string ToString()
        {
            return "IdJobOffer :" + IdJobOffer + " ," +
                "IdCreationUser :" + IdCreationUser + " ," +
                "Email :" + Email + " ," +
                "Description :" + Description + " ," +
                "Location :" + Location + " ," +
                "Title :" + Title + " ," +
                "LowPriceRange :" + LowPriceRange + " ," +
                "HighPriceRange :" + HighPriceRange + " ," +
                "Files :" + Files;
        }
    }
}
