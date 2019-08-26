using System.Threading.Tasks;
using Autofac;
using Falcon.Reports;

namespace Falcon.Hosts.Report
{
    internal static class Program
    {
        private static Task Main() => Host.InitBasic(builder => builder.RegisterType<ReportService>().As<IReportService>());
    }
}