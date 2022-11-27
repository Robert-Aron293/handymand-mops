using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.One_to_One
{
    public class Model1OO: BaseEntity
    {
        public Model2OO Model2OO { get; set; }
    }
}
