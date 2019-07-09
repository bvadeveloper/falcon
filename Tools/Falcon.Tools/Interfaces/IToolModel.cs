namespace Falcon.Tools.Interfaces
{
    public interface IToolModel
    {
        /// <summary>
        /// Name of tool like 'nmap, sqlmap, etc'
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Tags for tool like 'WordPress, Joomla, PHP, Java, SQL, etc'
        /// </summary>
        string FrameworkTags { get; set; }
        
        string CommonTags { get; set; }

        /// <summary>
        /// Tags for host like 'Nginx, IIS, Kestrel, etc'
        /// </summary>
        string ServerTags { get; set; }

        /// <summary>
        /// Info about tool, refs, helpers, etc
        /// </summary>
        string Info { get; set; }

        /// <summary>
        /// Command line with place holders like 'nmap -v -A {0}' where {0} is target
        /// </summary>
        string CommandLine { get; set; }

        /// <summary>
        /// Execution timeout for tool
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Map target to command line
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        string MakeCommandLine(string target);
    }
}