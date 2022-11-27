using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.Many_to_Many
{
    public class Model2MM:BaseEntity
    {
        //public ICollection<Model1MM> Model1MMs { get; set; }
        public ICollection<ModelsRelation> ModelsRelations { get; set; }
    }
}
