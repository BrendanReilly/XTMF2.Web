
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
using System.Timers;
using Microsoft.Extensions.Options;
using XTMF2.Web.Server.Options;

namespace XTMF2.Web.Server.Services
{
    public class UserTimeoutService
    {
        private readonly Timer _serviceTimer;
        private readonly IOptions<UserTimeoutOptions> _timeoutOptions;
        private readonly TimeSpan _maxUserInactiveTime;
        public Dictionary<User, DateTime> TimeOfUsersLastActivity = new Dictionary<User, DateTime>();

        public UserTimeoutService()
        {
            _maxUserInactiveTime = new TimeSpan(1,0,0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeoutOptions"></param>
        public UserTimeoutService(IOptions<UserTimeoutOptions> timeoutOptions)
        {
            _timeoutOptions = timeoutOptions;

            // 60 second interval default
            _serviceTimer = new Timer(1000 * 60);
            _serviceTimer.Elapsed += OnIntervalElapsed;
            _serviceTimer.AutoReset = true;
            _serviceTimer.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public void RefreshLastActivity(User user)
        {
            TimeOfUsersLastActivity[user] = DateTime.Now;
        }

        /// <summary>
        /// Interval called every minute, checks if a user has been inactive 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnIntervalElapsed(Object source, ElapsedEventArgs e)
        {
            foreach (var user in TimeOfUsersLastActivity.Keys)
            {
                if ((DateTime.Now - TimeOfUsersLastActivity[user]) >= _maxUserInactiveTime)
                {
                    // release this users data and sessions
                }
            }
        }

    }
}