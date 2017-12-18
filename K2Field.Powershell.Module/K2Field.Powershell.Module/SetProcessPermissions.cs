using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using SourceCode.Workflow.Management;

namespace K2Field.Powershell.Module
{
    [Cmdlet("Set", "ProcessPermissions")]
    public class SetProcessPermissions : Cmdlet
    {
        private WorkflowManagementServer wfMan = new WorkflowManagementServer();

        [Parameter(Mandatory = true, Position = 1, ValueFromPipelineByPropertyName = true)]
        public int ProcSetId { get; set; }

        [Parameter(Mandatory = true, Position = 2)]
        [ValidateSet(new string[] { "Group", "User" })]
        public string IdentityType { get; set; }

        [Parameter(Mandatory = true, Position = 3)]
        public string FQN { get; set; }

        [Parameter(Position = 4)]
        public SwitchParameter Admin { get; set; }

        [Parameter(Position = 5)]
        public SwitchParameter Start { get; set; }

        [Parameter(Position = 6)]
        public SwitchParameter View { get; set; }

        [Parameter(Position = 7)]
        public SwitchParameter ViewParticipate { get; set; }

        [Parameter( Position = 8)]
        public SwitchParameter ServerEvent { get; set; }
        


        protected override void BeginProcessing()
        {
            try
            {
                wfMan.CreateConnection();
                wfMan.Connection.Open(ModuleHelper.BuildConnectionString());
            }
            catch (Exception ex)
            {
                ErrorHelper.Throw(ex);
            }
        }

        protected override void ProcessRecord()
        {
            try
            {
                var procSetPermissions = new ProcSetPermissions()
                {
                    Admin = Admin,
                    Start = Start,
                    View = View,
                    ViewPart = ViewParticipate,
                    ServerEvent = ServerEvent
                };
                var permissions = new Permissions();
                if (this.IdentityType.ToLower() == "user")
                {
                    procSetPermissions.UserName = FQN;
                    permissions.Add(procSetPermissions);
                    if (!procSetPermissions.Admin && !procSetPermissions.Start && (!procSetPermissions.View && !procSetPermissions.ViewPart) && !procSetPermissions.ServerEvent)
                        this.wfMan.UpdateProcPermissions(this.ProcSetId, permissions, true);
                    else
                        this.wfMan.UpdateOrAddProcUserPermissions(this.ProcSetId, permissions);
                }
                else
                {
                    procSetPermissions.GroupName = FQN;
                    permissions.Add(procSetPermissions);
                    if (!procSetPermissions.Admin && !procSetPermissions.Start && (!procSetPermissions.View && !procSetPermissions.ViewPart) && !procSetPermissions.ServerEvent)
                        this.wfMan.UpdateProcPermissions(this.ProcSetId, permissions, true);
                    else
                        this.wfMan.UpdateProcGroupPermissions(this.ProcSetId, permissions);
                }
            }
            catch (Exception ex)
            {
                ErrorHelper.Throw(ex);
            }
        }

        protected override void EndProcessing()
        {
            this.wfMan.Connection.Close();
        }

        protected override void StopProcessing()
        {
            this.wfMan.Connection.Close();
        }
    }
}
