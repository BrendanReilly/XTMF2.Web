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
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.ApiClient;
using XTMF2.Web.Shared.Util;

namespace XTMF2.Web.Components.ModelSystemEditor
{
    public partial class ModelSystemEditor : ComponentBase
    {
        protected Dictionary<Guid, ViewObject> ViewObjectMap { get; set; } = new Dictionary<Guid, ViewObject>();

        protected Dictionary<Guid, BasePart> ComponentMap { get; set; } = new Dictionary<Guid, BasePart>();

        [Parameter]
        public ModelSystemModel ModelSystemInfo { get; set; }

        [Parameter]
        public ModelSystemEditingModel Model { get; set; }

        [Inject]
        protected ModelSystemEditorClient EditorClient { get; set; }

        [Inject]
        protected ILogger<ModelSystemEditor> Logger { get; set; }

        protected override void OnParametersSet()
        {
            Logger.LogInformation(ModelSystemInfo.Name);
            UpdateComponentMap();
        }

        private void UpdateComponentMap()
        {
            Console.WriteLine(Model);
            // store all view object (model) references in map
            foreach (var viewObject in EditingUtil.ModelSystemObjects(Model))
            {
                ViewObjectMap[viewObject.Id] = viewObject;
                Console.WriteLine(viewObject);
            }
        }

        /// <summary>
        /// Called when any of the child model system objects' bounds changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBoundsChanged(object sender, BoundsChangedEventArgs e)
        {

        }

        /// <summary>
        /// Register the component with the editor, maps the view object id to the actual rendered component
        /// </summary>
        /// <param name="component"></param>
        public void RegisterComponent(BasePart component)
        {
            ComponentMap[component.Model.Id] = component;
            Logger.LogInformation($"registered component {component}, {component.Model.Id}");
            component.BoundsChanged += OnBoundsChanged;
        }

        /// <summary>
        /// Un-registers the component from the component map
        /// </summary>
        /// <param name="component"></param>
        public void UnRegisterComponent(BasePart component)
        {
            component.BoundsChanged -= OnBoundsChanged;
            ComponentMap.Remove(component.Model.Id);
        }
    }
}