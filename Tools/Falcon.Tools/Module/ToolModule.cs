using Autofac;
using Falcon.Tools.Interfaces;
using Falcon.Utils.Autofac;

namespace Falcon.Tools.Module
{
    public class ToolModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ToolsModel>().As<ICollectToolsModel>().As<IScanToolsModel>().SingleInstance();
            builder.RegisterModelAsInterface<ToolsModel, ICollectToolsModel>("Collect");
            builder.RegisterModelAsInterface<ToolsModel, IScanToolsModel>("Scan");
            builder.RegisterType<ToolsHolder>();
        }
    }
}