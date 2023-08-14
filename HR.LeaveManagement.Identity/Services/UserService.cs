using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Dtos;
using HR.LeaveManagement.Application.Models.Identity;
using HR.LeaveManagement.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;

namespace HR.LeaveManagement.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _contextAccessor = httpContextAccessor;
        }

        public string UserId { get => _contextAccessor.HttpContext?.User?.FindFirstValue("uid"); }

        public async Task AddEmployee(EmployeeDto employeeDtos)
        {
            var newEmployee = new ApplicationUser
            {
                FirstName = employeeDtos.FistName,
                LastName = employeeDtos.LastName,
                Email = employeeDtos.Email,
                UserName = employeeDtos.UserName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newEmployee, employeeDtos.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newEmployee, "Employee");
            }
            else
            {
                StringBuilder str = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    str.AppendFormat("* {0}\n", error.Description);
                }
                throw new BadRequestException($"{str}");
            }
        }

        public async Task<Employee> GetEmployee(string userId)
        {
            var employee = await _userManager.FindByIdAsync(userId);
            return new Employee
            {
                Email = employee.Email,
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
            };
        }

        public List<Employee> GetEmployees()
        {
            
            return _userManager.Users.Select(x => new Employee
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
            }).ToList();
        }
    }
}
