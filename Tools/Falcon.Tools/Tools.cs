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

        public async Task<List<string>> RunToolsAsync(string target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var tasks = Toolset.Select(t =>
                new ToolRunner().MakeTask(t.MakeCommandLine(target), new CancellationTokenSource(t.Timeout).Token));

            var results = await Task.WhenAll(tasks);

            var outputs = results.Select(c => c.GetOutput()).ToList();


            return outputs;
        }

        public IScanToolsModel MapOptionalTools(List<string> optionalTools)
        {
            return optionalTools != null && optionalTools.Any()
                ? new Tools { Toolset = this.Toolset.Where(m => optionalTools.Contains(m.Name)).ToList() }
                : this;
        }
    }
}