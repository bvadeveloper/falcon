using Autofac;
using Falcon.Tools.Interfaces;
using Falcon.Utils.Autofac;

namespace Falcon.Tools.Module
{
    public class ToolModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ToolsRunner>().As<ICollectToolsModel>().As<IScanToolsModel>().SingleInstance();
            builder.RegisterModelAsInterface<ToolsRunner, ICollectToolsModel>("Collect");
            builder.RegisterModelAsInterface<ToolsRunner, IScanToolsModel>("Scan");
            builder.RegisterType<TagFactory>();
            builder.RegisterType<ToolsFactory>();
        }
    }
}