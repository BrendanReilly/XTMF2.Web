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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BlazorStrap;
using Microsoft.AspNetCore.Components;
using Serilog;
using Microsoft.JSInterop;
using XTMF2.Web.ApiClient;
using XTMF2.Web.Components.Util;
using XTMF2.Web.Data.Models;
using XTMF2.Web.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace XTMF2.Web.Client.Shared
{
    /// <summary>
    ///     Projects (page) base component. This page lists the currently existing projects
    ///     for the current active user. The user can both add and delete projects from this page.
    /// </summary>
    public partial class MainLayout : LayoutComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        private bool IsLoggedIn { get; set; }

        [Inject] protected XtmfAuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject] protected IJSRuntime JSRuntime { get; set; }

        [Inject] protected AuthenticationService AuthenticationService { get; set; }

        [Inject] protected ILogger<MainLayout> Logger { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
            var result = await AuthenticationService.LoginAsync("local");
            await AuthenticationStateTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authStateTask"></param>
        /// <returns></returns>
        private async void OnAuthenticationStateChanged(Task<AuthenticationState> authStateTask)
        {
            var result = await authStateTask;
            await InvokeAsync(() =>
           {
               IsLoggedIn = result.User.Identity.IsAuthenticated;
               if (IsLoggedIn)
               {
                   // hide the overlay on successful login
                   JSRuntime.InvokeVoidAsync("xtmf2.hideOverlay");
                   Logger.LogInformation("Authenticated successfully.");
               }
               else
               {
                   Logger.LogInformation("Unable to authenticate.");
               }
               StateHasChanged();
           });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // invoke after render javascript functions
                await JSRuntime.InvokeVoidAsync("afterRender");
            }
        }
    }
}