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

using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XTMF2.Web.Data.Interfaces;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Server.Controllers {
    /// <summary>
    ///     API controller for the editing of model systems.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ModelSystemController : ControllerBase {

        private readonly ILogger<ModelSystemController> _logger;
        private readonly IMapper _mapper;
        private readonly XTMFRuntime _xtmfRuntime;

        /// <summary>
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        public ModelSystemController(XTMFRuntime runtime, User user, ILogger<ModelSystemController> logger,
            IMapper mapper) {
            _xtmfRuntime = runtime;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        ///     Creates a new model system from the passed model.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemModel"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("project/{projectName}")]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ModelSystemModel), StatusCodes.Status201Created)]
        public ActionResult Create(string projectName, [FromBody] ModelSystemModel modelSystemModel, [FromServices] User user) {
            if (!ModelState.IsValid) {
                return new UnprocessableEntityObjectResult(ModelState.Values.ToArray());
            }

            var error = default(string);
            if (!_xtmfRuntime.ProjectController.GetProject(user.UserName, projectName, out var project, ref error)) {
                return new NotFoundObjectResult(error);
            }

            if (!_xtmfRuntime.ProjectController.GetProjectSession(user, project, out var projectSession, ref error)) {
                return new NotFoundObjectResult(error);
            }

            projectSession.CreateNewModelSystem(user, modelSystemModel.Name, out var modelSystem, ref error);
            return new CreatedResult(nameof(ModelSystemController), _mapper.Map<ModelSystemModel>(modelSystem));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpDelete("projects/{projectName}/model-systems/{modelSystemName}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Delete(string projectName, string modelSystemName, [FromServices] User user) {
            return new OkResult();
        }

        /// <summary>
        /// </summary>
        /// <param name="project"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("projects/{projectName}/model-systems/{modelSystemName}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ModelSystemModel), StatusCodes.Status200OK)]
        public ActionResult<ModelSystemModel> Get(string projectName, string modelSystemName, [FromServices] User user) {
            string error = default;
            if (!_xtmfRuntime.ProjectController.GetProject(user.UserName, projectName, out var project, ref error)) {
                return new NotFoundObjectResult(error);
            }

            if (!_xtmfRuntime.ProjectController.GetProjectSession(user, project, out var projectSession, ref error)) {
                return new NotFoundObjectResult(error);
            }

            if (!projectSession.GetModelSystemHeader(user, modelSystemName, out var modelSystemHeader, ref error)) {
                return new NotFoundObjectResult(error);
            }

            return new OkObjectResult(_mapper.Map<ModelSystemModel>(modelSystemHeader));
        }
    }
}