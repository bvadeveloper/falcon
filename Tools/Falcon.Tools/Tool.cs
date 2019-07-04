using System.Collections.Generic;
using Falcon.Tools.Interfaces;

namespace Falcon.Tools
{
    public class Tool : ITool
    {
        public string Name { get; set; }

        public string Info { get; set; }

        public string CommonTags { get; set; }

        public string HostTags { get; set; }

        public string CommandLine { get; set; }

        public string MakeCommandLine(string target) => string.Format(CommandLine, target);
    }

    public class Tools : ICollectTools, IScanTools
    {
        public List<ITool> ToolCollection { get; set; }
    }
}