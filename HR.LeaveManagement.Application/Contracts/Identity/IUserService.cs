using HR.LeaveManagement.Application.Models.Dtos;
using HR.LeaveManagement.Application.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Contracts.Identity
{
    public interface IUserService
    {
        List<Employee> GetEmployees();
        Task<Employee> GetEmployee(string userId);

        Task AddEmployee(EmployeeDto employeeDtos);

        public string UserId { get; }
    }
}
