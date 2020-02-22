﻿//    Copyright 2017-2019 University of Toronto
// 
//    This file is part of XTMF2.
// 
//    XTMF2 is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
// 
//    XTMF2 is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
// 
//    You should have received a copy of the GNU General Public License
//    along with XTMF2.  If not, see <http://www.gnu.org/licenses/>.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Client.Pages
{
    /// <summary>
    ///     Root page / component for the model system display.
    /// </summary>
    public partial class ModelSystem : ComponentBase
    {
        /// <summary>
        ///     Path parameter that specifies the project name.
        /// </summary>
        [Parameter]
        public string ProjectName { get; set; }

        /// <summary>
        ///     Path parameter that specifies the model system name.
        /// </summary>
        [Parameter]
        public string ModelSystemName { get; set; }


        [Inject]
        protected ILogger<ModelSystem> Logger { get; set; }

        protected ModelSystemModel Model { get; private set; }

        protected override void OnInitialized()
        {
            Model = new ModelSystemModel()
            {
                Name = ModelSystemName
            };
        }

    }
}
