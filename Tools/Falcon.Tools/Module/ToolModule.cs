using Autofac;
using Falcon.Tools.Interfaces;
using Falcon.Utils.Autofac;

namespace Falcon.Tools.Module
{
    public class ToolModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModelAsInterface<Tools, ICollectTools>("CollectTool");
            builder.RegisterModelAsInterface<Tools, IScanTools>("ScanTool");
        }
    }
}