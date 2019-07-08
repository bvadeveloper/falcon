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

        public int Timeout { get; set; }

        public string VersionCommandLine { get; set; }

        public string MakeCommandLine(string target) => string.Format(CommandLine, target);
    }
}