using AutoMapper;
using HR.LeaveManagement.BlazorUI.Models.LeaveAllocations;
using HR.LeaveManagement.BlazorUI.Models;
using HR.LeaveManagement.BlazorUI.Models.LeaveRequests;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;
using HR.LeaveManagement.BlazorUI.Services.Base;
//using HR.LeaveManagement.Application.Models.Identity;
//using HR.LeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocationDetails;

namespace HR.LeaveManagement.BlazorUI.MappingProfiles
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //Leave Type
            CreateMap<LeaveTypeDto, LeaveTypeVM>().ReverseMap();
            CreateMap<LeaveTypeDetailsDto, LeaveTypeVM>().ReverseMap();
            CreateMap<CreateLeaveTypeCommand, LeaveTypeVM>().ReverseMap();
            CreateMap<UpdateLeaveTypeCommand, LeaveTypeVM>().ReverseMap();

            //Leave Request
            CreateMap<LeaveRequestListDto, LeaveRequestVM>()
                .ForMember(q => q.DateRequested, opt => opt.MapFrom(x => x.DateRequested.DateTime)) //Solved map issue on datetimeoffset [swag api] to datetime
                .ForMember(q => q.StartDate, opt => opt.MapFrom(x => x.StartDate.DateTime))
                .ForMember(q => q.EndDate, opt => opt.MapFrom(x => x.EndDate.DateTime))
                .ReverseMap();
            CreateMap<LeaveRequestDetailsDto, LeaveRequestVM>()
                .ForMember(q => q.DateRequested, opt => opt.MapFrom(x => x.DateRequested.DateTime))
                .ForMember(q => q.StartDate, opt => opt.MapFrom(x => x.StartDate.DateTime))
                .ForMember(q => q.EndDate, opt => opt.MapFrom(x => x.EndDate.DateTime))
                .ReverseMap();
            CreateMap<CreateLeaveRequestCommand, LeaveRequestVM>().ReverseMap();
            CreateMap<UpdateLeaveRequestCommand, LeaveRequestVM>().ReverseMap();

            //Leave Allocation
            CreateMap<LeaveAllocationDto, LeaveAllocationVM>().ReverseMap();
            //CreateMap<LeaveAllocationDetailsDto, LeaveAllocationVM>().ReverseMap();
            CreateMap<CreateLeaveAllocationCommand, LeaveAllocationVM>().ReverseMap();
            CreateMap<UpdateLeaveAllocationCommand, LeaveAllocationVM>().ReverseMap();

            //Employee
            CreateMap<EmployeeVM, Employee>().ReverseMap();
            //CreateMap<EmployeeVM, Application.Models.Identity.Employee>().ReverseMap();

        }
    }
}
