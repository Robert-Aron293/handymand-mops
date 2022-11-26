using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Models.Base
{
    public class BaseEntity: IBaseEntity
    {
        [Key]
        //Generates a values when a row is inserted
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

/*        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]*/
        public DateTime DateCreated { get; set; }

/*        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]*/
        public DateTime? DateModified { get; set; }
    }
}
