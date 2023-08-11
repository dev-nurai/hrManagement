using Blazored.LocalStorage;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Providers;
using HR.LeaveManagement.BlazorUI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace HR.LeaveManagement.BlazorUI.Services
{
    public class AuthenticationService : IAuthenticationService //BaseHttpService, 
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private ILocalStorageService _localStorageService1;
        private IClient _client;
        public AuthenticationService(IClient client, 
            ILocalStorageService localStorageService,
            AuthenticationStateProvider authenticationStateProvider) //: base(client, localStorageService)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _client = client;
            _localStorageService1 = localStorageService;
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
         {
            try
            {
                AuthRequest request = new AuthRequest()
                {
                    Email = email,
                    Password = password
                };
                
                var authenticationResponse = await _client.LoginAsync(request);
                if (authenticationResponse.Token != string.Empty)
                {
                    string gettoken25 = await _localStorageService1.GetItemAsync<string>("token25");
                    await _localStorageService1.SetItemAsync("token", authenticationResponse.Token);
                    string gettoken = await _localStorageService1.GetItemAsync<string>("token");
                    //Set claims in Blazor and login state
                    await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();
                    string gettoken1 = await _localStorageService1.GetItemAsync<string>("token");
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public async Task Logout()
        {
            //await _localService.RemoveItemAsync("token");
            //Remove claims in Blazor and login state
            await ((ApiAuthenticationStateProvider)_authenticationStateProvider).LoggedOut();
        }

        public async Task<bool> RegisterAsync(string firstName, string lastName, string userName, string email, string password)
        {
            RegistrationRequest request = new RegistrationRequest()
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                Password = password
            };
            
            var response = await _client.RegisterAsync(request);//Breaks here
            if (!string.IsNullOrEmpty(response.UserId))
            {
                return true;
            }
            return false;
        }
    }
}
