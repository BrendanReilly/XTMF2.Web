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

using Microsoft.AspNetCore.Mvc.Filters;
using XTMF2.Web.Server.Services;
using XTMF2.Web.Server.Session;

namespace XTMF2.Web.Server.Controllers
{
    public class UserTimeoutActionFilter : IActionFilter
    {
        private readonly UserTimeoutService _timeoutService;
        private readonly UserSession _userSession;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeoutService"></param>
        /// <param name="userSession"></param>
        public UserTimeoutActionFilter(UserTimeoutService timeoutService, UserSession userSession) {
            _timeoutService = timeoutService;
            _userSession = userSession;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Do something before the action executes.
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if(_userSession.User != null) {
                _timeoutService.RefreshLastActivity(_userSession.User);
            }
        }
    }
}