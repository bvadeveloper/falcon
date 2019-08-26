using System.Collections.Generic;
using System.Linq;
using Falcon.Logging;

namespace Falcon.Tools
{
    public static class OutputExtension
    {
        public static void LogOutputs(this IJsonLogger logger, IEnumerable<OutputModel> models)
        {
            foreach (var model in models)
            {
                if (model.Successful)
                {
                    logger.Trace("Successful tool processing", model);
                }
                else
                {
                    logger.Error("Tool failure", new
                        {
                            model.ToolName, model.Output, model.ErrorOutput
                        },
                        model.ExecutionException);
                }
            }
        }

        /// <summary>
        /// Get successful outputs results
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static IEnumerable<OutputModel> SelectSuccessful(
            this IEnumerable<OutputModel> models) =>
            models
                .AsParallel()
                .Where(f => f.Successful);
    }
}