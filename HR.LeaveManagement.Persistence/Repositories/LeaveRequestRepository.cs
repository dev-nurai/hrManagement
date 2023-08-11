using HR.LeaveManagement.Application.Contracts.Persistance;
using HR.LeaveManagement.Domain;
using HR.LeaveManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(HrDatabaseContext hrDatabaseContext) : base(hrDatabaseContext)
        {

        }

        public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails()
        {
            var leaveRequests = await _hrDatabaseContext.LeaveRequests
                .Where(x=> !string.IsNullOrEmpty(x.RequestingEmployeeId))
                .Include(x => x.LeaveType)
                .ToListAsync();
            return leaveRequests;
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestsWithDetails(string userId)
        {
            var leaveRequests = await _hrDatabaseContext.LeaveRequests
                .Where(z => z.RequestingEmployeeId == userId)
                .Include(x => x.LeaveType)
                .ToListAsync();

            return leaveRequests;
        }

        public async Task<LeaveRequest> GetLeaveRequestWithDetails(int id)
        {
            var leaveRequest = await _hrDatabaseContext.LeaveRequests
                .Include(x => x.LeaveType)
                .FirstOrDefaultAsync(x => x.Id == id);

            return leaveRequest;
        }
    }
}
