using System.Collections.Generic;

namespace Falcon.Profiles.Scan
{
    public class DomainScanProfile : ScanProfile 
    {
        public Dictionary<TargetTag, string> TargetTags { get; set; }
    }
}