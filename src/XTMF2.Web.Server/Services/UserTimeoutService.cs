
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
using Microsoft.Extensions.Options;
using XTMF2.Web.Server.Options;

namespace XTMF2.Web.Server.Services {
    public class UserTimeoutService  {

        private readonly IOptions<UserTimeoutOptions> _timeoutOptions;
        public Dictionary<User, DateTime> TimeOfUsersLastActivity = new Dictionary<User, DateTime>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeoutOptions"></param>
        public UserTimeoutService(IOptions<UserTimeoutOptions> timeoutOptions) {
            _timeoutOptions = timeoutOptions;
            Console.WriteLine("h");
        }

    }
}