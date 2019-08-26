using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Falcon.Tools.Interfaces;

namespace Falcon.Tools
{
    public class ToolsRunner : ICollectToolsModel, IScanToolsModel
    {
        public List<ToolModel> Toolset { get; set; }

        private static CancellationToken MakeCancellationToken(int timeout) =>
            new CancellationTokenSource(timeout).Token;

        public async Task<List<OutputModel>> RunToolsAsync(string target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var tasks = Toolset.Select(t =>
                new ToolProcess()
                    .Init(t.Name, t.MakeCommandLine(target))
                    .MakeTask(MakeCancellationToken(t.Timeout)));

            var results = await Task.WhenAll(tasks);

            return results.Select(r => r.MakeOutput()).ToList();
        }

        public async Task<List<OutputModel>> RunToolsVersionCommandAsync()
        {
            var tasks = Toolset.Select(t =>
                new ToolProcess()
                    .Init(t.Name, t.VersionCommandLine)
                    .MakeTask(MakeCancellationToken(t.Timeout)));

            var results = await Task.WhenAll(tasks);

            return results.Select(r => r.MakeOutput()).ToList();
        }

        public IScanToolsModel UseOnly(List<string> optionalTools)
        {
            if (optionalTools != null && optionalTools.Any())
            {
                Toolset = Toolset.Where(m => optionalTools.Contains(m.Name)).ToList();
            }

            return this;
        }
    }
}