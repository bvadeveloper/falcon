using Autofac;
using Falcon.Tools.Interfaces;
using Falcon.Utils.Autofac;

namespace Falcon.Tools.Module
{
    public class ToolModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Tools>().As<ICollectTools>();
            builder.RegisterType<Tools>().As<IScanTools>();
            builder.RegisterModelAsInterface<Tools, ICollectTools>("CollectTools");
            builder.RegisterModelAsInterface<Tools, IScanTools>("ScanTools");
        }
    }
}