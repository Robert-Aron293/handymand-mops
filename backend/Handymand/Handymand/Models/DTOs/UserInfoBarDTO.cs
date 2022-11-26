using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.DTOs
{
    public class UserInfoBarDTO
    {
        public int OpenedContracts { get; set; }
        public int ClosedContracts { get; set; }
        public int OpenedOffers { get; set; }
        public int OpenedJobOffers { get; set; }
    }
}
