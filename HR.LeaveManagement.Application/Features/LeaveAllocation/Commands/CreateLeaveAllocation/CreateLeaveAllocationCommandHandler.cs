using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistance;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IUserService _userService;

        public CreateLeaveAllocationCommandHandler(IMapper mapper, 
            ILeaveTypeRepository leaveTypeRepository, 
            ILeaveAllocationRepository leaveAllocationRepository,
            IUserService userService)
        {
            _mapper = mapper;
            _leaveAllocationRepository = leaveAllocationRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _userService = userService;
        }
        public async Task<Unit> Handle(CreateLeaveAllocationCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveAllocationCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);
            if(validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid leave Allocation Request", validationResult);
            }

            //Get Leave Type for Allocations
            var leaveType = await _leaveTypeRepository.GetByIdAsync(request.LeaveTypeId);

            //Get Employees
            var employees =  _userService.GetEmployees();

            //Get Period
            var period = DateTime.Now.Year;

            //Assign Allocations if an allocation doesn't already exist for period and leave type
            var allocations = new List<Domain.LeaveAllocation>();
            foreach(var employee in employees)
            {
                var allocationExists = await _leaveAllocationRepository.AllocationExists(employee.Id, request.LeaveTypeId, period);
                if(allocationExists == false)
                {
                    allocations.Add(new Domain.LeaveAllocation
                    {
                        EmployeeId = employee.Id,
                        LeaveTypeId = leaveType.Id,
                        NumberOfDays = leaveType.DefaultDays,
                        Period = period, 
                    });
                }
            }

            if (allocations.Any())
            {
                await _leaveAllocationRepository.AddAllocations(allocations);
            }

            //var leaveAllocation = _mapper.Map<Domain.LeaveAllocation>(request);
            //await _leaveAllocationRepository.CreateAsync(leaveAllocation);

            return Unit.Value;
        }
    }
}
