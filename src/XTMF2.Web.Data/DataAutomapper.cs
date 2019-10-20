﻿using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using XTMF2.Web.Data.Models;

namespace XTMF2.Web.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DataAutoMapper
    {
        public MapperConfiguration Configuration { get; set; }

        public DataAutoMapper()
        {
            this.ConfigureAutoMapper();
        }
        private void ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<XTMF2.User, XtmfUser>()
                    .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
                    .ForMember(d => d.IsAdmin, o => o.MapFrom(s => s.Admin));
            });

            this.Configuration = config;

        }
    }
}
