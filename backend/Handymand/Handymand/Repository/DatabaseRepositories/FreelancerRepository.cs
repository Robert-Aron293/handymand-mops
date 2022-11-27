using Handymand.Data;
using Handymand.Models;
using Handymand.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Repository.DatabaseRepositories
{
    public class FreelancerRepository:GenericRepository<Freelancer>, IFreelancerRepository
    {
        public FreelancerRepository(HandymandContext context) : base(context)
        {

        }
    }
}
