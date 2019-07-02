using System.Collections.Generic;

namespace Falcon.Profiles.Scan
{
    public abstract class ScanProfile
    {
        public List<string> Targets { get; set; }
        
        public List<string> Tools { get; set; }
    }
}