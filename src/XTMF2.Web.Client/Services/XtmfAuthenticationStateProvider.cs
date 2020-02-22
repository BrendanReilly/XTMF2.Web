//     Copyright 2017-2020 University of Toronto
// 
//     This file is part of XTMF2.
// 
//     XTMF2 is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     XTMF2 is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with XTMF2.  If not, see <http://www.gnu.org/licenses/>.

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace XTMF2.Web.Client.Services
{
    /// <summary>
    ///     Client authentication state provider.
    /// </summary>
    public class XtmfAuthenticationStateProvider : AuthenticationStateProvider
    {
        /// <summary>
        /// </summary>
        /// <param name="authenticationService"></param>
        public XtmfAuthenticationStateProvider(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            authenticationService.Authenticated += OnAuthenticated;
        }

        public static bool IsAuthenticated { get; set; }
        private AuthenticationService _authenticationService { get; }

        /// <summary>
        ///     Returns the authentication state of the current context. This method is invokved by Authorization tags
        ///     in several pages.
        /// </summary>
        /// <returns></returns>
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, _authenticationService.IsLoggedIn ? "local" : string.Empty)
            }, "JwtBearer");
            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }

        private void OnAuthenticated(object sender, AuthenticatedEventArgs eventArgs)
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}