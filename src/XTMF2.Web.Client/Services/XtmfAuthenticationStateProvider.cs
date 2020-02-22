using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace XTMF2.Web.Client.Services
{

    /// <summary>
    /// Client authentication state provider.
    /// </summary>
    public class XtmfAuthenticationStateProvider : AuthenticationStateProvider
    {
        public static bool IsAuthenticated { get; set; }
        private AuthenticationService _authenticationService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticationService"></param>
        public XtmfAuthenticationStateProvider(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            authenticationService.Authenticated += OnAuthenticated;
        }

        /// <summary>
        /// Returns the authentication state of the current context. This method is invokved by Authorization tags 
        /// in several pages.
        /// </summary>
        /// <returns></returns>
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity(new[] {
                new Claim (ClaimTypes.Name, _authenticationService.IsLoggedIn ? "local" : string.Empty),
                }, "JwtBearer")
            {
            };
            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }

        private void OnAuthenticated(object sender, AuthenticatedEventArgs eventArgs)
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}