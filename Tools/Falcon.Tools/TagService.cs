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
        public delegate TagFactory Factory(TagType tag);

        public TagFactory(TagType tag)
        {
            switch (tag)
            {
                case TagType.Framework:
                    _tagKeyWords = new List<string> { "joomla", "wordpress" };
                    break;
                case TagType.WebServer:
                    _tagKeyWords = new List<string> { "nginx", "iis", "kestrel", "tomcat", "apache" };
                    break;
                case TagType.Database:
                    _tagKeyWords = new List<string> { "mysql", "mssql", "postgres" };
                    break;
                case TagType.Ports:
                    _tagKeyWords = new List<string> { "80", "443", "15672", "5672", "6379", "3306" };
                    break;
                case TagType.Server:
                    _tagKeyWords = new List<string> { "linux", "windows", "ubuntu", "fedora", "redhat", "centos", "redhat" };
                    break;
                case TagType.NotAvailable:
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
        public static Dictionary<TagType, string> FindTags(this TagFactory.Factory tagFactory,
            IEnumerable<ReportModel> outputs)
        {
            var output = outputs.Aggregate("", (c, m) => $"{c} {m.Output}");
            var tags = new Dictionary<TagType, string>();

            foreach (var tagType in (TagType[]) Enum.GetValues(typeof(TagType)))
            {
                var tag = tagFactory(tagType).Find(output);
                if (tag == null) continue;
                tags.Add(tagType, tag);
            }

            return tags;
        }
    }
}