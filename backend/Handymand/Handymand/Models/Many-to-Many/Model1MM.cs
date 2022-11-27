using Handymand.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.Many_to_Many
{
    public class Model1MM:BaseEntity
    {
       // public ICollection<Model2MM> Model2MMs { get; set; }
        
        public ICollection<ModelsRelation> ModelsRelations { get; set; }
    }
}
