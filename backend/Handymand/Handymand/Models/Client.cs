using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models
{
    public class Client:BaseEntity
    {
        public string Location { get; set; }
        
        public string Rating { get; set; }

        public User User { get; set; }

        public int IdUser { get; set; }
    }
}
