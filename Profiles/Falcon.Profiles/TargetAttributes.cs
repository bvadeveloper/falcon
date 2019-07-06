using System.Collections.Generic;

namespace Falcon.Profiles
{
    public class TargetAttributes
    {
        public string Server { get; set; }

        public string Framework { get; set; }

        public string Database { get; set; }

        public List<string> SubDomains { get; set; }

        public List<string> Ports { get; set; }

        public bool HasHttps { get; set; }

        public List<string> Ns { get; set; }

        public string ContactInfo { get; set; }

        //etc
    }
}