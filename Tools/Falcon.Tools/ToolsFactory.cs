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

        private readonly ToolType _type;
        private List<string> _optionalTools;
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
        public IToolsModel InitTools()
        {
            switch (_type)
            {
                case ToolType.Scan:
                    return _scanTools.Value.UseOnlyTools(_optionalTools);
                case ToolType.Collect:
                    return _collectTools.Value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Use optional tools from request (by concept only for scanners)
        /// </summary>
        /// <param name="tools"></param>
        /// <returns></returns>
        public ToolsFactory UseOptionalTools(List<string> tools)
        {
            _optionalTools = tools;
            return this;
        }

        /// <summary>
        /// Map tools by tags (by concept only for scanners)
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public ToolsFactory MapToolsByTags(Dictionary<TagType, string> tags)
        {
            if (_optionalTools != null && _optionalTools.Any())
            {
                return this;
            }

            if (tags.Any())
            {
                var mappedTools = new List<string>();

                foreach (var (_, value) in tags)
                {
                    var mappedTool = _scanTools.Value.Toolset.FirstOrDefault(t => t.FrameworkTags.Contains(value)
                                                                                  || t.ServerTags.Contains(value)
                                                                                  || t.CommonTags.Contains(value))
                        ?.Name;
                    if (!string.IsNullOrWhiteSpace(mappedTool))
                    {
                        mappedTools.Add(mappedTool);
                    }
                }

                _optionalTools = mappedTools;
            }
            else
            {
                _optionalTools = new List<string> { "nmap" };
            }

            return this;
        }
    }

    public static class ToolsFactoryExtensions
    {
        public static IEnumerable<string> ToolNames(this IToolsModel model)
        {
            return model.Toolset.Select(n => n.Name);
        }
    }
}