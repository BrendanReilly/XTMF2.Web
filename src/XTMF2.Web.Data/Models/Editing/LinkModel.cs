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
using System.Text.Json.Serialization;
using XTMF2.Web.Data.Interfaces.Editing;
using XTMF2.Web.Data.Types;

namespace XTMF2.Web.Data.Models.Editing
{
    public class LinkModel : ViewObject
    {
        [JsonIgnore]
        public NodeModel Origin { get; set; }

        [JsonIgnore]
        public NodeHookModel OriginHook { get; set; }

        public Guid OriginId { get; set; }

        public Guid OriginHookId { get; set; }

        public List<Guid> DestinationIds { get; set; }

        [JsonIgnore]
        public List<NodeModel> Destinations { get; set; }

        public LinkType LinkType { get; set; }
    }

    public enum LinkType
    {
        Single,
        Multi
    }
}