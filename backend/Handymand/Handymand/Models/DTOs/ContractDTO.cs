using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.DTOs
{
    public class ContractDTO
    {
        public int Id { get; set; }
        public string CreationUserFullName { get; set; }
        public int CreationUserId { get; set; }

        public string RefferedUserFullName { get; set; }
        public int RefferedUserId { get; set; }

        public DateTime? CreationDate { get; set; }
        //Daca nu are referred user atunci avem valoare aici o CreationDate + o luna
        public DateTime? ExpirationDate { get; set; }

        public int ExpectedDurationInHours { get; set; }

        public double PaymentAmountPerHour { get; set; }


    }
}
