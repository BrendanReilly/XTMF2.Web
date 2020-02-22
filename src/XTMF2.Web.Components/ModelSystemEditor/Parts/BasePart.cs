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
using XTMF2.Web.Data.Types;

namespace XTMF2.Web.Components
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BasePart : ComponentBase
    {
        [Parameter]
        public ModelSystemModel ModelSystem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public event BoundsChangedEventHandler BoundsChanged;

    }

    /// <summary>
    /// 
    /// </summary>
    public class BoundsChangedEventArgs : EventArgs
    {
        public Rectangle OldBounds { get; }
        public Rectangle NewBounds { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldBounds"></param>
        /// <param name="newBounds"></param>
        /// <returns></returns>
        public BoundsChangedEventArgs(Rectangle oldBounds, Rectangle newBounds) => (OldBounds,NewBounds) = (oldBounds,NewBounds);
    }

    public delegate void BoundsChangedEventHandler(object sender, BoundsChangedEventArgs e);
}