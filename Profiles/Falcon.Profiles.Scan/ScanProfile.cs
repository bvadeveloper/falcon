using System.Collections.Generic;

namespace Falcon.Profiles.Scan
{
    public class ScanProfile
    {
        public string Target { get; set; }

        public Dictionary<TargetAttributes, string> TargetData { get; set; }

        public List<string> Tools { get; set; }
    }
}