﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XTMF2.Web.Data.Interfaces;

namespace XTMF2.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        /// <summary>
        /// Creates a new project from the model passed.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(IProject project) {

            return new OkResult();
        }

        /// <summary>
        /// Deletes the specified project.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult Delete(IProject project) {
            return new OkResult();
        }
    }
}