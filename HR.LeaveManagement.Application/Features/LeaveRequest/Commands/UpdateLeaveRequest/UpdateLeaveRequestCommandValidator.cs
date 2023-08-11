using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistance;
using HR.LeaveManagement.Application.Features.LeaveRequest.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest
{
    public class UpdateLeaveRequestCommandValidator : AbstractValidator<UpdateLeaveRequestCommand>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        public UpdateLeaveRequestCommandValidator(ILeaveTypeRepository leaveTypeRepository, ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _leaveTypeRepository = leaveTypeRepository;

            //This will include the BaseLeaveRequest Validation
            Include(new BaseLeaveRequestValidator(_leaveTypeRepository));

            RuleFor(x => x.Id)
                .NotNull()
                .MustAsync(LeaveRequestMustExist)
                .WithMessage("{PropertyName} must be present");

        }

        private async Task<bool> LeaveRequestMustExist(int id, CancellationToken token)
        {
            var leaveAllocation = await _leaveRequestRepository.GetByIdAsync(id);
            return leaveAllocation != null;
        }
    }
}
