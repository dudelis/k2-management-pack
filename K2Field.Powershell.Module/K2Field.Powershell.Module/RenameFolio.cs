using System;
using System.Management.Automation;
using SourceCode.Workflow.Client;

namespace K2Field.Powershell.Module
{
    [Cmdlet(VerbsCommon.Rename, "Folio")]
    public class RenameFolio : PSCmdlet
    {
        private readonly Connection _connection = new Connection();
        [Parameter(Mandatory = true, Position = 1)]
        public int ProcInstId { get; set; }
        [Parameter(Mandatory = true, Position = 2)]
        public string Folio { get; set; }
        protected override void BeginProcessing()
        {
            try
            {
                _connection.Open("localhost");
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
                var procInst = _connection.OpenProcessInstance(ProcInstId);
                procInst.Folio = Folio;
                procInst.Update();
            }
            catch (Exception ex)
            {
                ErrorHelper.Write(ex);
            }
        }
        protected override void EndProcessing()
        {
            _connection?.Close();
        }
        protected override void StopProcessing()
        {
            _connection?.Close();
        }
    }
}
