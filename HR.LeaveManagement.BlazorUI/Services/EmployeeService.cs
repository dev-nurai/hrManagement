using AutoMapper;
using Blazored.LocalStorage;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Models;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;
using HR.LeaveManagement.BlazorUI.Services.Base;

namespace HR.LeaveManagement.BlazorUI.Services
{
    public class EmployeeService : BaseHttpService, IEmployeeService
    {
        private readonly IMapper _mapper;
        public EmployeeService(IClient client, ILocalStorageService localStorageService, IMapper mapper) : base(client, localStorageService)
        {
            _mapper = mapper;
        }

        public async Task<List<EmployeeVM>> GetAll()
        {
            var employees = await _client.EmployeeAllAsync();
            return _mapper.Map<List<EmployeeVM>>(employees);
        }

        public async Task<EmployeeVM> GetById(string id)
        {
            
            var employee = await _client.EmployeeAsync(id);
            return _mapper.Map<EmployeeVM>(employee);
        }

        //Add Employee
    }
}
