using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using SourceCode.Workflow.Client;

namespace K2Field.Powershell.Module
{
    [Cmdlet(VerbsCommon.Set, "DataField")]
    public class SetDataField : PSCmdlet
    {
        private readonly Connection _connection = new Connection();
        [Alias("p", "pid")]
        [Parameter(Mandatory = true, Position = 1)]
        public int ProcInstId { get; set; }

        [Alias("df")]
        [Parameter(Mandatory = true, Position = 2)]
        public string DataFieldName { get; set; }

        [Alias("v")]
        [Parameter(Mandatory = true, Position = 3)]
        public object DataFieldValue { get; set; }
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
                var dataFields = procInst.DataFields;
                dataFields[DataFieldName].Value = DataFieldValue;
                procInst.Update();
            }
            catch (Exception ex)
            {
                ErrorHelper.Write(ex);
            }
        }
        protected override void EndProcessing()
        {
            _connection.Close();
        }
        protected override void StopProcessing()
        {
            _connection.Close();
        }
    }
}
