//using HR.LeaveManagement.Application.Contracts.Identity;
//using HR.LeaveManagement.Application.Models.Identity;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Models;
using HR.LeaveManagement.BlazorUI.Models.LeaveRequests;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;
using Microsoft.AspNetCore.Components;

namespace HR.LeaveManagement.BlazorUI.Pages.LeaveRequests
{
    public partial class Create
    {
        [Inject]
        IEmployeeService employeeService { get; set; }
        [Inject]
        ILeaveTypeService leaveTypeService { get; set; }
        [Inject]
        ILeaveRequestService leaveRequestService { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; }
        List<LeaveTypeVM> leaveTypeVMs { get; set; }
        List<EmployeeVM> employeeVMs { get; set; }
        LeaveRequestVM LeaveRequestVM { get; set; } = new LeaveRequestVM();

        protected override async Task OnInitializedAsync()
        {
            leaveTypeVMs = await leaveTypeService.GetLeaveTypes();
            employeeVMs = await employeeService.GetAll();

        }
        private async Task HandleValidSubmit()
        {
            //Perform form submission here
            var response = await leaveRequestService.CreateLeaveRequest(LeaveRequestVM);
            if (response.Success)
            {
                navigationManager.NavigateTo("/leaverequests/");
            }
        }
    }
}