using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XTMF2.Web.Data.Models.Editing;

namespace XTMF2.Web.Server.Services
{
    public  class MappingReferenceTracker
    {
        public  Dictionary<object, ViewObject> References = new Dictionary<object, ViewObject>();
    }
}
