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
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;

namespace XTMF2.Web.ApiClient
{
    /// <summary>
    ///     Base API client with share logic for manipulating request header variables before being sent to the server.
    /// </summary>
    public class BaseClient
    {
        protected ISessionStorageService SessionStorageService { get; set; }

        /// <summary>
        /// Required base method from client generation, this provides a custom mechanism to create the HttpRequest
        /// object and insert any values needed.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken token)
        {
            var req = new HttpRequestMessage();
            if (SessionStorageService != null)
            {
                var auth = await SessionStorageService.GetItemAsync<string>("token");
                req.Headers.Add("Authorization", $"Bearer {auth}");
            }
            return req;
        }
    }
}