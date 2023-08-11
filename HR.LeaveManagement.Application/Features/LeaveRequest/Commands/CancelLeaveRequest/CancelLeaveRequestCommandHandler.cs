using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Persistance;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Models.Email;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest
{
    public class CancelLeaveRequestCommandHandler : IRequestHandler<CancelLeaveRequestCommand, Unit>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmailSender _emailSender;

        public CancelLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _leaveRequestRepository = leaveRequestRepository;
        }
        public async Task<Unit> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            //Fetch the leaveRequest
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

            if (leaveRequest == null)
            {
                throw new NotFoundException(nameof(leaveRequest), request.Id);
            }

            leaveRequest.Cancelled = true;

            //Re-evaluate the employee's allocations for the leave type

            //Send confirmation Email
            var email = new EmailMessage
            {
                To = string.Empty, //Get email from employee record
                Body = $"Your leave request for {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D} has been cancelled successfully.",
                Subject = "Leave Request Cancelled"
            };

            await _emailSender.SendEmail(email);

            return Unit.Value;
        }
    }
}
