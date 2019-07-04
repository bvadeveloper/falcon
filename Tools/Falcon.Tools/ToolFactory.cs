using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Falcon.Tools
{
    public class ToolFactory
    {
        private const int DefaultProcessTimeout = 10000;
        private readonly CancellationToken _cancellationToken;
        private string _target;
        private List<string> _tools;

        private ToolFactory(int timeout)
        {
            _cancellationToken = new CancellationTokenSource(timeout).Token;
        }

        public static ToolFactory Init(int timeout = default(int))
        {
            return new ToolFactory(timeout == default(int) ? DefaultProcessTimeout : timeout);
        }

        public Task<string[]> RunAsync()
        {
            var tasks = _tools.Select(command => Task.Run(() => new ToolRunner()
                .Run(command)
                .GetOutput(), _cancellationToken));

            return Task.WhenAll(tasks);
        }

        public ToolFactory AddTarget(string target)
        {
            _target = target;
            return this;
        }

        public ToolFactory UseCollectTools()
        {
            _tools = new List<string> { $"nmap -v -A {_target}" };
            return this;
        }

        public ToolFactory UseTools(List<string> tools)
        {
            _tools = tools;
            return this;
        }
    }
}