using System.Collections.Generic;

namespace Falcon.Tools.Interfaces
{
    public interface IToolModel
    {
        /// <summary>
        /// nmap, sqlmap, etc
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Command line with place holders like 'nmap -v -A {0}' where {0} is target
        /// </summary>
        string CommandLine { get; set; }
        
        string VersionCommandLine { get; set; }

        /// <summary>
        /// Execution timeout
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Map target to command line
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        string MakeCommandLine(string target);
        
        /// <summary>
        /// wordpress, joomla, etc
        /// </summary>
        List<string> FrameworkTags { get; set; }

        /// <summary>
        /// ftp, ssh, email, vnc, etc
        /// </summary>
        List<string> ServiceTags { get; set; }

        /// <summary>
        /// nginx, apache, iis, etc
        /// </summary>
        List<string> HostTags { get; set; }

        /// <summary>
        /// 80, 443, 8080, etc
        /// </summary>
        List<string> PortTags { get; set; }
    }
}