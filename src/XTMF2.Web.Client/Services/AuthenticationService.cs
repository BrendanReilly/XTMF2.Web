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

using System;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using Microsoft.Extensions.Logging;
using XTMF2.Web.ApiClient;

namespace XTMF2.Web.Client.Services
{
    public class AuthenticationService
    {
        private readonly AuthenticationClient _client;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly ISessionStorageService _storage;

        /// <summary>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="storage"></param>
        /// <param name="logger"></param>
        /// <param name="authProvider"></param>
        public AuthenticationService(AuthenticationClient client, ISessionStorageService storage,
            ILogger<AuthenticationService> logger)
        {
            _client = client;
            _storage = storage;
            _logger = logger;
        }

        /// <summary>
        /// </summary>
        /// <value></value>
        public bool IsLoggedIn { get; private set; }

        /// <summary>
        /// </summary>
        public event AuthenticatedEventHandler Authenticated;

        /// <summary>
        ///     Tests the login state of the current stored user.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TestLoginAsync()
        {
            return await _client.TestLoginAsync(await _storage.GetItemAsync<string>("uerName"),
                await _storage.GetItemAsync<string>("token"));
        }

        /// <summary>
        ///     Performs a login action. If successful, the access token is stored in browser session data.
        /// </summary>
        /// <param name="userName">The username to login with.</param>
        /// <returns>True/false depending on login result.</returns>
        public async Task<bool> LoginAsync(string userName)
        {
            string result = default;
            try
            {
                result = await _client.LoginAsync("local");
                await _storage.SetItemAsync("token", result);
                await _storage.SetItemAsync("userName", userName);
            }
            catch (ApiException exception)
            {
                IsLoggedIn = false;
                _logger.LogError("Invalid login.", exception);
                return false;
            }

            IsLoggedIn = true;
            _logger.LogInformation("Logged in.");
            Authenticated?.Invoke(this, new AuthenticatedEventArgs(userName, result));
            return true;
        }

        /// <summary>
        ///     Performs a logout action.
        /// </summary>
        /// <returns></returns>
        public async void LogoutAsync()
        {
            await _client.LogoutAsync();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void AuthenticatedEventHandler( object sender, AuthenticatedEventArgs args);

    /// <summary>
    /// </summary>
    public class AuthenticatedEventArgs : EventArgs
    {
        /// <summary>
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public AuthenticatedEventArgs(string userName, string token)
        {
            (UserName, Token) = (userName, token);
        }

        public string UserName { get; }
        public string Token { get; }
    }
}