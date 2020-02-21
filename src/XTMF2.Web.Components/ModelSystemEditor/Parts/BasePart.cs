//    Copyright 2017-2019 University of Toronto
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

using XTMF2.Web.Data.Models;
using Microsoft.AspNetCore.Components;
using System;

namespace XTMF2.Web.Components
{
    public interface BasePart
    {
        /// <summary>
        /// Event called whenever the component is moved or resized.
        /// </summary>
        public event EventHandler<EventArgs> OnBoundsChanged;

        [Microsoft.AspNetCore.Components.Parameter]
        public ModelSystemModel ModelSystem { get; set; }

    }
}