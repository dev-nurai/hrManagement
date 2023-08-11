using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistance;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType
{
    public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        public CreateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            //Validate incoming data
            var validator = new CreateLeaveTypeCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            //if (!validationResult.IsValid)
            if (validationResult.Errors.Any()) //For any given error messages
            {
                throw new BadRequestException("Invalid Leave type", validationResult);
            }

            //Convert to domain entity object
            var leaveTypeToCreate = _mapper.Map<Domain.LeaveType>(request);
            // add to database
            await _leaveTypeRepository.CreateAsync(leaveTypeToCreate);

            //return record id 
            return leaveTypeToCreate.Id;
        }
    }
}
