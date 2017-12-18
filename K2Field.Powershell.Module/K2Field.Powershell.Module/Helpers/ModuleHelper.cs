using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Xml;
using Microsoft.Win32;
using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.Workflow.Client;

namespace K2Field.Powershell.Module
{
    internal class ModuleHelper: Cmdlet
    {
        private static readonly string InstallDir = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\SourceCode\\blackpearl\\blackpearl Core\\", "InstallDir", (object)null).ToString();
        private static readonly string HostConfig = InstallDir + "Host Server\\Bin\\K2HostServer.exe.config";

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
    }
}
