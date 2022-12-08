using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Handymand.Data;
using Handymand.Models;
using Handymand.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Handymand.Repository.DatabaseRepositories
{
    public class ContractRepository: GenericRepository<Contract>, IContractRepository
    {
        public ContractRepository(HandymandContext context): base(context)
        {

        }

        public List<Contract> GetAllWithInclude()
        {
            return _table
                .Include(x => x.CreationUser)
                .ToList();
        }

        public Contract GetById(int Id)
        {
            return _table.Where(i => i.Id == Id).FirstOrDefault();
        }

        public Contract GetByIdIncludingAll(int Id)
        {
            return _table.Where(i => i.Id == Id)
                .Include(x => x.CreationUser).FirstOrDefault();

        }
    }
}
