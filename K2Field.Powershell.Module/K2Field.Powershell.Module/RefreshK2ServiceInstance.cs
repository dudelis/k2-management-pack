using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using SourceCode.SmartObjects.Management;
using SourceCode.SmartObjects.Services.Management;

namespace K2Field.Powershell.Module
{
    [Cmdlet("Refresh", "K2ServiceInstance")]
    public class RefreshK2ServiceInstance : PSCmdlet
    {
        private readonly ServiceManagementServer _managementServer = new ServiceManagementServer();
        private readonly SmartObjectManagementServer _smoManagementServer = new SmartObjectManagementServer();
        private Guid _serviceTypeGuid;

        [Parameter(Position = 1,
            HelpMessage = "Guid of the service instance to refresh.")]
        [Alias("siGuid")]
        public string ServiceInstanceGuid { get; set; }

        [Parameter(Position = 2,
            HelpMessage = "Name of the service instance to refresh.")]
        [Alias("siName")]
        public string ServiceInstanceName{ get; set; }

        [Parameter(Mandatory = false, Position = 3,
            HelpMessage = "Guid of the service type.")]
        [Alias("stGuid")]
        public string ServiceTypeGuid { get; set; }
        

        protected override void BeginProcessing()
        {
            if (string.IsNullOrEmpty(ServiceInstanceGuid) && (string.IsNullOrEmpty(ServiceInstanceName) || string.IsNullOrEmpty(ServiceTypeGuid)))
            {
                throw new Exception("You need to provide either Service Instance Guid OR Service Instance System name with Service type Guid");
            }
            try
            {
                _managementServer.CreateConnection();
                _managementServer.Connection.Open(ModuleHelper.BuildConnectionString());
            }
            catch (Exception ex)
            {
                ErrorHelper.Write(ex);
            }
        }

        protected override void ProcessRecord()
        {
            try
            {
                if (!string.IsNullOrEmpty(ServiceInstanceGuid))
                {
                    var sInstanceGuid = Guid.Parse(ServiceInstanceGuid);
                    _managementServer.RefreshServiceInstance(sInstanceGuid);
                }
                else
                {
                    var sTypeGuid = Guid.Parse(ServiceTypeGuid);
                    _smoManagementServer.Connection = _managementServer.Connection;
                    var sInstanceGuid = _smoManagementServer.GetServiceInstanceGuid(sTypeGuid, ServiceInstanceName);
                    _managementServer.RefreshServiceInstance(sInstanceGuid);
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.Write(ex);
            }
        }

        protected override void EndProcessing()
        {
            _managementServer.Connection?.Close();
        }

        protected override void StopProcessing()
        {
            _managementServer.Connection?.Close();
        }
    }
}
