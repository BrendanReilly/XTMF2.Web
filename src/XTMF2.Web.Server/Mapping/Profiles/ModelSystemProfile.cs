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
using System.Collections.ObjectModel;
using AutoMapper;
using XTMF2.ModelSystemConstruct;
using XTMF2.Web.Data.Models;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Server.Mapping.Actions;
using XTMF2.Web.Server.Mapping.Converters;
using XTMF2.Web.Server.Services;

namespace XTMF2.Web.Server.Mapping.Profiles
{
    /// <summary>
    ///     AutoMapper profile
    /// </summary>
    public class ModelSystemProfile : Profile
    {
        public ModelSystemProfile()
        {
            CreateMap<ModelSystemHeader, ModelSystemModel>();
            MapEditorProfile();
        }

        /// <summary>
        ///     Create mapping profiles for model system editor objects
        /// </summary>
        private void MapEditorProfile()
        {
            CreateMap<ModelSystem, ModelSystemEditingModel>();
            CreateMap<Boundary, BoundaryModel>()
                .BeforeMap<Actions.GenerateModelSystemObjectIdAction<Boundary, BoundaryModel>>();

            CreateMap<Link, LinkModel>()
                .BeforeMap<Actions.GenerateModelSystemObjectIdAction<Link, LinkModel>>();
            CreateMap<Link, MultiLinkModel>()
                .BeforeMap<Actions.GenerateModelSystemObjectIdAction<Link, MultiLinkModel>>();
            CreateMap<MultiLink, LinkModel>()
                .BeforeMap<Actions.GenerateModelSystemObjectIdAction<MultiLink, LinkModel>>();
            CreateMap<SingleLink, LinkModel>()
                .BeforeMap<Actions.GenerateModelSystemObjectIdAction<SingleLink, LinkModel>>()
                .AfterMap((src, dest) =>
                {
                    dest.OriginHookId = dest.OriginHook.Id;
                    dest.OriginId = dest.Origin.Id;
                });
            CreateMap<Start, StartModel>()
                .ForMember(m => m.TypeString, opt => opt.MapFrom(x => x.Type.ToString()))
                .BeforeMap<Actions.GenerateModelSystemObjectIdAction<Start, StartModel>>();

            CreateMap<Node, NodeModel>()
                .ForMember(m => m.ContainedWithin, opt => { opt.MapFrom(x => x.ContainedWithin); })
                .BeforeMap<Actions.GenerateModelSystemObjectIdAction<Node, NodeModel>>()
                .ForMember(m => m.TypeString, opt => opt.MapFrom(x => x.Type.ToString()))
                .AfterMap((src, dest) => { dest.ContainedWithinId = dest.ContainedWithin.Id; });

            CreateMap<NodeHook, NodeHookModel>()
                .ForMember(m => m.TypeString, opt => opt.MapFrom(x => x.Type.ToString()))
                .BeforeMap<Actions.GenerateModelSystemObjectIdAction<NodeHook, NodeHookModel>>();
            CreateMap<CommentBlock, CommentBlockModel>()
                .BeforeMap<Actions.GenerateModelSystemObjectIdAction<CommentBlock, CommentBlockModel>>();
            CreateMap<Rectangle, Data.Types.Rectangle>();
            CreateMap<Data.Types.Rectangle,Rectangle > ();

        }

    }
}