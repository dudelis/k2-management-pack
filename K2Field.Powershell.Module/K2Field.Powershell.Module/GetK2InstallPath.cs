using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;

namespace K2Field.Powershell.Module
{
    [Cmdlet(VerbsCommon.Get, "K2InstallPath")]
    public class GetK2InstallPath :Cmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(ModuleHelper.InstallDir);
        }
    }
}
