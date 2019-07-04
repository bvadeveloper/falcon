using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Falcon.Logging;
using Falcon.Tools.Interfaces;

namespace Falcon.Tools
{
    public class ToolsHolder
    {
        public delegate ToolsHolder Factory(string target, List<string> tools, ToolType toolType);

        private const int DefaultProcessTimeout = 10000;
        private int _processTimeout;

        private readonly string _target;
        private readonly List<string> _tools;
        private readonly ToolType _toolType;
        private readonly Lazy<ICollectToolsModel> _collectTools;
        private readonly Lazy<IScanToolsModel> _scanTools;
        private readonly IJsonLogger _logger;
        private readonly CancellationToken _cancellationToken;

        public ToolsHolder(
            string target,
            List<string> tools,
            ToolType toolType,
            Lazy<ICollectToolsModel> collectTools,
            Lazy<IScanToolsModel> scanTools,
            IJsonLogger<ToolsHolder> logger)
        {
            _target = target;
            _tools = tools;
            _toolType = toolType;
            _collectTools = collectTools;
            _scanTools = scanTools;
            _logger = logger;
            _cancellationToken = new CancellationTokenSource(_processTimeout).Token;
        }

        public ToolsHolder Timeout(int timeout)
        {
            _processTimeout = timeout == default(int) ? DefaultProcessTimeout : timeout;
            return this;
        }

        public async Task<object> RunAsync()
        {
            switch (_toolType)
            {
                case ToolType.Scan:
                    break;
                case ToolType.Collect:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}