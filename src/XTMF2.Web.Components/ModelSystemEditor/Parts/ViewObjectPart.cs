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
using Microsoft.AspNetCore.Components;
using XTMF2.Web.Data.Models;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Data.Types;

namespace XTMF2.Web.Components.ModelSystemEditor
{
    /// <summary>
    /// </summary>
    public class ViewObjectPart<T> : BasePart where T : ViewObject
    {
        [Parameter]
        public new T Model
        {
            get => (T) _model;
            set => _model = value;
        }

        [Parameter] public EventCallback<BoundsChangedEventArgs> OnBoundsChanged { get; set; }

        protected override void OnInitialized()
        {
            Console.WriteLine("Editor: " + Editor);
            Editor.RegisterComponent(this);
        }

        private async void ObjectBoundsChanged()
        {
            await OnBoundsChanged.InvokeAsync(new BoundsChangedEventArgs(Model.Location, Model.Location));
        }
    }

    /// <summary>
    /// </summary>
    public abstract class BasePart : ComponentBase
    {
        protected ViewObject _model;
        [Parameter] public ModelSystemModel ModelSystemInfo { get; set; }

        public ViewObject Model
        {
            get => _model;
            set => _model = value;
        }

        [CascadingParameter] public ModelSystemEditor Editor { get; set; }
    }

    /// <summary>
    /// </summary>
    public class BoundsChangedEventArgs : EventArgs
    {
        /// <summary>
        /// </summary>
        /// <param name="oldBounds"></param>
        /// <param name="newBounds"></param>
        /// <returns></returns>
        public BoundsChangedEventArgs(Rectangle oldBounds, Rectangle newBounds)
        {
            (OldBounds, NewBounds) = (oldBounds, NewBounds);
        }

        public Rectangle OldBounds { get; }
        public Rectangle NewBounds { get; }
    }
}