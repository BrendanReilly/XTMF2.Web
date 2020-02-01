﻿//     Copyright 2017-2020 University of Toronto
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

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Data.Models;
using XTMF2.Web.Server.Session;

namespace XTMF2.Web.Server.Controllers {
    /// <summary>
    ///     API Controller for project related actions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] ! Authorization pending change to client
    public class ProjectController : ControllerBase {
        private readonly ILogger<ProjectController> _logger;
        private readonly IMapper _mapper;
        private readonly XTMFRuntime _xtmfRuntime;

        /// <summary>
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public ProjectController(XTMFRuntime runtime,
            ILogger<ProjectController> logger,
            IMapper mapper) {
            _xtmfRuntime = runtime;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        ///     Creates a new project from the model passed. A model representing the newly created
        ///     project is returned to the caller.
        /// </summary>
        /// <param name="projectName"></param>
        /// <paramref name="user"/>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ProjectModel), StatusCodes.Status201Created)]
        public IActionResult Create(string projectName, [FromServices] UserSession state) {
            var error = default(string);
            if (!_xtmfRuntime.ProjectController.CreateNewProject(state.User, projectName,
                    out var session, ref error)) {
                _logger.LogError($"Unable to create project: {projectName}\n" +
                    $"Error: {error}");
                return new UnprocessableEntityObjectResult(error);
            }
            _logger.LogInformation($"New project created: {session.Project.Name}");
            return new CreatedResult(nameof(ProjectController), _mapper.Map<ProjectModel>(session.Project));
        }

        /// <summary>
        ///     Gets the specified project.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param  name="user"></param>
        /// <returns></returns>
        [HttpGet("{projectName}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProjectModel), StatusCodes.Status200OK)]
        public IActionResult Get(string projectName, [FromServices] UserSession state) {
            string error = default;
            var project = state.Projects.FirstOrDefault(p => p.Name == projectName);
            if (project != null) {
                return new OkObjectResult(_mapper.Map<ProjectModel>(project));
            }
            if (!_xtmfRuntime.ProjectController.GetProject(state.User.UserName, projectName,
                    out project, ref error)) {
                return new NotFoundResult();
            }
            return new OkObjectResult(_mapper.Map<ProjectModel>(project));
        }

        /// <summary>
        ///     Lists all projects by the current user.
        /// </summary>
        /// <param  name="user"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<ProjectModel>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ProjectModel>> List([FromServices] UserSession state) {
            return new OkObjectResult(_mapper.Map<List<ProjectModel>>(state.Projects));
        }

        /// <summary>
        ///     Deletes a project.
        /// </summary>
        /// <param name="projectName">The name of the project to delete.</param>
        /// <param  name="user"></param>
        /// <returns></returns>
        [HttpDelete("{projectName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult Delete(string projectName, [FromServices] UserSession state) {
            var error = default(string);
            var project = state.Projects.FirstOrDefault(p => p.Name == projectName);
            if (project == null) {
                return new NotFoundObjectResult(error);
            }
            if (!_xtmfRuntime.ProjectController.DeleteProject(state.User, project, ref error)) {
                _logger.LogError($"Unable to delete project: {projectName}\n" +
                    $"Error: {error}");
                return new UnprocessableEntityObjectResult(error);
            }
            _logger.LogInformation($"Project deleted: {projectName}");
            return new OkResult();
        }
    }
}