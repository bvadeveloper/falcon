using System;
using System.Collections.Generic;

namespace Falcon.Profiles.Data
{
    public class SaveProfile : ISessionContext
    {
        public Dictionary<TagType, string> Tags { get; set; }

        public SessionContext Context { get; set; }

        public DateTime ScanDate { get; set; }
    }
}