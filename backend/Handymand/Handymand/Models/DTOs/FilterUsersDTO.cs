using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.DTOs
{
    public class FilterUsersDTO
    {
        public string Name { get; set; }
        public int? RatingAsFreelancer { get; set; }
        public int? RatingAsCustomer { get; set; }
    }
}
