using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.Many_to_Many
{
    public class ModelsRelation
    {
        public int Model1MMId { get; set; }
        public Model1MM Model1MM { get; set; }

        public int Model2MMId { get; set; }
        public Model2MM Model2MM { get; set; }

        public string Nume { get; set; }
    }
}
