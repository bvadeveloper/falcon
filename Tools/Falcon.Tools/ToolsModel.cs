using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public string MakeCommandLine(string target) => string.Format(CommandLine, target);
    }

    public class ToolsModel : ICollectToolsModel, IScanToolsModel
    {
        public List<ToolModel> Toolset { get; set; }

        public Task<object> RunToolsAsync(string target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            return Task.FromResult<object>(target);
        }

        /// <summary>
        /// Map tools only for scanners
        /// </summary>
        /// <param name="optionalTools"></param>
        /// <returns></returns>
        public IScanToolsModel MapOptionalTools(List<string> optionalTools)
        {
            return optionalTools != null && optionalTools.Any()
                ? new ToolsModel { Toolset = this.Toolset.Where(m => optionalTools.Contains(m.Name)).ToList() }
                : this;
        }
    }
}