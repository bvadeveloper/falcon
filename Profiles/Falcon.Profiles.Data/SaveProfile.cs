using System;
using System.Collections.Generic;

namespace Falcon.Profiles.Data
{
    public class SaveProfile : ISession
    {
        public Dictionary<TagType, string> Tags { get; set; }

        public IContext Context { get; set; }

        public DateTime ScanDate { get; set; }
    }
}