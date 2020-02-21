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
        private ISessionStorageService Storage { get; set; }
        private bool _isLoggedIn;
        public XtmfAuthenticationStateProvider(ISessionStorageService storage)
        {
        }

        /// <summary>
        /// Returns the authentication state of the current context. This method is invokved by Authorization tags 
        /// in several pages.
        /// </summary>
        /// <returns></returns>
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity(new[] {
                new Claim (ClaimTypes.Name, string.Empty),
                }, _isLoggedIn ? "local" : string.Empty)
            {

            };
            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggedIn"></param>
        public void NotifyUpdate(bool loggedIn = false)
        {   _isLoggedIn = loggedIn;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}