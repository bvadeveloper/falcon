using System.Collections.Generic;

namespace Falcon.Profiles.Scan
{
    public class ScanDomainProfile : ScanProfile
    {
        public Dictionary<TargetAttributes, string> TargetData { get; set; }
    }
}