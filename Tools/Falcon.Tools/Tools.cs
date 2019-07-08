using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Falcon.Tools.Interfaces;

namespace Falcon.Tools
{
    public class Tools : ICollectToolsModel, IScanToolsModel
    {
        public List<ToolModel> Toolset { get; set; }

        private static CancellationToken MakeCancellationToken(int timeout) =>
            new CancellationTokenSource(timeout).Token;

        public async Task<List<OutputModel>> RunToolsAsync(string target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var tasks = Toolset.Select(t =>
                new ToolRunner()
                    .Init(t.Name, t.MakeCommandLine(t.CommandLine))
                    .MakeTask(MakeCancellationToken(t.Timeout)));

            var results = await Task.WhenAll(tasks);

            return results.Select(r => r.MakeOutput()).ToList();
        }

        public async Task<List<OutputModel>> RunToolsVersionCommandAsync()
        {
            var tasks = Toolset.Select(t =>
                new ToolRunner()
                    .Init(t.Name, t.VersionCommandLine)
                    .MakeTask(MakeCancellationToken(t.Timeout)));

            var results = await Task.WhenAll(tasks);

            return results.Select(r => r.MakeOutput()).ToList();
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