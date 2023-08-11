﻿using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Identity;
using HR.LeaveManagement.Application.Contracts.Persistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IUserService _userService;
        public CreateLeaveRequestCommandHandler(IMapper mapper, 
            IEmailSender emailSender, 
            ILeaveRequestRepository leaveRequestRepository, 
            ILeaveTypeRepository leaveTypeRepository,
            IUserService userService,
            ILeaveAllocationRepository leaveAllocationRepository)
        {
            _mapper = mapper;
            _emailSender = emailSender;
            _leaveRequestRepository = leaveRequestRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _userService = userService;
            _leaveAllocationRepository = leaveAllocationRepository;
        }
        public async Task<int> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            //Validate the request
            var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if(validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid Leave Request", validationResult);
            }

            //Get requesting employee's id
            var employeeId = _userService.UserId;

            ////Check on employee's allocation
            var allocation = await _leaveAllocationRepository.GetUserAllocations(employeeId, request.LeaveTypeId);

            //if allocations aren't enough, return validation error with message
            if (allocation is null)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(nameof(request.LeaveTypeId),
                    "You do not have any allocations for this leave type."));
                throw new BadRequestException("Invalid Leave Request", validationResult);
            }

            int daysRequested = (int)(request.EndDate - request.StartDate).TotalDays;
            if (daysRequested > allocation.NumberOfDays)
            {
                validationResult.Errors.Add(new FluentValidation.Results.ValidationFailure(
                    nameof(request.EndDate), "You do not have enough days for this request"));
                throw new BadRequestException("Invalid Leave Request", validationResult);
            }

            //Create leave Request
            var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request);
            leaveRequest.RequestingEmployeeId = employeeId;
            leaveRequest.DateRequested = DateTime.Now;
            await _leaveRequestRepository.CreateAsync(leaveRequest);

            //Send confirmation email
            try
            {
                var email = new EmailMessage
                {
                    To = string.Empty, //Get email from employee record
                    Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} has been updated successfully.",
                    Subject = "Leave Request Submitted"
                };

                await _emailSender.SendEmail(email);
            }
            catch (Exception)
            {

            }
            

            return leaveRequest.Id;
        }
    }
}
