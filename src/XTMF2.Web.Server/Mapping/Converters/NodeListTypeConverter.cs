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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoMapper;
using Namotion.Reflection;
using XTMF2.ModelSystemConstruct;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Server.Services;

namespace XTMF2.Web.Server.Mapping.Converters
{
    /// <summary>
    ///     Maps Links to the appropriate link type
    /// </summary>
    public class NodeListTypeConverter<TNodeSrc, TDest> : ITypeConverter<ReadOnlyObservableCollection<TNodeSrc>, List<TDest>> where TDest : NodeModel where TNodeSrc : Node
    {
        private MappingReferenceTracker _tracker;
        public NodeListTypeConverter(MappingReferenceTracker tracker)
        {
            _tracker = tracker;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<TDest> Convert(ReadOnlyObservableCollection<TNodeSrc> source, List<TDest> destination, ResolutionContext context)
        {
            List<TDest> nodesConvert = new List<TDest>();
            foreach (var node in source)
            {
                if (!(node is TNodeSrc))
                {
                    continue;
                }
                if (!_tracker.References.TryGetValue(node, out var model))
                {
                    model = context.Mapper.Map<TDest>(node);
                    _tracker.References[node] = model;
                }

                nodesConvert.Add((TDest)model);
            }

            return nodesConvert;
        }
    }
}