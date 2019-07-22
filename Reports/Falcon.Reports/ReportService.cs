using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Falcon.Profiles;
using IronPdf;

namespace Falcon.Reports
{
    public class ReportService : IReportService
    {
        public async Task<byte[]> MakeReportAsync(ReportType reportType, string fileName,
            List<ReportModel> reportModels)
        {
            var renderer = new HtmlToPdf();

            // Render an HTML document or snippet as a string
            var rendered = await renderer.RenderHtmlAsPdfAsync("<h1>Hello World</h1>");

            return rendered.SaveAs(Path.Combine(Path.GetTempPath(), fileName)).BinaryData;
        }
    }
}