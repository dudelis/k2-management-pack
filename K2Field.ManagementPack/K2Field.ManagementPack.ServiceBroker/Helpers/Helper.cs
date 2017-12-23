using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;

namespace K2Field.ManagementPack.ServiceBroker.Helpers
{
    public static class Helper
    {
        public static Property CreateProperty(string name, SoType type, string description)
        {
            return CreateSpecificProperty(name, name, description, type);
        }

        public static Property CreateProperty(string name, string description, SoType type)
        {
            return CreateSpecificProperty(name, name, description, type);
        }

        public static FileProperty CreateFileProperty(string name, string description)
        {
            var metadata = new MetaData()
            {
                DisplayName = name,
                Description = description
            };
            var fileProperty = new FileProperty(name, metadata, string.Empty, string.Empty);
            return fileProperty;
        }


        /// <summary>
        /// Create an instance of a Service Object property
        /// This method allows you to set the displayname specifically.
        /// </summary>
        /// <param name="name">DisplayName of the property</param>
        /// <param name="displayName">The displayname to use.</param>
        /// <param name="description">A short description.</param>
        /// <param name="type">SMO Type of the property.</param>
        /// <returns>The property</returns>
        public static Property CreateSpecificProperty(string name, string displayName, string description, SoType type)
        {
            Property property = new Property
            {
                Name = name,
                SoType = type,
                Type = MapHelper.GetTypeBySoType(type),
                MetaData = new MetaData(displayName, description)
            };
            return property;

        }
    }
}
