using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using HR.LeaveManagement.BlazorUI;
using HR.LeaveManagement.BlazorUI.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using HR.LeaveManagement.BlazorUI.Models;
using HR.LeaveManagement.BlazorUI.Contracts;

namespace HR.LeaveManagement.BlazorUI.Pages.Employees
{
    public partial class Details
    {
        [Inject]
        public IEmployeeService EmployeeService { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        [Parameter]
        public string id { get; set; }
        public EmployeeVM EmployeeModel { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            EmployeeModel = await EmployeeService.GetById(id);
            
        }
    }
}