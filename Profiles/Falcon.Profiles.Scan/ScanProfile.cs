using System.Collections.Generic;

namespace Falcon.Profiles.Scan
{
    public abstract class ScanProfile
    {
        public string Target { get; set; }

        public List<string> Tools { get; set; }
    }
}