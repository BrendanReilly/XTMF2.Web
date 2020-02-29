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

using AutoMapper;
using XTMF2.ModelSystemConstruct;
using XTMF2.Web.Data.Models;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Server.Mapping.Actions;

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
                .BeforeMap<GenerateModelSystemObjectIdAction<Boundary, BoundaryModel>>();

            CreateMap<Link, LinkModel>()
                .BeforeMap<GenerateModelSystemObjectIdAction<Link, LinkModel>>();
            CreateMap<Link, MultiLinkModel>()
                .BeforeMap<GenerateModelSystemObjectIdAction<Link, MultiLinkModel>>();
            CreateMap<MultiLink, LinkModel>()
                .BeforeMap<GenerateModelSystemObjectIdAction<MultiLink, LinkModel>>()
                .AfterMap((src, dest) => { dest.LinkType = LinkType.Multi; });
            CreateMap<SingleLink, LinkModel>()
                .BeforeMap<GenerateModelSystemObjectIdAction<SingleLink, LinkModel>>()
                .AfterMap((src, dest) =>
                {
                    dest.LinkType = LinkType.Single;
                    dest.OriginHookId = dest.OriginHook.Id;
                    dest.OriginId = dest.Origin.Id;
                });
            CreateMap<Start, StartModel>()
                .ForMember(m => m.TypeString, opt => opt.MapFrom(x => x.Type.ToString()))
                .BeforeMap<GenerateModelSystemObjectIdAction<Start, StartModel>>();

            CreateMap<Node, NodeModel>()
                .ForMember(m => m.ContainedWithin, opt => { opt.MapFrom(x => x.ContainedWithin); })
                .BeforeMap<GenerateModelSystemObjectIdAction<Node, NodeModel>>()
                .ForMember(m => m.TypeString, opt => opt.MapFrom(x => x.Type.ToString()))
                .AfterMap((src, dest) => { dest.ContainedWithinId = dest.ContainedWithin.Id; });

            CreateMap<NodeHook, NodeHookModel>()
                .ForMember(m => m.TypeString, opt => opt.MapFrom(x => x.Type.ToString()))
                .BeforeMap<GenerateModelSystemObjectIdAction<NodeHook, NodeHookModel>>();
            CreateMap<CommentBlock, CommentBlockModel>()
                .BeforeMap<GenerateModelSystemObjectIdAction<CommentBlock, CommentBlockModel>>();
            CreateMap<Rectangle, Data.Types.Rectangle>();
            CreateMap<Data.Types.Rectangle, Rectangle>();
        }
    }
}