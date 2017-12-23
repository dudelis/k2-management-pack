using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.Hosting.Server.Interfaces;
using SourceCode.Workflow.Client;

namespace K2Field.ManagementPack.ServiceBroker
{
    internal class K2Connection
    {
        private readonly object _baseapilock = new object();
        private readonly object _wfconnectionlock = new object();
        private readonly string _sessionConnectionString;
        private readonly ISessionManager _sessionManager;
        private BaseAPIConnection _connection;
        private Connection _workflowClientConnection;
        private readonly string _workflowConnectionString;
        private readonly int _workflowConnectionPort;

        public string SessionConnectionString => _sessionConnectionString;
        public ISessionManager SessionManager => _sessionManager;
        
        public string UserName { get; set; }

        public K2Connection(IServiceMarshalling serviceMarshalling, IServerMarshaling serverMarshaling)
        {
            if (string.IsNullOrEmpty(_workflowConnectionString) || _workflowConnectionPort == 0)
            {
                _workflowConnectionString = ConfigurationManager.ConnectionStrings["WorkflowServer"].ConnectionString;
            }
            _sessionManager = serverMarshaling.GetSessionManagerContext();
            var sessionCookie = SessionManager.CurrentSessionCookie;
            _sessionConnectionString = serverMarshaling.GetSecurityManagerContext().GetSessionConnectionString(sessionCookie);
        }

        public BaseAPIConnection GetConnection()
        {
            if (_connection == null)
            {
                lock (_baseapilock)
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
        public Connection GetWorkflowClientConnection()
        {
            if (_workflowClientConnection == null)
            {
                lock (_wfconnectionlock)
                {
                    if (_workflowClientConnection == null)
                    {
                        _workflowClientConnection = new Connection();
                        try
                        {
                            var connectionSetup = new ConnectionSetup();
                            connectionSetup.ParseConnectionString(_workflowConnectionString);
                            _workflowClientConnection.Open(connectionSetup);
                        }
                        catch (Exception ex)
                        {
                            if (_workflowClientConnection != null)
                            {
                                _workflowClientConnection.Dispose();
                            }

                            throw new Exception("Failed to create Connection to K2.", ex);
                        }
                    }
                }
            }
            return _workflowClientConnection;
        }

        public void CloseConnections()
        {
            _connection?.Close();
            _workflowClientConnection?.Close();
        }
    }
}
