using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceProcess;
using System.Text;
using System.Xml;
using Microsoft.Win32;
using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.Workflow.Client;

namespace K2Field.Powershell.Module
{
    internal class ModuleHelper: Cmdlet
    {
        public static readonly string InstallDir = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\SourceCode\\blackpearl\\blackpearl Core\\", "InstallDir", (object)null).ToString();
        private static readonly string HostConfig = InstallDir + "Host Server\\Bin\\K2HostServer.exe.config";
        public static string K2Version = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\SourceCode\\installer\\", "Version", (object)null).ToString();
        public static int majorVersion = K2Version[0];

        internal static string BuildConnectionString()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(HostConfig);
            uint uint16 = (uint)Convert.ToUInt16(xmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='port']").Attributes["value"].Value);
            return new SCConnectionStringBuilder()
            {
                Integrated = true,
                IsPrimaryLogin = true,
                Authenticate = true,
                EncryptedPassword = false,
                Host = "localhost",
                Port = uint16
            }.ToString();
        }
        internal static void RestartK2Service()
        {
            ServiceController serviceController = new ServiceController();
            serviceController.ServiceName = ModuleHelper.majorVersion != 52 ? "K2 Server" : "K2 blackpearl Server";
            try
            {
                var tickCount = Environment.TickCount;
                var timeout1 = TimeSpan.FromMilliseconds(120000.0);
                serviceController.Stop();
                serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout1);
                var timeout2 = TimeSpan.FromMilliseconds((double)(120000 - (Environment.TickCount - tickCount)));
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout2);
            }
            catch (Exception ex)
            {
                ErrorHelper.Write(ex);
            }
        }
    }
}
