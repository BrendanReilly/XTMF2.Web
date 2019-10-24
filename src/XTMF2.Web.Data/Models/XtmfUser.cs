﻿using System;

namespace XTMF2.Web.Data.Models
{
    /// <summary>
    /// Facade object for XTMF2.Users.
    /// </summary>
    public class XtmfUser
    {
        /// <summary>
        /// The backing XTMF2.User
        /// </summary>
        private XTMF2.User _user;

        /// <summary>
        /// 
        /// </summary>
        public XTMF2.User User => _user;

        /// <summary>
        /// 
        /// </summary>
        public bool IsAdmin => _user.IsAdmin;

        /// <summary>
        /// 
        /// </summary>
        public string UserName => _user.UserName;

        /// <summary>
        /// Default no argument constructor.
        /// </summary>
        public XtmfUser(XTMF2.User user)
        {
            this._user = user;
        }


    }
}
