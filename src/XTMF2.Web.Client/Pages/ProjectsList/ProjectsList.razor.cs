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

namespace XTMF2.Web.Client.Pages
{
    /// <summary>
    ///     Projects (page) base component. This page lists the currently existing projects
    ///     for the current active user. The user can both add and delete projects from this page.
    /// </summary>
    public partial class ProjectsList : ComponentBase
    {
        /// <summary>
        ///     New project form validation model.
        /// </summary>
        private InputRequestDialog _inputRequestDialog = null;

        /// <summary>
        ///     Modal ref for the new project dialog.
        /// </summary>
        public BSModal NewProjectModal;

        protected bool IsLoaded { get; set; }

        [Inject] protected ProjectClient ProjectClient { get; set; }

        protected ILogger Logger { get; } = Log.ForContext<ProjectsList>();

        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] protected NotificationService NotificationService { get; set; }

        /// <summary>
        ///     List of projects for the active user.
        /// </summary>
        public List<ProjectModel> Projects { get; } = new List<ProjectModel>();

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        public void OpenNewProjectDialog(EventArgs e)
        {
            _inputRequestDialog.Show();
        }

        /// <summary>
        ///     Initialization for component.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            Logger.Information("Projects List loading.");
            var projects = await ProjectClient.ListAsync();
            Projects.AddRange(projects);
            IsLoaded = true;
        }

        /// <summary>
        ///     Deletes a project for this user.
        /// </summary>
        public async void DeleteProject(ProjectModel project)
        {
            await ProjectClient.DeleteAsync(project.Name);
            Projects.Remove(project);
            Logger.Information($"Project {project.Name} has been deleted.");
            NotificationService.ErrorMessage($"Project {project.Name} deleted.");
            this.StateHasChanged();
        }

        /// <summary>
        ///     Attempts to create a new project on submission of the new project form.
        /// </summary>
        protected async void OnNewProjectFormSubmit(string input)
        {
            var model = await ProjectClient.CreateAsync(input);
            Projects.Add(model);
            Logger.Information($"Project {input} has been created.");
            NotificationService.SuccessMessage($"Project {input} Created.");
            this.StateHasChanged();

        }

        /// <summary>
        ///     Closes the new project dialog/
        /// </summary>
        /// <param name="e"></param>
        protected void CloseNewProjectDialog()
        {
            NewProjectModal.Hide();
        }
    }
}