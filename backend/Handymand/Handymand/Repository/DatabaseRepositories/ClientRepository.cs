using Handymand.Data;
using Handymand.Models;
using Handymand.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Repository.DatabaseRepositories
{
    public class ClientRepository:GenericRepository<Client>,IClientRepository
    {

        public ClientRepository(HandymandContext context): base(context)
        {

        }
    }
}
