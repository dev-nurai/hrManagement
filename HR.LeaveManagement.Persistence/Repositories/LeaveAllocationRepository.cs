using HR.LeaveManagement.Application.Contracts.Persistance;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
    {
        public LeaveAllocationRepository(HrDatabaseContext hrDatabaseContext) : base(hrDatabaseContext)
        {

        }

        public async Task AddAllocations(List<LeaveAllocation> allocations)
        {
            await _hrDatabaseContext.AddRangeAsync(allocations);
            await _hrDatabaseContext.SaveChangesAsync();
        }

        public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period)
        {
            return await _hrDatabaseContext.LeaveAllocations
                .AnyAsync(z => z.EmployeeId == userId && z.LeaveTypeId == leaveTypeId && z.Period == period);
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
        {
            var leaveAllocations = await _hrDatabaseContext.LeaveAllocations
                .Include(z => z.LeaveType)
                .ToListAsync();

            return leaveAllocations;
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId)
        {
            var leaveAllocations = await _hrDatabaseContext.LeaveAllocations
                .Where(x => x.EmployeeId == userId)
                .Include(z => z.LeaveType)
                .ToListAsync();

            return leaveAllocations;
        }

        public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
        {
            var leaveAllocation = await _hrDatabaseContext.LeaveAllocations
                .Include(z => z.LeaveType)
                .FirstOrDefaultAsync(x => x.Id == id);

            return leaveAllocation;
        }

        public async Task<LeaveAllocation> GetUserAllocations(string userId, int leaveTypeId)
        {
            return await _hrDatabaseContext.LeaveAllocations
                .FirstOrDefaultAsync(x => x.EmployeeId == userId && x.LeaveTypeId == leaveTypeId);
        }
    }
}
