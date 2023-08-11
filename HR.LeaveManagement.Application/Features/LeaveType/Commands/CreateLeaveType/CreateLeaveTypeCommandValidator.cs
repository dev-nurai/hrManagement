using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType
{
    public class CreateLeaveTypeCommandValidator : AbstractValidator<CreateLeaveTypeCommand>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public CreateLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
        {
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

        private Task<bool> LeaveTypeNameUnique(CreateLeaveTypeCommand command, CancellationToken token)
        {
            return _leaveTypeRepository.IsLeaveTypeUnique(command.Name, 0);
        }
    }
}
