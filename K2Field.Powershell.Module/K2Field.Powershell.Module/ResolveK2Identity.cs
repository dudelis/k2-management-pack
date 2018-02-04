using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using SourceCode.Security.UserRoleManager.Management;
using SourceCode.Hosting.Server.Interfaces;

namespace K2Field.Powershell.Module
{
    [Cmdlet("Resolve", "K2Identity")]
    public class ResolveK2Identity : Cmdlet
    {
        private readonly UserRoleManager _urm = new UserRoleManager();

        [Parameter(Mandatory = true, Position = 1,
            HelpMessage = "FQN of the identiy to resolve.")]
        public string FQN { get; set; }

        [Parameter(Mandatory = true, Position = 2, HelpMessage = "User/Group/Role - type of the identity")]
        [ValidateSet(new string[] { "User", "Role", "Identity" }, IgnoreCase = true)]
        public string Identity { get; set; }

        [Parameter(Position = 3, HelpMessage = "Defines if the Idenditty Members should be also resolved.")]
        public SwitchParameter ResolveMembers { get; set; }

        [Parameter(Position = 4, HelpMessage = "Defines if the Idenditty Containers should be also resolved.")]
        public SwitchParameter ResolveContainers { get; set; }

        protected override void BeginProcessing()
        {
            try
            {
                _urm.CreateConnection();
                _urm.Connection.Open(ModuleHelper.BuildConnectionString());
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
                var fqnname = new FQName(FQN);
                var iType = new IdentityType();
                Enum.TryParse(Identity, true, out iType);

                _urm.ResolveIdentity(fqnname, iType, IdentitySection.Identity);
                if (ResolveMembers)
                {
                    _urm.ResolveIdentity(fqnname, iType, IdentitySection.Members);
                }
                if (ResolveContainers)
                {
                    _urm.ResolveIdentity(fqnname, iType, IdentitySection.Containers);
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.Write(ex);
            }
        }
        protected override void EndProcessing()
        {
            _urm.Connection?.Close();
        }

        protected override void StopProcessing()
        {
            _urm.Connection?.Close();
        }
    }
}
