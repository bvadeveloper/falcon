using Autofac;
using Falcon.Tools.Interfaces;
using Falcon.Utils.Autofac;

namespace Falcon.Tools.Module
{
    public class ToolModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Tools>().As<ICollectToolsModel>().As<IScanToolsModel>().SingleInstance();
            builder.RegisterModelAsInterface<Tools, ICollectToolsModel>("Collect");
            builder.RegisterModelAsInterface<Tools, IScanToolsModel>("Scan");
            builder.RegisterType<ToolsHolder>();
        }
    }
}