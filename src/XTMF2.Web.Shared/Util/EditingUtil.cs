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