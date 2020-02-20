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

using System.Linq;
using XTMF2.Editing;
using XTMF2.Web.Server.Session;

namespace XTMF2.Web.Server.Utils
{

    public class XtmfUtils
    {

        /// <summary>
        /// Retrieves an active reference to a project session or creates a new one if one does not exist.
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="userSession"></param>
        /// <param name="projectName"></param>
        /// <param name="projectSession"></param>
        /// <param name="projectSessions"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool GetProjectSession(XTMFRuntime runtime, UserSession userSession, string projectName,
            out ProjectSession projectSession, ModelSystemEditingSessions editingSessions,
             out CommandError error)
        {
            var project = GetProject(projectName, userSession);
            if (project == null)
            {
                error = new CommandError("Project does not exist.");
                projectSession = null;
                return false;
            }
            // determine if the project session already exists
            if (editingSessions.ProjectSessions.ContainsKey(userSession.User))
            {
                projectSession = editingSessions.ProjectSessions[userSession.User].FirstOrDefault(p => p.Project == project);
                if (projectSession != null)
                {
                    error = null;
                    return true;
                }
            }
            // otherwise get the project session from the xtmf project controller
            if (!runtime.ProjectController.GetProjectSession(userSession.User, project, out projectSession, out error))
            {
                return false;
            }
            // store the project session
            editingSessions.TrackProjectSessionForUser(userSession.User, projectSession);
            return true;
        }

        /// <summary>
        /// Utility for compacting call access to retrieving a model system header.
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="user"></param>
        /// <param name="projectName"></param>
        /// <param name="modelSystemName"></param>
        /// <param name="modelSystem"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool GetModelSystemHeader(XTMFRuntime runtime, UserSession userSession, ModelSystemEditingSessions editingSessions,
            string projectName, string modelSystemName, out ModelSystemHeader modelSystem, out CommandError error)
        {
            if (!GetProjectSession(runtime, userSession, projectName, out var projectSession, editingSessions, out error))
            {
                modelSystem = null;
                return false;
            }
            if (!projectSession.GetModelSystemHeader(userSession.User, modelSystemName, out modelSystem, out error))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Retrieves a reference to the project stored in the user session.
        /// </summary>
        /// <param name="projectName">The name of the project</param>
        /// <param name="session">The associated usre session</param>
        /// <returns>Returns the project, or null if it does not exist.</returns>
        public static Project GetProject(string projectName, UserSession session)
        {
            var project = session.Projects.FirstOrDefault(p => p.Name == projectName);
            return project;
        }
    }
}