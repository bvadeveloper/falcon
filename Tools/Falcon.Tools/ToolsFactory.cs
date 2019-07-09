using System;
using System.Collections.Generic;
using System.Linq;
using Falcon.Tools.Interfaces;

namespace Falcon.Tools
{
    /// <summary>
    /// Hold tools and prepares tools for using
    /// </summary>
    public class ToolsFactory
    {
        /// <summary>
        /// Tools holder factory for autofac injections
        /// https://autofaccn.readthedocs.io/en/latest/advanced/delegate-factories.html
        /// </summary>
        /// <param name="type"></param>
        public delegate ToolsFactory Factory(ToolType type);

        private readonly ToolType _type;
        internal List<string> OptionalTools;
        private readonly Lazy<ICollectToolsModel> _collectTools;
        private readonly Lazy<IScanToolsModel> _scanTools;

        public ToolsFactory(
            ToolType type,
            Lazy<ICollectToolsModel> collectTools,
            Lazy<IScanToolsModel> scanTools)
        {
            _type = type;
            _collectTools = collectTools;
            _scanTools = scanTools;
        }

        /// <summary>
        /// Make tools
        /// </summary>
        /// <returns></returns>
        public IToolsModel MakeTools()
        {
            switch (_type)
            {
                case ToolType.Scan:
                    return _scanTools.Value.UseOnlyTools(OptionalTools);
                case ToolType.Collect:
                    return _collectTools.Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public static class ToolsFactoryExtensions
    {
        /// <summary>
        /// Use optional tools from request (by concept only for scanners)
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="tools"></param>
        /// <returns></returns>
        public static ToolsFactory UseOptionalTools(this ToolsFactory holder, List<string> tools)
        {
            holder.OptionalTools = tools;
            return holder;
        }

        public static IEnumerable<string> ToolNames(this IToolsModel toolsModel)
        {
            return toolsModel.Toolset.Select(n => n.Name);
        }
    }
}