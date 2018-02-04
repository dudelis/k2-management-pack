using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Field.ManagementPack.ServiceBroker.Constants
{
    public static class Methods
    {
        public static class ProcessInstance
        {
            public const string UpdateFolio = "UpdateFolio";
            public const string UpdateDataField = "UpdateDataField";
            public const string ListDataFields = "ListDataFields";
            public const string UpdateXmlField = "UpdateXmlField";
            public const string ListXmlFields = "ListXmlFields";
            public const string SetProcessInstanceVersion = "SetProcessInstanceVersion";
            public const string GoToActivity = "GoToActivity";
            public const string ListProcessInstances = "ListProcessInstances";

        }
        public static class Identity
        {
            public const string ResolveUser = "ResolveUser";
            public const string ResolveGroup = "ResolveGroup";
            public const string ResolveRole = "ResolveRole";
        }
    }
}
