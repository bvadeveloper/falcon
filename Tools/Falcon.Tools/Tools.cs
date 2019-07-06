using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Falcon.Logging;
using Falcon.Tools.Interfaces;

namespace Falcon.Tools
{
    public class Tools : ICollectToolsModel, IScanToolsModel
    {
        private readonly IJsonLogger _logger;

        public Tools(IJsonLogger<Tools> logger)
        {
            _logger = logger;
        }

        public List<ToolModel> Toolset { get; set; }

        private static CancellationToken MakeCancellationToken(int timeout) =>
            new CancellationTokenSource(timeout).Token;

        public async Task<IEnumerable<OutputModel>> RunToolsAsync(string target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var tasks = Toolset.Select(t => new ToolRunner()
                .Init(t.Name, t.MakeCommandLine(t.CommandLine))
                .MakeTask(MakeCancellationToken(t.Timeout)));

            var results = await Task.WhenAll(tasks);
            var outputs = results.Select(r => r.MakeOutput()).ToList();

            LogOutputs(outputs);

            return outputs.Where(o => o.Successful);
        }

        private void LogOutputs(List<OutputModel> outputs)
        {
            outputs.ForEach(m =>
            {
                if (m.Successful)
                {
                    _logger.Trace("Successful processing", m);
                }
                else
                {
                    _logger.Error("Tool failure", new { m.ToolName, m.ErrorOutput }, m.ExecutionException);
                }
            });
        }

        public async Task<IEnumerable<OutputModel>> RunToolsVersionCommandAsync()
        {
            var tasks = Toolset.Select(t =>
                new ToolRunner()
                    .Init(t.Name, t.VersionCommandLine)
                    .MakeTask(MakeCancellationToken(t.Timeout)));

            var results = await Task.WhenAll(tasks);
            var outputs = results.Select(r => r.MakeOutput()).ToList();

            LogOutputs(outputs);

            return outputs.Where(o => o.Successful);
        }

        public IScanToolsModel UseOnlyTools(List<string> specificTools)
        {
            if (specificTools != null && specificTools.Any())
            {
                this.Toolset = this.Toolset.Where(m => specificTools.Contains(m.Name)).ToList();
            }

            return this;
        }
    }
}