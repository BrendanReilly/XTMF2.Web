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
using System.Linq;
using AutoMapper;
using XTMF2.Editing;
using XTMF2.Web.Data.Models.Editing;

namespace XTMF2.Web.Server.Session
{
    /// <summary>
    ///     Holds the session state for an active user. Maintains references to projects and other
    ///     related items.
    /// </summary>
    public class ModelSystemEditingSessions
    {

        /// <summary>
        ///     User dictionary of active model system sessions.
        /// </summary>
        /// <returns></returns>

        public Dictionary<ValueTuple<User, Project>, List<ModelSystemSession>> UserModelSystemEditingSessions { get; } =
        new Dictionary<ValueTuple<User, Project>, List<ModelSystemSession>>();

        /// <summary>
        /// Model system object reference tracker. Maps a model system session to the currently "active"
        /// view model.
        /// </summary>
        /// <returns></returns>
        public Dictionary<ModelSystemSession, ModelSystemEditingModel> ModelSystemEditingModels { get; } =
        new Dictionary<ModelSystemSession, ModelSystemEditingModel>();

        /// <summary>
        /// Stores reference to model system editing trackers
        /// </summary>
        /// <value></value>
        public Dictionary<ModelSystemSession, ModelSystemEditingTracker> ModelSystemEditingTrackers { get; } =
        new Dictionary<ModelSystemSession, ModelSystemEditingTracker>();

        public Dictionary<User, List<ProjectSession>> ProjectSessions { get; } = new Dictionary<User, List<ProjectSession>>();

        private readonly IMapper _mapper;

        private readonly XTMFRuntime _runtime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="runtime"></param>
        public ModelSystemEditingSessions(IMapper mapper, XTMFRuntime runtime)
        {
            _mapper = mapper;
            _runtime = runtime;
        }

        /// <summary>
        ///     Clears all model system sessions for the associated user
        /// </summary>
        /// <param name="user">The user to clear sessions for.</param>
        public void ClearSessionsForUser(User user)
        {
            if (!ProjectSessions.ContainsKey(user))
            {
                return;
            }
            var projectSessions = ProjectSessions[user];
            foreach (var projectSession in projectSessions)
            {
                if (UserModelSystemEditingSessions.TryGetValue((user, projectSession.Project), out var modelSystemSessions))
                {
                    foreach (var modelSystemSession in modelSystemSessions)
                    {
                        modelSystemSession.Dispose();
                        if (modelSystemSession.References <= 0)
                        {
                            ModelSystemEditingModels.Remove(modelSystemSession);
                            ModelSystemEditingTrackers[modelSystemSession].Dispose();
                            ModelSystemEditingTrackers.Remove(modelSystemSession);
                        }
                    }
                    UserModelSystemEditingSessions[(user, projectSession.Project)].Clear();
                }
            }
        }

        /// <summary>
        ///     Adds / tracks a session for the associated user.
        /// </summary>
        /// <param name="user">The user to track the session with.</param>
        /// <param name="session">The session to track.</param>
        public void TrackSessionForUser(User user, Project project, ModelSystemSession session)
        {
            if (!UserModelSystemEditingSessions.ContainsKey((user, project)))
            {
                InitializeNewModelSystemSessionForUser(user, project, session.ModelSystemHeader, out var commandError);
            }
        }

        /// <summary>
        /// Retrieves the active editing model for the passed model system session
        /// </summary>
        /// <param name="session">The model system session</param>
        /// <returns>The editing model for the passed model system / session</returns>
        public ModelSystemEditingModel GetModelSystemEditingModel(ModelSystemSession session)
        {
            if (!ModelSystemEditingModels.TryGetValue(session, out var model))
            {
                model = _mapper.Map<ModelSystemEditingModel>(session.ModelSystem);
                ModelSystemEditingModels[session] = model;
            }
            return model;
        }

        /// <summary>
        /// Gets a model system session for the associate model system and project. If a model system session does not
        /// already exist, a new one is created and stored.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="project"></param>
        /// <param name="modelSystemHeader"></param>
        /// <returns></returns>
        public bool GetModelSystemSession(User user, Project project, ModelSystemHeader modelSystemHeader, out ModelSystemSession session, out CommandError commandError)
        {
            if (!ModelSystemSessionExistsForUser(user, project, modelSystemHeader))
            {
                InitializeNewModelSystemSessionForUser(user, project, modelSystemHeader, out commandError);
            }
            commandError = null;
            session = UserModelSystemEditingSessions[(user, project)].FirstOrDefault(s => s.ModelSystemHeader == modelSystemHeader);
            return session == null ? false : true;
        }

        private bool ModelSystemSessionExistsForUser(User user, Project project, ModelSystemHeader header)
        {
            if (!UserModelSystemEditingSessions.TryGetValue((user, project), out var sessions))
            {
                return false;
            }
            return sessions.Any(s => s.ModelSystemHeader == header);
        }

        /// <summary>
        /// 
        /// </summary>
        /// /// <param name="user"></param>
        /// <param name="project"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        private bool InitializeNewModelSystemSessionForUser(User user, Project project, ModelSystemHeader header, out CommandError commandError)
        {
            if(!GetProjectSession(user,project, out var projectSession, out commandError)) {
                return false;
            }
            if (!projectSession.EditModelSystem(user, header, out var modelSystemSession, out commandError))
            {
                return false;
            }
            if (!UserModelSystemEditingSessions.TryGetValue((user, project), out var sessions))
            {
                sessions = new List<ModelSystemSession>();
                UserModelSystemEditingSessions[(user, project)] = sessions;
            }
            ModelSystemEditingTrackers[modelSystemSession] = new ModelSystemEditingTracker(modelSystemSession, _mapper);
            sessions.Add(modelSystemSession);
            return true;
        }

        /// <summary>
        /// Attempts to get a project session for the user / project. If it does not exist, a new session is spun up.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="project"></param>
        /// <param name="projectSession"></param>
        /// <param name="commandError"></param>
        /// <returns></returns>
        public bool GetProjectSession(User user, Project project, out ProjectSession projectSession, out CommandError commandError)
        {
            if (!ProjectSessions.TryGetValue(user, out var userProjectSessions))
            {
                userProjectSessions = new List<ProjectSession>();
                ProjectSessions[user] = userProjectSessions;
            }

            projectSession = userProjectSessions.FirstOrDefault(p => p.Project == project);
            if (projectSession == null)
            {
                // add the new projectSession
                if (!_runtime.ProjectController.GetProjectSession(user, project, out projectSession, out commandError))
                {
                    return false;
                }
                else
                {
                    userProjectSessions.Add(projectSession);
                }
            }
            commandError = null;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public ModelSystemEditingTracker GetModelSystemEditingTracker(ModelSystemSession session)
        {
            return ModelSystemEditingTrackers[session];
        }

        /// <summary>
        ///     Clears all project sessions for the associated user
        /// </summary>
        /// <param name="user"></param>
        public void ClearProjectSessionsForUser(User user)
        {
            if (ProjectSessions.ContainsKey(user))
            {
                //dispose each session
                foreach (var session in ProjectSessions[user])
                {
                    session.Dispose();
                }
                ProjectSessions[user].Clear();
            }
        }

        /// <summary>
        ///     Adds / tracks a session for the associated user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="session"></param>
        public void TrackProjectSessionForUser(User user, ProjectSession session)
        {
            if (!ProjectSessions.ContainsKey(user)) { ProjectSessions[user] = new List<ProjectSession>(); }

            ProjectSessions[user].Add(session);
        }
    }
}