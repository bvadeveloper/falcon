using System;
using System.Collections.Generic;
using System.Linq;
using Falcon.Tools.Interfaces;

namespace Falcon.Tools
{
    /// <summary>
    /// Hold tools and prepares tools for using
    /// </summary>
    public class ToolsHolder
    {
        /// <summary>
        /// Tools holder factory for autofac injections
        /// https://autofaccn.readthedocs.io/en/latest/advanced/delegate-factories.html
        /// </summary>
        /// <param name="toolType"></param>
        public delegate ToolsHolder Factory(ToolType toolType);

        private readonly ToolType _toolType;
        internal List<string> OptionalTools;
        private readonly Lazy<ICollectToolsModel> _collectTools;
        private readonly Lazy<IScanToolsModel> _scanTools;

        public ToolsHolder(
            ToolType toolType,
            Lazy<ICollectToolsModel> collectTools,
            Lazy<IScanToolsModel> scanTools)
        {
            _toolType = toolType;
            _collectTools = collectTools;
            _scanTools = scanTools;
        }

        /// <summary>
        /// Make tools
        /// </summary>
        /// <returns></returns>
        public IToolsModel MakeTools()
        {
            switch (_toolType)
            {
                case ToolType.Scan:
                    return _scanTools.Value.MapOptionalTools(OptionalTools);
                case ToolType.Collect:
                    return _collectTools.Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public static class ToolsHolderExtensions
    {
        /// <summary>
        /// Set optional tools from request (ONLY FOR SCANNERS)
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="tools"></param>
        /// <returns></returns>
        public static ToolsHolder UseOptionalTools(this ToolsHolder holder, List<string> tools)
        {
            holder.OptionalTools = tools;
            return holder;
        }

        public static IEnumerable<string> GetScannersName(this ToolsHolder holder, Lazy<IScanToolsModel> scanTools)
        {
            return scanTools.Value.Toolset.Select(n => n.Name);
        }
    }
}