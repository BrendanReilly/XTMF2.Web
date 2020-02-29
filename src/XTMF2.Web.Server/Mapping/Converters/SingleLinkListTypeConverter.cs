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
    public class SingleLinkListTypeConverter : ITypeConverter<ReadOnlyObservableCollection<Link>, List<SingleLinkModel>>
    {
        private MappingReferenceTracker _tracker;
        public SingleLinkListTypeConverter(MappingReferenceTracker tracker)
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
        public List<SingleLinkModel> Convert(ReadOnlyObservableCollection<Link> source, List<SingleLinkModel> destination, ResolutionContext context)
        {
            List<SingleLinkModel> linksConvert = new List<SingleLinkModel>();
            foreach (var link in source)
            {
                if (!(link is SingleLink))
                {
                    continue;
                }
                if (!_tracker.References.TryGetValue(link, out var model))
                {
                    model = context.Mapper.Map<SingleLinkModel>(link);
                    _tracker.References[link] = model;
                }

                linksConvert.Add((SingleLinkModel)model);
            }

            return linksConvert;
        }
    }
}