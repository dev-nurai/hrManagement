using HR.LeaveManagement.BlazorUI.Models;

namespace HR.LeaveManagement.BlazorUI.Contracts
{
    public interface IEmployeeService
    {
        Task<List<EmployeeVM>> GetAll();
        Task<EmployeeVM> GetById(string id);
       //Task CreateEmployee(EmployeeVM employeeVM);
    }
}
