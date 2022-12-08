using Handymand.Data;
using Handymand.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Repository.DatabaseRepositories
{
    public class ContractsSkillsRepository:IContractsSkillsRepository
    {
        private readonly HandymandContext _context;
        protected readonly DbSet<ContractsSkills> _table;

        public ContractsSkillsRepository(HandymandContext context)
        {
            this._context = context;
            _table = context.Set<ContractsSkills>();
        }


        public async Task<List<ContractsSkills>> GetAll()
        {
            return await _table.AsNoTracking().ToListAsync();
        }

        public IQueryable<ContractsSkills> GetAllAsQueryable()
        {
            return _table.AsNoTracking();
        }

        public void Create(ContractsSkills entity)
        {
            _table.Add(entity);
        }

        public void CreateRange(IEnumerable<ContractsSkills> entities)
        {
            _table.AddRange(entities);
        }

        public async Task CreateAsync(ContractsSkills entity)
        {
            await _table.AddAsync(entity);
        }

        public async Task CreateRangeAsync(IEnumerable<ContractsSkills> entities)
        {
            await _table.AddRangeAsync(entities);
        }



        //Update
        public void Update(ContractsSkills entity)
        {
            _table.Update(entity);
        }

        public void UpdateRange(IEnumerable<ContractsSkills> entities)
        {
            _table.UpdateRange(entities);
        }

        //Delete

        public void Delete(ContractsSkills entity)
        {
            _table.Remove(entity);
        }

        public void DeleteRange(IEnumerable<ContractsSkills> entities)
        {
            _table.RemoveRange(entities);
        }

        //Find

        public ContractsSkills FindById(int id)
        {
            return _table.Find(id);
        }

        public async Task<ContractsSkills> FindByIdAsync(int id)
        {
            return await _table.FindAsync(id);
        }

        //Save

        public bool Save()
        {
            try
            {
                return _context.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> SaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
