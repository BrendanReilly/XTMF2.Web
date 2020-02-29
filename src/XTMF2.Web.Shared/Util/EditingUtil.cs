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
using XTMF2.Web.Data.Models.Editing;

namespace XTMF2.Web.Shared.Util
{
    /// <summary>
    /// </summary>
    public class EditingUtil
    {
        /// <summary>
        ///     Traverses the passed object and all child objects of the passed view object.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static IEnumerable<ViewObject> Traverse(ViewObject o)
        {
            return ModelSystemObjectsTraverse(o);
        }

        /// <summary>
        ///     Iterate over all model system objects
        /// </summary>
        /// <param name="modelSystem"></param>
        /// <returns></returns>
        public static IEnumerable<ViewObject> ModelSystemObjects(ModelSystemEditingModel modelSystem)
        {
            return ModelSystemObjectsTraverse(modelSystem.GlobalBoundary);
        }

        private static IEnumerable<ViewObject> ModelSystemObjectsTraverse(ViewObject viewObject)
        {
            yield return viewObject;
            if (viewObject is BoundaryModel boundary)
            {
                foreach (var c in boundary.CommentBlocks)
                {
                    yield return c;
                }

                foreach (var s in boundary.Starts)
                {
                    yield return s;
                }

                foreach (var n in boundary.Modules)
                {
                    yield return n;
                }

                foreach (var link in boundary.Links)
                {
                    yield return link;
                }

                foreach (var b in boundary.Boundaries)
                {
                    foreach (var boundaryChild in ModelSystemObjectsTraverse(b))
                    {
                        yield return boundaryChild;
                    }
                }
            }
        }
    }
}