namespace Falcon.Tools.Interfaces
{
    public interface ITool
    {
        /// <summary>
        /// Name of tool
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Tags for tool like 'WordPress, Joomla, PHP, Java, SQL etc'
        /// </summary>
        string CommonTags { get; set; }

        /// <summary>
        /// Tags for host like 'Nginx, IIS, Kestrel'
        /// </summary>
        string HostTags { get; set; }

        /// <summary>
        /// Info about tool, refs, helpers erc
        /// </summary>
        string Info { get; set; }

        /// <summary>
        /// Command line with place holders like 'nmap -v -A {0}'
        /// where {0} is target
        /// </summary>
        string CommandLine { get; set; }

        /// <summary>
        /// Map target to command line
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        string MakeCommandLine(string target);
    }
}