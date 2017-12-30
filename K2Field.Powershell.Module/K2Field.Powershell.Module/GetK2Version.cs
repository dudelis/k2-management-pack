using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;

namespace K2Field.Powershell.Module
{
    [Cmdlet(VerbsCommon.Get, "K2Version")]
    public class GetK2Version :Cmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(ModuleHelper.K2Version);
        }
    }
}
