using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HR.LeaveManagement.BlazorUI.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly JwtSecurityTokenHandler _jwtSercurityTokenHandler;

        public ApiAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
            _jwtSercurityTokenHandler = new JwtSecurityTokenHandler();
        }

        //Get State Authentication
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            //Checking the token in localStorage
            var isTokenPresent = await _localStorageService.ContainKeyAsync("token");
            
            if (isTokenPresent == false)
            {
                return new AuthenticationState(user);
            }

            var savedToken = await _localStorageService.GetItemAsync<string>("token");
            var tokenContent = _jwtSercurityTokenHandler.ReadJwtToken(savedToken);

            //Checking token expiring and removing the token
            if (tokenContent.ValidTo < DateTime.UtcNow)
            {
                await _localStorageService.RemoveItemAsync("token");
                return new AuthenticationState(user);
            }

            var claims = await GetClaims();

            user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            return new AuthenticationState(user);
        }

        //LoggedIn
        public async Task LoggedIn()
        {
            //Fetch the claim
            var claims = await GetClaims();
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            //Set authentication state as user
            var authState = Task.FromResult(new AuthenticationState(user));
            //Notify State has changed => logged in
            NotifyAuthenticationStateChanged(authState);
        }

        //LoggedOut
        public async Task LoggedOut()
        {
            //Remove the item from the localStorage
            await _localStorageService.RemoveItemAsync("token");
            //Fresh Claims - nobody
            var nobody = new ClaimsPrincipal(new ClaimsIdentity());
            //Set authentication state as nobody
            var authState = Task.FromResult(new AuthenticationState(nobody));
            //Notify State has changed => logged out
            NotifyAuthenticationStateChanged(authState);

        }

        //Get Claims
        private async Task<List<Claim>> GetClaims()
        {
            //Fetch the token from the localStorage
            var savedToken = await _localStorageService.GetItemAsync<string>("token");
            //Parsing JWT
            var tokenContent = _jwtSercurityTokenHandler.ReadJwtToken(savedToken);
            //Get claims from the token
            var claims = tokenContent.Claims.ToList();
            claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));// Breaks here
            return claims;
        }
    }
}
