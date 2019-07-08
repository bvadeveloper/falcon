using System.Collections.Generic;
using System.Linq;
using Falcon.Logging;

namespace Falcon.Tools
{
    public static class OutputExtension
    {
        public static void LogOutputs(
            this IJsonLogger logger, IEnumerable<OutputModel> models)
        {
            var outputModels = models.ToList();

            foreach (var model in outputModels)
            {
                if (model.Successful)
                {
                    logger.Trace("Successful tool processing", model);
                }
                else
                {
                    logger.Error("Tool failure", new { model.Output, model.ErrorOutput }, model.ExecutionException);
                }
            }
        }

        /// <summary>
        /// Get successful outputs results
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static List<OutputModel> GetSuccessful(
            this IEnumerable<OutputModel> models)
        {
            return models
                .AsParallel()
                .Where(f => f.Successful)
                .ToList();
        }
    }
}