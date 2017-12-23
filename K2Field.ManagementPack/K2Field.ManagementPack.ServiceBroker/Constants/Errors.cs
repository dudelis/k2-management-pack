using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Field.ManagementPack.ServiceBroker.Constants
{
    public static class Errors
    {
        public const string RequiredPropertyNotFound = "Required property {0} is not found!";
        public const string RequiredPropertyIsEmpty = "Required property {0} is empty!";
        public const string RequiredParameterNotFound = "Required paramater {0} is not found!";
        public const string RequiredParameterIsEmpty = "Required parameter {0} is empty!";
        public const string SOIsNotSet = "Service Object is not set!";
        public const string IsNotValidSO = "Service Object {0} is not valid!";
        public const string NotParseToInteger = "Unable to cast property {0} to integer";
    }
}
