using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;

namespace K2Field.ManagementPack.ServiceBroker.Helpers
{
    public class ServiceObjectBuilder
    {
        private ServiceObject so = new ServiceObject();

        public ServiceObjectBuilder(string name, string description, bool active)
        {
            so.Name = name;
            so.Active = active;
            so.MetaData = new MetaData()
            {
                Description = description,
                DisplayName = name
            };
        }

        public ServiceObjectBuilder CreateProperty(string name, string description, SoType soType)
        {
            var property = new Property
            {
                Name = name,
                SoType = soType,
                Type = MapHelper.GetTypeBySoType(soType),
                MetaData = new MetaData(name, description)
            };
            so.Properties.Add(property);
            return this;
        }

        public ServiceObjectBuilder AddMethod(Method method)
        {
            so.Methods.Add(method);
            return this;
        }

        public static implicit operator ServiceObject(ServiceObjectBuilder soBuilder)
        {
            return soBuilder.so;
        }
    }
}
