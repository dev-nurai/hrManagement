using HR.LeaveManagement.Application.Contracts.Persistance;
using HR.LeaveManagement.Persistence.DatabaseContext;
using HR.LeaveManagement.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HR.LeaveManagement.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Db Context service
            services.AddDbContext<HrDatabaseContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("HrDatabaseConnectionString"));
            });
            //Adding scoped [GenericRepository] Generic type service adding
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //Adding scoped [LeaveType Repository]
            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();

            //Adding scoped [LeaveAllocation Repository]
            services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();

            //Adding scoped [LeaveRequest Repository]
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();

            return services;
        }
    }
}
