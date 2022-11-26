using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.DTOs
{
    public class FilterJobOffersDTO
    {
        public string CreatorName { get; set; }
        public int? CityId { get; set; }
        public string Keywords { get; set; }
        public IList<SkillShortDTO> Skills { get; set; }
        public int? lowPriceRange { get; set; }
        public int? highPriceRange { get; set; }
        public bool myJobOffers { get; set; }
    }
}
