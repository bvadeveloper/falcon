using System;
using System.Collections.Generic;
using System.Linq;
using Falcon.Profiles;
using Falcon.Tools.Interfaces;

namespace Falcon.Tools.Filters
{
    public class OutputFilter
    {
        /// <summary>
        /// Attribute filter factory for autofac injections
        /// https://autofaccn.readthedocs.io/en/latest/advanced/delegate-factories.html
        /// </summary>
        public delegate OutputFilter Factory(ToolType toolType);

        private readonly ToolType _toolType;

        private readonly Lazy<IEnumerable<IToolOutputFilter>> _outputFilters;

        public OutputFilter(
            ToolType toolType,
            Lazy<IEnumerable<IToolOutputFilter>> outputFilters)
        {
            _toolType = toolType;
            _outputFilters = outputFilters;
        }

        public IEnumerable<IToolOutputFilter> MakeFilters()
        {
            return _outputFilters.Value.Where(f => f.ToolType == _toolType);
        }
    }

    public static class OutputFilerExtensions
    {
        public static TargetAttributes FillAttributes(this IEnumerable<IToolOutputFilter> filters,
            IEnumerable<ToolOutputModel> outputs)
        {
            return new TargetAttributes { };
        }
    }
}