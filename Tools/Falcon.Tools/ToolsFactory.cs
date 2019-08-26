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

        public IToolsModel UseTools() => _toolsModel;

        /// <summary>
        /// Use optional tools from request
        /// </summary>
        /// <param name="optionalTools"></param>
        /// <returns></returns>
        public IToolsModel UseOptionalTools(List<string> optionalTools)
        {
            if (optionalTools != default && optionalTools.Any())
            {
                return _toolsModel.UseOnly(optionalTools);
            }

            return default;
        }

        /// <summary>
        /// Use tools by target tags
        /// </summary>
        /// <param name="profileTags"></param>
        /// <returns></returns>
        public IToolsModel UseToolsByTags(Dictionary<TagType, string> profileTags = default)
        {
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

                return _toolsModel.UseOnly(mappedTools);
            }

            return _toolsModel;
        }
    }
}