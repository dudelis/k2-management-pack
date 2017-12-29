using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using K2Field.ManagementPack.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using SourceCode.Workflow.Management.Criteria;
using client = SourceCode.Workflow.Client;
using mng = SourceCode.Workflow.Management;

namespace K2Field.ManagementPack.ServiceBroker.ServiceObjects
{
    public class ProcessInstanceManagement : ServiceObjectBase
    {
        private client.Connection _wfClient = new client.Connection();
        private mng.WorkflowManagementServer _mngServer = new mng.WorkflowManagementServer();
        public ProcessInstanceManagement(ServiceBroker api) : base(api) { }
        public override string ServiceFolder => "ProcessInstanceManagement";
        
        public override List<ServiceObject> DescribeServiceObjects()
        {
            var so = new ServiceObjectBuilder("ProcessInstanceManagement", "Exposes functionality to manage the process instance.", true);
            so.CreateProperty(Constants.SoProperties.ProcessInstance.ProcessFolio,
                    "The folio to use for the process.", SoType.Text)
                .CreateProperty(Constants.SoProperties.ProcessInstance.ProcessName,
                    "The full name of the process.", SoType.Text)
                .CreateProperty(Constants.SoProperties.ProcessInstance.StartSync,
                    "Start the process synchronously or not.", SoType.YesNo)
                .CreateProperty(Constants.SoProperties.ProcessInstance.ProcInstId,
                    "The process instance ID.", SoType.Number)
                .CreateProperty(Constants.SoProperties.ProcessInstance.ProcessVersion,
                    "The version number to start. Leave empty for default.", SoType.Number)
                .CreateProperty(Constants.SoProperties.ProcessInstance.DataFieldName,
                    "The name of the DataField.", SoType.Text)
                .CreateProperty(Constants.SoProperties.ProcessInstance.DataFieldValue,
                    "The value of the DataField.", SoType.Memo)
                .CreateProperty(Constants.SoProperties.ProcessInstance.DataFieldType,
                    "The data type of the DataField.", SoType.Text)
                .CreateProperty(Constants.SoProperties.ProcessInstance.XmlFieldName, "The name of the XML field.",
                    SoType.Text)
                .CreateProperty(Constants.SoProperties.ProcessInstance.XmlFieldValue,
                    "The value of the XML field.", SoType.Memo)
                .CreateProperty(Constants.SoProperties.ProcessInstance.TargetProcVersion, "Target Process Version Id",
                    SoType.Number)
                .CreateProperty(Constants.SoProperties.ProcessInstance.FromActName, "Activity Name to expire",
                    SoType.Text)
                .CreateProperty(Constants.SoProperties.ProcessInstance.ToActName,
                    "Activity, to which the workflow needs to go", SoType.Text)
                .CreateProperty(Constants.SoProperties.ProcessInstance.StartDate,
                    "StartDate of the process instance", SoType.DateTime)
                .CreateProperty(Constants.SoProperties.ProcessInstance.Status,
                    "Status of the worfklow", SoType.Text)
                .CreateProperty(Constants.SoProperties.ProcessInstance.Originator,
                    "Originator of the workflow", SoType.Text)
                .CreateProperty(Constants.SoProperties.ProcessInstance.Version,
                    "Version of the workflow", SoType.Number)
                .CreateProperty(Constants.SoProperties.ProcessInstance.ExecutingVersion,
                    "Executing version of the workflow", SoType.Number)
                .CreateProperty(Constants.SoProperties.ProcessInstance.ProcSetId,
                    "Process Set of the Process Instance", SoType.Number)
                .CreateProperty(Constants.SoProperties.ProcessInstance.ProcId,
                "Process ID of the process Instance", SoType.Number);

            var updateFolio = new ServiceObjectMethodBuilder(Constants.Methods.ProcessInstance.UpdateFolio, "Updates the folio of a running process instance", MethodType.Update)
                .AddProperty(Constants.SoProperties.ProcessInstance.ProcInstId, true, true, false)
                .AddProperty(Constants.SoProperties.ProcessInstance.ProcessFolio, true, true, false);
            so.AddMethod(updateFolio);

            var updateDataField  = new ServiceObjectMethodBuilder(Constants.Methods.ProcessInstance.UpdateDataField,
                    "Updates the DataField of a running process instance", MethodType.Update)
                .AddProperty(Constants.SoProperties.ProcessInstance.ProcInstId, true, true, false)
                .AddProperty(Constants.SoProperties.ProcessInstance.DataFieldName, true, true, false)
                .AddProperty(Constants.SoProperties.ProcessInstance.DataFieldValue, true, false, false);
            so.AddMethod(updateDataField);

            var listDataFields = new ServiceObjectMethodBuilder(Constants.Methods.ProcessInstance.ListDataFields,
                    "Lists the data fields with values from the Process Instance", MethodType.List)
                .AddProperty(Constants.SoProperties.ProcessInstance.ProcInstId, true, true, false)
                .AddProperty(Constants.SoProperties.ProcessInstance.DataFieldName, false, false, true)
                .AddProperty(Constants.SoProperties.ProcessInstance.DataFieldType, false, false, true)
                .AddProperty(Constants.SoProperties.ProcessInstance.DataFieldValue, false, false, true);
            so.AddMethod(listDataFields);

            var listXmlFields = new ServiceObjectMethodBuilder(Constants.Methods.ProcessInstance.ListXmlFields,
                    "Lists the data fields with values from the Process Instance", MethodType.List)
                .AddProperty(Constants.SoProperties.ProcessInstance.ProcInstId, true, true, false)
                .AddProperty(Constants.SoProperties.ProcessInstance.XmlFieldName, false, false, true)
                .AddProperty(Constants.SoProperties.ProcessInstance.XmlFieldValue, false, false, true);
            so.AddMethod(listXmlFields);

            var updateXmlField  = new ServiceObjectMethodBuilder(Constants.Methods.ProcessInstance.UpdateXmlField,
                    "Updates the XmlField of a running process instance", MethodType.Update)
                .AddProperty(Constants.SoProperties.ProcessInstance.ProcInstId, true, true, false)
                .AddProperty(Constants.SoProperties.ProcessInstance.XmlFieldName, true, true, false)
                .AddProperty(Constants.SoProperties.ProcessInstance.XmlFieldValue, true, false, false);
            so.AddMethod(updateXmlField);

            var setProcessInstanceVersion = new ServiceObjectMethodBuilder(Constants.Methods.ProcessInstance.SetProcessInstanceVersion,
                    "Migrates the workflow to another version", MethodType.Execute)
                .AddProperty(Constants.SoProperties.ProcessInstance.ProcInstId, true, true, false)
                .AddProperty(Constants.SoProperties.ProcessInstance.TargetProcVersion, true, true, false);
            so.AddMethod(setProcessInstanceVersion);

            var goToActivity = new ServiceObjectMethodBuilder(Constants.Methods.ProcessInstance.GoToActivity,
                    "Moves the workflow to another activity", MethodType.Execute)
                .AddProperty(Constants.SoProperties.ProcessInstance.ProcInstId, true, true, false)
                .AddProperty(Constants.SoProperties.ProcessInstance.FromActName, true, false, false)
                .AddProperty(Constants.SoProperties.ProcessInstance.ToActName, true, true, false);
            so.AddMethod(goToActivity);

            var listProcessInstances = new ServiceObjectMethodBuilder(
                    Constants.Methods.ProcessInstance.ListProcessInstances,
                    "Lists the process instances", MethodType.List)
                .AddProperty(Constants.SoProperties.ProcessInstance.ProcInstId, false, false, true)
                .AddProperty(Constants.SoProperties.ProcessInstance.ProcessFolio, false, false, true)
                .AddProperty(Constants.SoProperties.ProcessInstance.StartDate, false, false, true)
                .AddProperty(Constants.SoProperties.ProcessInstance.Status, false, false, true)
                .AddProperty(Constants.SoProperties.ProcessInstance.Originator, false, false, true)
                .AddProperty(Constants.SoProperties.ProcessInstance.Version, false, false, true)
                .AddProperty(Constants.SoProperties.ProcessInstance.ExecutingVersion, false, false, true)
                .AddProperty(Constants.SoProperties.ProcessInstance.ProcSetId, false, false, true)
                .AddProperty(Constants.SoProperties.ProcessInstance.ProcId, false, false, true)
                .AddParameter(Constants.SoParameters.ProcessSetId, SoType.Number);
            so.AddMethod(listProcessInstances);

            return new List<ServiceObject>() { so };
        }
        public override void Execute()
        {
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.ProcessInstance.UpdateFolio:
                    UpdateFolio();
                    break;
                case Constants.Methods.ProcessInstance.UpdateDataField:
                    UpdateDataField();
                    break;
                case Constants.Methods.ProcessInstance.ListDataFields:
                    ListDataFields();
                    break;
                case Constants.Methods.ProcessInstance.ListXmlFields:
                    ListXmlFields();
                    break;
                case Constants.Methods.ProcessInstance.UpdateXmlField:
                    UpdateXmlField();
                    break;
                case Constants.Methods.ProcessInstance.GoToActivity:
                    GoToActivity();
                    break;
                case Constants.Methods.ProcessInstance.SetProcessInstanceVersion:
                    SetProcessInstanceVersion();
                    break;
                case Constants.Methods.ProcessInstance.ListProcessInstances:
                    ListProcessInstances();
                    break;
            }
        }

