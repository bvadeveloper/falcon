using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Falcon.Logging;
using Falcon.Tools.Interfaces;

namespace Falcon.Tools
{
    public class ToolsHolder
    {
        public delegate ToolsHolder Factory(string target, ToolType toolType);

        private readonly string _target;
        private List<string> _optionalTools;
        private readonly ToolType _toolType;
        private readonly Lazy<ICollectToolsModel> _collectTools;
        private readonly Lazy<IScanToolsModel> _scanTools;
        private readonly IJsonLogger _logger;

        public ToolsHolder(
            string target,
            ToolType toolType,
            Lazy<ICollectToolsModel> collectTools,
            Lazy<IScanToolsModel> scanTools,
            IJsonLogger<ToolsHolder> logger)
        {
            _target = target;
            _toolType = toolType;
            _collectTools = collectTools;
            _scanTools = scanTools;
            _logger = logger;
        }

        public Task<object> RunAsync()
        {
            switch (_toolType)
            {
                case ToolType.Scan:
                    return _scanTools.Value.MapOptionalTools(_optionalTools).RunToolsAsync(_target);
                case ToolType.Collect:
                    return _collectTools.Value.RunToolsAsync(_target);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Set optional tools
        /// </summary>
        /// <param name="optionalTools"></param>
        /// <returns></returns>
        public ToolsHolder OptionalTools(List<string> optionalTools)
        {
            _optionalTools = optionalTools;
            return this;
        }

        /// <summary>
        /// Get tool names
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetToolNames()
        {
            return _scanTools.Value.Toolset.Select(t => t.Name);
        }
    }
}