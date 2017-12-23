using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;

namespace K2Field.ManagementPack.ServiceBroker.Helpers
{
    public class ServiceObjectMethodBuilder
    {
        private Method method = new Method();

        public ServiceObjectMethodBuilder(string name, string description, MethodType type)
        {
            method.Name = name;
            method.Type = type;
            method.MetaData = new MetaData(name, description);
        }

        public ServiceObjectMethodBuilder AddProperty(string name, bool isInput, bool isRequired, bool isReturn)
        {
            if (isInput) method.InputProperties.Add(name);
            if (isRequired) method.Validation.RequiredProperties.Add(name);
            if (isReturn) method.ReturnProperties.Add(name);
            return this;
        }

        public ServiceObjectMethodBuilder AddParameter(string name, SoType soType, bool isRequired = false)
        {
            var methodParam = new MethodParameter
            {
                Name = name,
                IsRequired = isRequired,
                MetaData = new MetaData
                {
                    DisplayName = name
                },
                SoType = soType,
                Type = MapHelper.GetTypeBySoType(soType)
            };
            method.MethodParameters.Create(methodParam);
            return this;
        }
        public static implicit operator Method(ServiceObjectMethodBuilder builder)
        {
            return builder.method;
        }
    }
}
