using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models
{
    public class City: BaseEntity
    {
        public string Name { get; set; }
        public ICollection<JobOffer> JobOffers { get; set; }
    }
}
