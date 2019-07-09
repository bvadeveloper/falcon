using System.Collections.Generic;

namespace Falcon.Profiles.Scan
{
    public class DomainScanProfile : ScanProfile
    {
        public Dictionary<TagType, string> Tags { get; set; }
    }
}