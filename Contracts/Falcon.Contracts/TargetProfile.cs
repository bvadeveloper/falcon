using System.Collections.Generic;

namespace Falcon.Contracts
{
    public class TargetProfile
    {
        public string Target { get; set; }

        public Dictionary<TargetAttributes, string> TargetData { get; set; }

        public List<string> Tools { get; set; }
    }

    public enum TargetAttributes
    {
        Server,
        Cms

        //etc
    }
}