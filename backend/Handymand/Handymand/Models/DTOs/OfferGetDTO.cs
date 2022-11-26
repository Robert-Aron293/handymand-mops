using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.DTOs
{
    public class OfferGetDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double PaymentAmount { get; set; }
        public DateTime DateCreated { get; set; }

        public int CreationUserId { get; set; }
        public string CreationUserName { get; set; }
        public string CreationUserTitle { get; set; }
        public int CreationUserAge { get; set; }

        public bool Available { get; set; }
    }
}
