using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.One_to_Many
{
    public class Model2OM: BaseEntity
    {

        public Model1OM Model1OM { get; set; }

        public int Model1Id { get; set; }
    }
}
