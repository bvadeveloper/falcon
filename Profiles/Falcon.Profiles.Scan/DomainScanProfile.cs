using System.Collections.Generic;

namespace Falcon.Profiles.Scan
{
    public class DomainScanProfile : ScanProfile
    {
        public Dictionary<TagType, List<string>> Tags { get; set; }
    }
}