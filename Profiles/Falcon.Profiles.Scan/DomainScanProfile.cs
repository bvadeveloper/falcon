using System.Collections.Generic;

namespace Falcon.Profiles.Scan
{
    public class DomainScanProfile : ScanProfile 
    {
        public Dictionary<TargetAttributes, string> TargetData { get; set; }
    }
}