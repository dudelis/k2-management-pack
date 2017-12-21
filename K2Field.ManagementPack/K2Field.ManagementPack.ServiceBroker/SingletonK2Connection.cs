using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.Hosting.Server.Interfaces;

namespace K2Field.ManagementPack.ServiceBroker
{
    internal class SingletonK2Connection
    {
        private IServiceMarshalling iServiceMarshalling;
        private IServerMarshaling iServerMarshaling;
        private BaseAPIConnection _connection;
        public string SessionConnectionString { get; set; }
        public ISessionManager SessionManager { get; set; }
        public BaseAPIConnection Connection => _connection;

        private SingletonK2Connection()
        {
            var sessionCookie = SessionManager.CurrentSessionCookie;
            SessionConnectionString = ServiceBroker.SecurityManager.GetSessionConnectionString(sessionCookie);
            
        }
        public SingletonK2Connection Create (IServerMarshaling sm)
        {
            iServerMarshaling = sm;
            return Instance;
        }

        private static readonly Lazy<SingletonK2Connection> instance =
            new Lazy<SingletonK2Connection>(() => new SingletonK2Connection());

        public static SingletonK2Connection Instance => instance.Value;

    }
}
