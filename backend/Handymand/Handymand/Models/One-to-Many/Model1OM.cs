using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.One_to_Many
{
    public class Model1OM: BaseEntity
    {
        
        public ICollection<Model2OM> Models2OM { get; set; }
    }
}
