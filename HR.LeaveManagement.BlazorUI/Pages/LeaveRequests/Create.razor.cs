using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Models.LeaveRequests;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;
using HR.LeaveManagement.BlazorUI.Services;
using HR.LeaveManagement.BlazorUI.Services.Base;
using Microsoft.AspNetCore.Components;

namespace HR.LeaveManagement.BlazorUI.Pages.LeaveRequests
{
    public partial class Create
    {
        [Inject]
        ILeaveTypeService leaveTypeService {  get; set; }
        [Inject]
        ILeaveRequestService leaveRequestService { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; }
        List<LeaveTypeVM> leaveTypeVMs { get; set; }
        LeaveRequestVM LeaveRequestVM { get; set; } = new LeaveRequestVM();

        protected override async Task OnInitializedAsync()
        {
            leaveTypeVMs = await leaveTypeService.GetLeaveTypes();
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