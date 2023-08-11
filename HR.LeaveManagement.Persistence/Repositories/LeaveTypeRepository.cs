using HR.LeaveManagement.Application.Contracts.Persistance;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
    {
        public LeaveTypeRepository(HrDatabaseContext hrDatabaseContext) : base(hrDatabaseContext)
        {

        }

        public async Task<bool> IsLeaveTypeUnique(string name, int id)
        {
            return await _hrDatabaseContext.LeaveTypes.AnyAsync(t => t.Name == name && t.Id != id) == false;
        }
    }
}
