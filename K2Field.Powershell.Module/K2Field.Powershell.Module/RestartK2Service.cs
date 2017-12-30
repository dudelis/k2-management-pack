using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.ServiceProcess;
using System.Text;

namespace K2Field.Powershell.Module
{
    [Cmdlet("Restart", "K2Service")]
    public class RestartK2Service : Cmdlet
    {
        protected override void ProcessRecord()
        {
            ModuleHelper.RestartK2Service();
        }

    }
}
