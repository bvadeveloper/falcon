using System.Diagnostics;
using Falcon.Tools.Interfaces;

namespace Falcon.Tools
{
    public class ToolRunner : IToolRunner
    {
        private string Output { get; set; }
        
        private string ErrorOutput { get; set; }

        public string GetOutput() => Output;
        
        public string GetErrorOutput() => ErrorOutput;

        public IToolRunner Run(string command)
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

                Output += process.StandardOutput.ReadToEnd();
                ErrorOutput += process.StandardError.ReadToEnd(); // todo: ??

                process.WaitForExit();
            }

            return this;
        }
    }
}