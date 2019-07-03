using System.Diagnostics;

namespace Falcon.Tools
{
    public interface IToolProcess
    {
        IToolProcess Run(string command);

        string MakeReport();
    }

    public class ToolProcess : IToolProcess
    {
        private string Report { get; set; }

        public string MakeReport()
        {
            return Report;
        }

        public IToolProcess Run(string command)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command.Replace("\"", "\\\"")}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                process.Start();

                Report += process.StandardOutput.ReadToEnd();
                Report += process.StandardError.ReadToEnd();

                process.WaitForExit();
            }

            return this;
        }
    }
}