        private void UpdateFolio()
        {
            var folio = GetStringProperty(Constants.SoProperties.ProcessInstance.ProcessFolio, true);
            var procId = GetIntProperty(Constants.SoProperties.ProcessInstance.ProcInstId, true);

            using (_wfClient = ServiceBroker.K2Connection.GetWorkflowClientConnection())
            {
                var pi = _wfClient.OpenProcessInstance(procId);
                pi.Folio = folio;
                pi.Update();
            }
        }
        private void UpdateDataField()
        {
            var dataFieldName = GetStringProperty(Constants.SoProperties.ProcessInstance.DataFieldName, true);
            var dataFieldValue = GetStringProperty(Constants.SoProperties.ProcessInstance.DataFieldValue);
            var procId = GetIntProperty(Constants.SoProperties.ProcessInstance.ProcInstId, true);

            using (_wfClient = ServiceBroker.K2Connection.GetWorkflowClientConnection())
            {
                var pi = _wfClient.OpenProcessInstance(procId);
                var dataField = pi.DataFields[dataFieldName];
                switch(dataField.ValueType)
                {
                    case client.DataType.TypeBinary:
                        dataField.Value = Convert.FromBase64String(dataFieldValue);
                        break;
                    case client.DataType.TypeBoolean:
                        dataField.Value = Convert.ToBoolean(dataFieldValue);
                        break;
                    case client.DataType.TypeDate:
                        dataField.Value = Convert.ToDateTime(dataFieldValue);
                        break;
                    case client.DataType.TypeDecimal:
                        dataField.Value = Convert.ToDecimal(dataFieldValue);
                        break;
                    case client.DataType.TypeDouble:
                        dataField.Value = Convert.ToDouble(dataFieldValue);
                        break;
                    case client.DataType.TypeInteger:
                        dataField.Value = Convert.ToInt32(dataFieldValue);
                        break;
                    case client.DataType.TypeLong:
                        dataField.Value = Convert.ToInt64(dataFieldValue);
                        break;
                    default:
                        dataField.Value = dataFieldValue;
                        break;
                }
                pi.Update();
            }
        }
        private void ListDataFields()
        {
            var procId = GetIntProperty(Constants.SoProperties.ProcessInstance.ProcInstId, true);
            ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            var dt = ServiceBroker.ServicePackage.ResultTable;

            using (_wfClient = ServiceBroker.K2Connection.GetWorkflowClientConnection())
            {
                var pi = _wfClient.OpenProcessInstance(procId);
                foreach (client.DataField dataField in pi.DataFields)
                {
                    var dRow = dt.NewRow();
                    dRow[Constants.SoProperties.ProcessInstance.DataFieldName] = dataField.Name;
                    dRow[Constants.SoProperties.ProcessInstance.DataFieldType] = dataField.ValueType.ToString();
                    string dataFieldValue;
                    switch (dataField.ValueType)
                    {
                        case client.DataType.TypeBinary:
                            dataFieldValue= Convert.ToBase64String((byte[])dataField.Value);
                            break;
                        case client.DataType.TypeDate:
                            dataFieldValue = Convert.ToDateTime(dataField.Value).ToString("yyyy-MM-dd HH:mm:ss");
                            break;
                        default:
                            dataFieldValue = Convert.ToString(dataField.Value);
                            break;
                    }
                    dRow[Constants.SoProperties.ProcessInstance.DataFieldValue] = dataFieldValue;
                    dt.Rows.Add(dRow);
                }
            }
        }
        private void ListXmlFields()
        {
            var procId = GetIntProperty(Constants.SoProperties.ProcessInstance.ProcInstId, true);
            ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            var dt = ServiceBroker.ServicePackage.ResultTable;

            using (_wfClient = ServiceBroker.K2Connection.GetWorkflowClientConnection())
            {
                var pi = _wfClient.OpenProcessInstance(procId);
                foreach (client.XmlField xmlField in pi.XmlFields)
                {
                    var dRow = dt.NewRow();
                    dRow[Constants.SoProperties.ProcessInstance.XmlFieldName] = xmlField.Name;
                    dRow[Constants.SoProperties.ProcessInstance.XmlFieldValue] = xmlField.Value;
                    dt.Rows.Add(dRow);
                }
            }
        }
        private void UpdateXmlField()
        {
            var xmlFieldName = GetStringProperty(Constants.SoProperties.ProcessInstance.XmlFieldName, true);
            var xmlFieldValue = GetStringProperty(Constants.SoProperties.ProcessInstance.XmlFieldValue);
            var procId = GetIntProperty(Constants.SoProperties.ProcessInstance.ProcInstId, true);

            using (_wfClient = ServiceBroker.K2Connection.GetWorkflowClientConnection())
            {
                var pi = _wfClient.OpenProcessInstance(procId);
                var xmlField = pi.XmlFields[xmlFieldName];
                xmlField.Value = xmlFieldValue;
                pi.Update();
            }
        }
        private void GoToActivity()
        {
            var fromActivity = GetStringProperty(Constants.SoProperties.ProcessInstance.FromActName);
            var toActivity = GetStringProperty(Constants.SoProperties.ProcessInstance.ToActName, true);
            var procInstId = GetIntProperty(Constants.SoProperties.ProcessInstance.ProcInstId, true);

            using (_mngServer.Connection = ServiceBroker.K2Connection.GetConnection())
            {
                if (string.IsNullOrEmpty(fromActivity))
                {
                    _mngServer.GotoActivity(procInstId, toActivity);
                }
                else
                {
                    _mngServer.GotoActivity(procInstId, fromActivity, toActivity);
                }
            }
        }
        private void SetProcessInstanceVersion()
        {
            var targetProcVersion = GetIntProperty(Constants.SoProperties.ProcessInstance.TargetProcVersion, true);
            var procInstId = GetIntProperty(Constants.SoProperties.ProcessInstance.ProcInstId, true);

            using (_mngServer.Connection = ServiceBroker.K2Connection.GetConnection())
            {
                var filter = new ProcessInstanceCriteriaFilter();
                filter.REGULAR_FILTER(mng.ProcessInstanceFields.ProcInstID, Comparison.Equals, procInstId);
                mng.ProcessInstances procInstances = _mngServer.GetProcessInstancesAll(filter);
                var procInstStatus = (mng.ProcessInstanceStatus)Enum.Parse(typeof(mng.ProcessInstanceStatus), procInstances[0]?.Status, true);
                switch (procInstStatus)
                {
                    case mng.ProcessInstanceStatus.Active:
                        _mngServer.StopProcessInstances(procInstId);
                        _mngServer.SetProcessInstanceVersion(procInstId, targetProcVersion);
                        _mngServer.StartProcessInstances(procInstId);
                        break;
                    case mng.ProcessInstanceStatus.Error:
                    case mng.ProcessInstanceStatus.Stopped:
                        _mngServer.SetProcessInstanceVersion(procInstId, targetProcVersion);
                        break;
                }
            }
        }

