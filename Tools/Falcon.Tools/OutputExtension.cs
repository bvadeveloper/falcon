using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Falcon.Logging;

namespace Falcon.Tools
{
    public static class OutputExtension
    {
        public static async Task<IEnumerable<OutputModel>> LogOutputsAsync(
            this Task<IEnumerable<OutputModel>> tasks, IJsonLogger logger)
        {
            var outputs = await tasks;
            var toolOutputModels = outputs.ToList();

            foreach (var m in toolOutputModels)
            {
                if (m.Successful)
                {
                    logger.Trace("Successful tool processing", m);
                }
                else
                {
                    logger.Error("Tool failure", new { m.Output, m.ErrorOutput }, m.ExecutionException);
                }
            }

            return toolOutputModels;
        }

        /// <summary>
        /// Get successful outputs results
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<OutputModel>> GetSuccessfulAsync(
            this Task<IEnumerable<OutputModel>> tasks)
        {
            return (await tasks)
                .AsParallel()
                .Where(f => f.Successful);
        }
    }
}