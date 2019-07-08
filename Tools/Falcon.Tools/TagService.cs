using System;
using System.Collections.Generic;
using System.Linq;
using Falcon.Profiles;

namespace Falcon.Tools
{
    public class TagFactory
    {
        private readonly List<string> _tagKeyWords;

        /// <summary>
        /// Tools holder factory for autofac injections
        /// https://autofaccn.readthedocs.io/en/latest/advanced/delegate-factories.html
        /// </summary>
        /// <param name="tag"></param>
        public delegate TagFactory Factory(TargetTag tag);

        public TagFactory(TargetTag tag)
        {
            switch (tag)
            {
                case TargetTag.Framework:
                    _tagKeyWords = new List<string> { "joomla, wordpress" };
                    break;
                case TargetTag.WebServer:
                    _tagKeyWords = new List<string> { "nginx, iis, kestrel, tomkat" };
                    break;
                case TargetTag.Database:
                    _tagKeyWords = new List<string> { "mysql, mssql, postgres" };
                    break;
                case TargetTag.Ports:
                    _tagKeyWords = new List<string> { "80, 443, 15672, 5672, 6379" };
                    break;
                case TargetTag.Server:
                    _tagKeyWords = new List<string> { "linux, windows, ubuntu, fedora, redhat. centos" };
                    break;
                case TargetTag.NotAvailable:
                    _tagKeyWords = new List<string> { "Nmap done: 0 IP addresses (0 hosts up)" };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tag), tag, null);
            }
        }

        public string Find(string text)
        {
            foreach (var keyWord in _tagKeyWords)
            {
                if (text.Contains(keyWord, StringComparison.InvariantCultureIgnoreCase))
                    return keyWord;
            }

            return null;
        }
    }

    public static class TagExtensions
    {
        public static Dictionary<TargetTag, string> FindTags(this TagFactory.Factory tagFactory,
            IEnumerable<ReportModel> outputs)
        {
            var output = outputs.Aggregate("", (c, m) => $"{c} {m.Output}");

            return ((TargetTag[]) Enum.GetValues(typeof(TargetTag)))
                .ToDictionary(tag => tag, tag => tagFactory(tag).Find(output));
        }
    }
}