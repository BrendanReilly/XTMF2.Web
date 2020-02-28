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
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using XTMF2.Web.Data.Models.Editing;

namespace XTMF2.Web.Components.ModelSystemEditor
{
    public partial class ModelSystemObject<T> : ComponentBase, IDisposable where T : ViewObject
    {
        private ElementReference _containerDiv;
        private DotNetObjectReference<ModelSystemObject<T>> _objRef;

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public ViewObject Model { get; set; }

        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Parameter] public BasePart ViewObjectComponent { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                // create dotnet object reference to call from js
                _objRef = DotNetObjectReference.Create<ModelSystemObject<T>>(this);
                await JSRuntime.InvokeVoidAsync("modelSystemEditor.enableInteract", _containerDiv, _objRef);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        [JSInvokable]
        public void UpdateBounds(float x, float y, float width, float height)
        {
            ViewObjectComponent?.UpdateBounds(x,y,width,height);
        }


        public void Dispose()
        {
            _objRef?.Dispose();
        }
    }
}