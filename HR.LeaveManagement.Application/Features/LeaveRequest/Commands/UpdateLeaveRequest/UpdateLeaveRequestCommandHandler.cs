using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IAppLogger<UpdateLeaveRequestCommandHandler> _appLogger;

        public UpdateLeaveRequestCommandHandler(IMapper mapper, 
            IEmailSender emailSender, 
            ILeaveRequestRepository leaveRequestRepository, 
            ILeaveTypeRepository leaveTypeRepository,
            IAppLogger<UpdateLeaveRequestCommandHandler> appLogger)
        {
            _mapper = mapper;
            _emailSender = emailSender;
            _leaveRequestRepository = leaveRequestRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _appLogger = appLogger;
        }
        public async Task<Unit> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            //Fetch the record to update
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);
            if (leaveRequest == null)
            {
                throw new NotFoundException(nameof(LeaveRequest), request.Id);
            }
            //Validate
            var validator = new UpdateLeaveRequestCommandValidator(_leaveTypeRepository, _leaveRequestRepository);
            var validationResult = await validator.ValidateAsync(request);

            //Log the errors
            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid Leave Request", validationResult);
            }
            _mapper.Map(request, leaveRequest);
            await _leaveRequestRepository.UpdateAsync(leaveRequest);

            try
            {
                //Send confirmation email
                var email = new EmailMessage
                {
                    To = string.Empty, //Get email from employee record
                    Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} has been updated successfully.",
                    Subject = "Leave Request Updated"
                };

                await _emailSender.SendEmail(email);
            }
            catch (Exception ex)
            {
                _appLogger.LogWarning(ex.Message);
            }

            return Unit.Value;

        }
    }
}
