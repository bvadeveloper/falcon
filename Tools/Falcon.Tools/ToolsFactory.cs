using System;
using System.Collections.Generic;
using System.Linq;
using Falcon.Profiles;
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

        private readonly IToolsModel _toolsModel;

        public ToolsFactory(
            ToolType type,
            Lazy<ICollectToolsModel> collectTools,
            Lazy<IScanToolsModel> scanTools)
        {
            switch (type)
            {
                case ToolType.Scan:
                    _toolsModel = scanTools.Value;
                    break;
                case ToolType.Collect:
                    _toolsModel = collectTools.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Use optional tools from request
        /// </summary>
        /// <param name="profileTags"></param>
        /// <param name="optionalTools"></param>
        /// <returns></returns>
        public IToolsModel UseTools(List<string> optionalTools, Dictionary<TagType, string> profileTags = default)
        {
            // use optional tools from request
            if (optionalTools != default && optionalTools.Any())
            {
                return _toolsModel.UseOnlyTools(optionalTools);
            }

            // try to map tools from tags
            if (profileTags != default && profileTags.Any())
            {
                var mappedTools = new List<string>();

                foreach (var (_, value) in profileTags)
                {
                    var mappedTool = _toolsModel.Toolset.FirstOrDefault(t => t.FrameworkTags.Contains(value)
                                                                             || t.ServerTags.Contains(value)
                                                                             || t.CommonTags.Contains(value))?.Name;
                    if (!string.IsNullOrWhiteSpace(mappedTool))
                    {
                        mappedTools.Add(mappedTool);
                    }
                }

                return _toolsModel.UseOnlyTools(mappedTools);
            }

            // use default tool
            return _toolsModel.UseOnlyTools(new List<string> { "nmap" }); // for debug
        }
    }
}