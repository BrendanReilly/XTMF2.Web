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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using XTMF2.Web.Client.Services.Api;
using XTMF2.Web.Server.Session;

namespace XTMF2.Web.Server.Mapping.Binders
{
    public class ModelSystemObjectBinder : IModelBinder
    {

        private readonly ModelSystemSessions _modelSystemSessions;

        private readonly ProjectSessions _projectSessions;

        private readonly UserSession _userSession;

        private readonly XTMFRuntime _runtime;

        public ModelSystemObjectBinder(ModelSystemSessions modelSystemSessions, UserSession userSession, XTMFRuntime runtime, ProjectSessions projectSessions)
        {
            _modelSystemSessions = modelSystemSessions;
            _userSession = userSession;
            _runtime = runtime;
            _projectSessions = projectSessions;
        }
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var routeValues = bindingContext.HttpContext.Request.RouteValues;
            if (routeValues.ContainsKey("projectName") || routeValues.ContainsKey("modelSystemName"))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Project or model system are not defined.");
                return Task.CompletedTask;
            }
            var project = Utils.XtmfUtils.GetProject((string)routeValues["projectName"], _userSession);
            if (!Utils.XtmfUtils.GetModelSystemHeader(_runtime, _userSession, _projectSessions, (string)routeValues["projectName"],
             (string)routeValues["modelSystemName"], out var modelSystemHeader, out var commandError))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid model system or project specified.");
            }
            var session = _modelSystemSessions.GetModelSystemSession(_userSession.User, project, modelSystemHeader);
            var tracker = _modelSystemSessions.GetModelSystemEditingTracker(session);
            if (tracker.TryGetModelSystemObject<ViewObject>(Guid.Parse(bindingContext.HttpContext.Request.Query[bindingContext.ModelName]), out var modelSystemObject))
            {
                bindingContext.Result = ModelBindingResult.Success(modelSystemObject);
            }
            return Task.CompletedTask;
        }
    }
}