using System;

namespace Falcon.Profiles
{
    public interface ISession
    {
        IContext Context { get; set; }
    }

    public interface IContext
    {
        Guid SessionId { get; set; }

        string ClientName { get; set; }
    }

    public interface IApiContext : IContext { }

    public class ApiContext : IApiContext
    {
        public Guid SessionId { get; set; }

        public string ClientName { get; set; }
    }

    public interface IMessengerContext : IContext
    {
        long ChatId { get; set; }
    }

    public class MessengerContext : IMessengerContext
    {
        public long ChatId { get; set; }

        public Guid SessionId { get; set; }

        public string ClientName { get; set; }
    }
}