using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeCommandValidator : AbstractValidator<UpdateLeaveTypeCommand>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public UpdateLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
        {
            RuleFor(x => x.Id)
                .NotNull()
                .MustAsync(LeaveTypeMustExist);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull()
                .MaximumLength(70).WithMessage("{PropertyName} must be fewer than  70 characters");

            RuleFor(x => x.DefaultDays)
                .GreaterThan(1).WithMessage("{PropertyName} cannot be less than 1")
                .LessThan(100).WithMessage("{PropertyName} cannot exceed 100");

            RuleFor(x => x)
                .MustAsync(LeaveTypeNameUnique)
            .WithMessage("Leave type already exists");

            this._leaveTypeRepository = leaveTypeRepository;

        }

        private async Task<bool> LeaveTypeMustExist(int id, CancellationToken token)
        {
            var leaveType = await _leaveTypeRepository.GetByIdAsync(id);
            return leaveType != null;
        }

        private Task<bool> LeaveTypeNameUnique(UpdateLeaveTypeCommand command, CancellationToken token)
        {
            return _leaveTypeRepository.IsLeaveTypeUnique(command.Name, command.Id);
        }
    }
}
