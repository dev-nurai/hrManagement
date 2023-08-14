using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Models;
using Microsoft.AspNetCore.Components;

namespace HR.LeaveManagement.BlazorUI.Pages.Employees
{
    public partial class Index
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        IEmployeeService employeeService { get; set; }
        List<EmployeeVM> employeeVMs { get; set; }

        protected async override Task OnInitializedAsync()
        {
            employeeVMs = await employeeService.GetAll();
        }

        protected void AddEmployee()
        {

        }
        protected void DetailsEmployee(string id)
        {
            NavigationManager.NavigateTo($"/employee/details/{id}");
        }
    }

}