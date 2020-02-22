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
using XTMF2.Web.Client.Services.Api;
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
    public partial class BasePage : ComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment NavContent { get; set; }

        [Parameter]
        public RenderFragment Title { get; set; }

        [Parameter]
        public bool ShowSideNav { get; set; }
    }
}