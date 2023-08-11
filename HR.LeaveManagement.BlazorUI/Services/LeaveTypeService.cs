using AutoMapper;
using Blazored.LocalStorage;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;
using HR.LeaveManagement.BlazorUI.Services.Base;

namespace HR.LeaveManagement.BlazorUI.Services
{
    public class LeaveTypeService : BaseHttpService, ILeaveTypeService
    {
        private readonly IMapper _mapper;
        public LeaveTypeService(IClient client, IMapper mapper, ILocalStorageService localStorageService) : base(client, localStorageService)
        {
            _mapper = mapper;
        }

        public async Task<Response<Guid>> CreateLeaveType(LeaveTypeVM leaveTypeVM)
        {
            try
            {
                await AddBearerToken();
                var createLeaveTypeCommand = _mapper.Map<CreateLeaveTypeCommand>(leaveTypeVM);
                await _client.LeaveTypesPOSTAsync(createLeaveTypeCommand);
                return new Response<Guid>()
                {
                    Success = true,
                };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<Guid>(ex);
            }
            
        }

        public async Task<Response<Guid>> DeleteLeaveType(int id)
        {
            try
            {
                await AddBearerToken();
                await _client.LeaveTypesDELETEAsync(id);
                return new Response<Guid>()
                {
                    Success = true,
                };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<Guid>(ex);
            }
        }

        public async Task<LeaveTypeVM> GetLeaveTypeDetails(int id)
        {
            await AddBearerToken();
            var leaveType = await _client.LeaveTypesGETAsync(id);
            return  _mapper.Map<LeaveTypeVM>(leaveType);
        }

        public async Task<List<LeaveTypeVM>> GetLeaveTypes()
        {
            //await AddBearerToken();
            var leaveTypes = await _client.LeaveTypesAllAsync(); // Breaks here
            return _mapper.Map<List<LeaveTypeVM>>(leaveTypes);
        }

        public async Task<Response<Guid>> UpdateLeaveType(int id, LeaveTypeVM leaveTypeVM)
        {
            try
            {
                //await AddBearerToken();
                var updateLeaveTypeCommand = _mapper.Map<UpdateLeaveTypeCommand>(leaveTypeVM);
                await _client.LeaveTypesPUTAsync(id.ToString(), updateLeaveTypeCommand);//Breaks here
                return new Response<Guid>()
                {
                    Success = true,
                };
            }
            catch(ApiException ex)
            {
                return ConvertApiExceptions<Guid>(ex);
            }
        }
    }

}
