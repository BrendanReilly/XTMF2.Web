using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using XTMF2.Web.Data.Models;
using XTMF2.Web.Server.Controllers;
using XTMF2.Web.Server.Profiles;
using XTMF2.Web.Server.Session;
using Xunit;

namespace XTMF2.Web.UnitTests.Controllers {
    public class ProjectControllerUnitTests {

        private IMapper _mapper;
        private XTMFRuntime _runtime;
        private ILogger<ProjectController> _logger;
        private ProjectController _controller;
        private UserSession _userSession;

        public ProjectControllerUnitTests() {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ProjectProfile>();
            });
            _mapper = config.CreateMapper();
            _runtime = TestHelper.CreateTestContext();
            _logger = Mock.Of<ILogger<ProjectController>>();
            _controller = new ProjectController(_runtime, _logger, _mapper);
            _userSession = new UserSession(_runtime.UserController.GetUserByName("TempUser"));

        }

        /// <summary>
        /// Tests if the projects list returns a valid list for the default (local) user.
        /// </summary>
        [Fact]
        public void IndexGet_ReturnsValidList() {
            var projects = _controller.List(_userSession);
            //assert
            Assert.IsAssignableFrom<ActionResult<IEnumerable<ProjectModel>>>(projects);
        }

        /// <summary>
        /// Tests creating a user, and that the controller returns the correct response type.
        /// </summary>
        [Fact]
        public void CreatePost_ReturnsCreatedResult_WhenProjectCreated() {
            var result = _controller.Create("projectName", _userSession);
            var projects = _controller.List(_userSession);
            //assert
            Assert.IsAssignableFrom<CreatedResult>(result);
            Assert.IsAssignableFrom<OkObjectResult>(projects.Result);
            Assert.Single(((List<ProjectModel>) ((OkObjectResult) projects.Result).Value));

        }

        /// <summary>
        /// Tests deleting a single project
        /// </summary>
        [Fact]
        public void DeletePost_ReturnsValid_WhenProjectDeleted() {
            _controller.Create("projectName", _userSession);
            var result = _controller.Delete("projectName", _userSession);
            //assert
            Assert.IsAssignableFrom<OkResult>(result);
        }
        
        /// <summary>
        /// Tests deletion of a non-existing project
        /// </summary>
        [Fact]
        public void DeletePost_ReturnsNotFound_WhenProjectDeleted() {
            var result = _controller.Delete("projectName", _userSession);
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        /// <summary>
        /// Tests retrieving of a single project
        /// </summary>
        [Fact]
        public void Get_ReturnsValid_WhenProjectRetrieved() {
            _controller.Create("projectName", _userSession);
            var result = _controller.Get("projectName", _userSession);
            Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Equal("projectName",((ProjectModel) ((OkObjectResult) result).Value).Name);
        }

        /// <summary>
        /// Tests single get project, should return not found.
        /// </summary>
        [Fact]
        public void Get_ReturnsNotFound_WhenProjectRetrieved() {
            _controller.Create("projectName", _userSession);
            var result = _controller.Get("projectNameNoTvalid", _userSession);
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

    }
}