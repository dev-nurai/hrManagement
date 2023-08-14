using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Models.Dtos;
using HR.LeaveManagement.Application.Models.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUserService _userService;

        public EmployeeController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public List<Employee> Get()
        {
            return _userService.GetEmployees();
        }

        [HttpGet("{id}")]
        public async Task<Employee> GetById(string id)
        {
            return await _userService.GetEmployee(id);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeDto employeeDto)
        {
            var addEmployee = _userService.AddEmployee(employeeDto);
            return Ok();
        }
    }
}
