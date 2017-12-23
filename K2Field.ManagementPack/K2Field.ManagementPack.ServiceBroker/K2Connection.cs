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
        private readonly object _padlock = new object();
        private readonly string _sessionConnectionString;
        private readonly ISessionManager _sessionManager;
        private BaseAPIConnection _connection;

        public string SessionConnectionString => _sessionConnectionString;
        public ISessionManager SessionManager => _sessionManager;
        
        public string UserName { get; set; }

        public K2Connection(IServiceMarshalling serviceMarshalling, IServerMarshaling serverMarshaling)
        {
            _sessionManager = serverMarshaling.GetSessionManagerContext();
            var sessionCookie = SessionManager.CurrentSessionCookie;
            _sessionConnectionString = serverMarshaling.GetSecurityManagerContext().GetSessionConnectionString(sessionCookie);
        }

        public BaseAPIConnection GetConnection()
        {
            if (_connection == null)
            {
                lock (_padlock)
                {
                    if (_connection == null)
                    {
                        var server = new BaseAPI();
                        server.CreateConnection();
                        server.Connection.Open(_sessionConnectionString);
                        _connection = server.Connection;
                    }
                }
            }
            return _connection;
        }
    }
}