        private void ListProcessInstances()
        {
            ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            var dt = ServiceBroker.ServicePackage.ResultTable;
            var procSetId = GetIntParameter(Constants.SoParameters.ProcessSetId);
            using (_mngServer.Connection = ServiceBroker.K2Connection.GetConnection())
            {
                var filter = new ProcessInstanceCriteriaFilter();
                if (procSetId > 0)
                {
                    filter.AddRegularFilter(mng.ProcessInstanceFields.ProcSetID, Comparison.Equals, procSetId);
                }
                var procInstances = _mngServer.GetProcessInstancesAll(filter);
                foreach (mng.ProcessInstance pi in procInstances)
                {
                    
                    var dRow = dt.NewRow();
                    dRow[Constants.SoProperties.ProcessInstance.ProcInstId] = pi.ID;
                    dRow[Constants.SoProperties.ProcessInstance.ProcessFolio] = pi.Folio;
                    dRow[Constants.SoProperties.ProcessInstance.StartDate] = pi.StartDate;
                    dRow[Constants.SoProperties.ProcessInstance.Originator] = pi.Originator;
                    dRow[Constants.SoProperties.ProcessInstance.ProcId] = pi.ProcID;
                    dRow[Constants.SoProperties.ProcessInstance.ProcSetId] = pi.ProcSetID;
                    dRow[Constants.SoProperties.ProcessInstance.Status] = Convert.ToString(Enum.Parse(typeof(mng.ProcessInstanceStatus), pi.Status, true));
                    dRow[Constants.SoProperties.ProcessInstance.Version] = _mngServer.GetProcess(pi.ProcID)?.VersionNumber;
                    dRow[Constants.SoProperties.ProcessInstance.ExecutingVersion] = _mngServer.GetProcess(pi.ExecutingProcID)?.VersionNumber;
                    dt.Rows.Add(dRow);
                }
            }
        }
    }
}
