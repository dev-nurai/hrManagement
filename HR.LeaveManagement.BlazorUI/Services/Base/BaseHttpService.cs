using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace HR.LeaveManagement.BlazorUI.Services.Base
{
    public class BaseHttpService
    {
        protected IClient _client;
        //private IClient client;
        protected ILocalStorageService _localStorageService;
        public BaseHttpService(IClient client, ILocalStorageService localStorageService)
        {
            _client = client;
            _localStorageService = localStorageService;
        }
        protected Response<Guid> ConvertApiExceptions<Guid>(ApiException exception)
        {
            if (exception.StatusCode == 400)
            {
                return new Response<Guid>()
                {
                    Message = "Invalid data was submitted",
                    ValidationErrors = exception.Response,
                    Success = false
                };
            }
            else if (exception.StatusCode == 404)
            {
                return new Response<Guid>()
                {
                    Message = "The record was not found.",
                    Success = false,
                };
            }
            else
            {
                return new Response<Guid>()
                {
                    Message = "Something went wrong. please try again later.",
                    Success = false,
                };
            }
        }
        protected async Task AddBearerToken()
        {
            if (await _localStorageService.ContainKeyAsync("token"))
            {
                _client.HttpClient.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", await _localStorageService.GetItemAsync<string>("token"));
            }
        }
    }

}
