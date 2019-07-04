using System.Collections.Generic;
using Falcon.Tools.Interfaces;

namespace Falcon.Tools
{
    /// <inheritdoc />
    public class ToolModel : IToolModel
    {
        public string Name { get; set; }

        public string Info { get; set; }

        public string CommonTags { get; set; }

        public string HostTags { get; set; }

        public string CommandLine { get; set; }

        public string MakeCommandLine(string target) => string.Format(CommandLine, target);
    }

    public class ToolsModel : ICollectToolsModel, IScanToolsModel
    {
        public List<ToolModel> Toolset { get; set; }
    }
}