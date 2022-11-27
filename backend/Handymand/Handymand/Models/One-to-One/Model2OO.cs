using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.One_to_One
{
    public class Model2OO : BaseEntity
    {
        public Model1OO Model1OO { get; set; }

        public int Model1Id { get; set; }
    }
}
