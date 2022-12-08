using Handymand.Models;
using Handymand.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Repository.DatabaseRepositories
{
    public interface IContractsSkillsRepository
    {
        Task<List<ContractsSkills>> GetAll();

        IQueryable<ContractsSkills> GetAllAsQueryable();


        //Create
        void Create(ContractsSkills entity);

        Task CreateAsync(ContractsSkills entity);

        void CreateRange(IEnumerable<ContractsSkills> entities);

        Task CreateRangeAsync(IEnumerable<ContractsSkills> entities);

        //Update
        void Update(ContractsSkills entity);

        void UpdateRange(IEnumerable<ContractsSkills> entities);

        //Delete

        void Delete(ContractsSkills entity);

        void DeleteRange(IEnumerable<ContractsSkills> entities);

        //Find

        ContractsSkills FindById(int id);

        Task<ContractsSkills> FindByIdAsync(int id);

        //Save

        bool Save();

        Task<bool> SaveAsync();
    }
}
