using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using XTMF2.Web.Data.Models.Editing;
using XTMF2.Web.Server.Services;

namespace XTMF2.Web.Server.Mapping.Converters
{
    public class ObjectReferenceValueConverter<TSrc,TDest> : IValueConverter<TSrc,TDest> where TDest : ViewObject
    {
        private MappingReferenceTracker _tracker;
        public ObjectReferenceValueConverter(MappingReferenceTracker tracker)
        {
            _tracker = tracker;
        }
        public TDest Convert(TSrc sourceMember, ResolutionContext context)
        {
            if (_tracker.References.TryGetValue(sourceMember, out var model))
            {
                return (TDest)model;
            }

            return null;
        }
    }
}
