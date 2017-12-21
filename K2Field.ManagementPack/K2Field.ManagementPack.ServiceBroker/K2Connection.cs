using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.Hosting.Server.Interfaces;

namespace K2Field.ManagementPack.ServiceBroker
{
    internal class K2Connection
    {
        private readonly string _sessionConnectionString;
        private ISessionManager _sessionManager;
        private readonly Lazy<BaseAPIConnection> _connection;

        public string SessionConnectionString => _sessionConnectionString;
        public ISessionManager SessionManager => _sessionManager;
        public BaseAPIConnection Connection => _connection.Value;

        public string UserName { get; set; }

        public K2Connection(IServiceMarshalling serviceMarshalling, IServerMarshaling serverMarshaling)
        {
            _sessionManager = serverMarshaling.GetSessionManagerContext();
            var sessionCookie = SessionManager.CurrentSessionCookie;
            _sessionConnectionString = serverMarshaling.GetSecurityManagerContext().GetSessionConnectionString(sessionCookie);
            _connection = new Lazy<BaseAPIConnection>(() =>
            {
                var server = new BaseAPI();
                server.CreateConnection();
                server.Connection.Open(_sessionConnectionString);
                return server.Connection;
            });
        }
    }
}
