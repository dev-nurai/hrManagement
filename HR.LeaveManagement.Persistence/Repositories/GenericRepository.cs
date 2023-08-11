using HR.LeaveManagement.Application.Contracts.Persistance;
using HR.LeaveManagement.Domain.Common;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly HrDatabaseContext _hrDatabaseContext;

        public GenericRepository(HrDatabaseContext hrDatabaseContext)
        {
            _hrDatabaseContext = hrDatabaseContext;
        }

        public async Task CreateAsync(T entity)
        {
            await _hrDatabaseContext.AddAsync(entity);
            await _hrDatabaseContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _hrDatabaseContext.Remove(entity);
            await _hrDatabaseContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync()
        {
            return await _hrDatabaseContext.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _hrDatabaseContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(T entity)
        {
            //_hrDatabaseContext.Update(entity); //First way to update
            _hrDatabaseContext.Entry(entity).State = EntityState.Modified; //Second way to update using State
            await _hrDatabaseContext.SaveChangesAsync();
        }
    }
}
