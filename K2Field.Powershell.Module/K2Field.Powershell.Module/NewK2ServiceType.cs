using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using SourceCode.Hosting.Server.Interfaces;
using SourceCode.SmartObjects.Services.Management;

namespace K2Field.Powershell.Module
{
    [Cmdlet(VerbsCommon.New, "K2ServiceType")]
    public class NewK2ServiceType: Cmdlet
    {
        private readonly ServiceManagementServer _managementServer = new ServiceManagementServer();
        private Guid _serviceTypeGuid;

        [Parameter(Mandatory = true, Position = 1,
            HelpMessage = "Assembly name with extension.")]
        [Alias("Assembly")]
        public string ServiceTypeAssembly { get; set; }
        [Parameter(Mandatory = true, Position = 2,
            HelpMessage = "System Name of the assembly.")]
        public string SystemName { get; set; }

        [Parameter(Mandatory = false, Position = 3,
            HelpMessage = "Display Name of the Service Type.")]
        public string DisplayName { get; set; }
        [Parameter(Mandatory = false, Position = 4,
            HelpMessage = "Guid of the service type.")]
        [Alias("Guid")]
        public string ServiceTypeGuid { get; set; }
        [Parameter(Mandatory = false, Position = 5,
            HelpMessage = "Description for the Service Type.")]
        public string Description { get; set; }


        protected override void BeginProcessing()
        {
            _serviceTypeGuid = string.IsNullOrEmpty(ServiceTypeGuid) ? Guid.NewGuid() : Guid.Parse(ServiceTypeGuid);
            DisplayName = string.IsNullOrEmpty(DisplayName) ? SystemName : DisplayName;
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
                var dllPath = ModuleHelper.InstallDir + "ServiceBroker\\" + ServiceTypeAssembly;
                var brokerClass = _managementServer.GetRegisterableServices()
                    .Where(service => service.Value == dllPath)
                    .Select(p => p.Key).FirstOrDefault();

                if (_managementServer.GetServiceType(_serviceTypeGuid) != null)
                {
                    WriteVerbose($"Updating existing Service Type {SystemName}");
                    _managementServer.UpdateServiceType(_serviceTypeGuid, SystemName, DisplayName, Description, dllPath,
                        brokerClass);
                }
                else
                {
                    WriteVerbose("Registering the following Service Type:");
                    WriteVerbose($"System Name - {SystemName}");
                    WriteVerbose($"Display Name - {DisplayName}");
                    WriteVerbose($"Guid - {_serviceTypeGuid}");
                    _managementServer.RegisterServiceType(_serviceTypeGuid, SystemName, DisplayName, Description,
                        brokerClass);
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
