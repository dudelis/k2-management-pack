using K2Field.ManagementPack.ServiceBroker.Helpers;
using SourceCode.Hosting.Server.Interfaces;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using SourceCode.Workflow.Client;
using System;
using System.Collections.Generic;
using System.Data;


namespace K2Field.ManagementPack.ServiceBroker.ServiceObjects
{
    public class Identity : ServiceObjectBase
    {

        public Identity(ServiceBroker broker)
            : base(broker)
        {
        }
        public override string ServiceFolder => "Identity";

        public override List<ServiceObject> DescribeServiceObjects()
        {
            var so = new ServiceObjectBuilder("Identity", "Exposes functionality to resolve Identities in K2.", true);
            so.CreateProperty(Constants.SoProperties.Identity.FQN,
                   "Fully Qualified Name of the Identity.", SoType.Text)
               .CreateProperty(Constants.SoProperties.Identity.ResolveMembers,
                   "Resolve Members of the Identity.", SoType.YesNo)
               .CreateProperty(Constants.SoProperties.Identity.ResolveContainers,
                   "Resolve Containers of the Identity", SoType.YesNo);

            var resolveUser = new ServiceObjectMethodBuilder(Constants.Methods.Identity.ResolveUser, "Resolves the user", MethodType.Execute)
               .AddProperty(Constants.SoProperties.Identity.FQN, true, true, false)
               .AddProperty(Constants.SoProperties.Identity.ResolveContainers, true, false, false);
            so.AddMethod(resolveUser);

            var resolveGroup = new ServiceObjectMethodBuilder(Constants.Methods.Identity.ResolveGroup, "Resolves the group", MethodType.Execute)
               .AddProperty(Constants.SoProperties.Identity.FQN, true, true, false)
               .AddProperty(Constants.SoProperties.Identity.ResolveContainers, true, false, false)
               .AddProperty(Constants.SoProperties.Identity.ResolveMembers, true, false, false);
            so.AddMethod(resolveGroup);

            var resolveRole = new ServiceObjectMethodBuilder(Constants.Methods.Identity.ResolveRole, "Resolves the role", MethodType.Execute)
               .AddProperty(Constants.SoProperties.Identity.FQN, true, true, false)
               .AddProperty(Constants.SoProperties.Identity.ResolveContainers, true, false, false)
               .AddProperty(Constants.SoProperties.Identity.ResolveMembers, true, false, false);
            so.AddMethod(resolveRole);            

            return new List<ServiceObject> { so };
        }


        public override void Execute()
        {
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.Identity.ResolveUser:
                    ResolveIdentity(IdentityType.User);
                    break;

                case Constants.Methods.Identity.ResolveGroup:
                    ResolveIdentity(IdentityType.Group);
                    break;

                case Constants.Methods.Identity.ResolveRole:
                    ResolveIdentity(IdentityType.Role);
                    break;
            }
        }

        public void ResolveIdentity(IdentityType iType)
        {
            string fqn = GetStringProperty(Constants.SoProperties.Identity.FQN, true);
            bool resolveContainers = GetBoolProperty(Constants.SoProperties.Identity.ResolveContainers);
            bool resolveMembers = GetBoolProperty(Constants.SoProperties.Identity.ResolveMembers);

            var fqnName = new FQName(fqn);

            ServiceBroker.IdentityService.ResolveIdentity(fqnName, iType, IdentityResolveOptions.Identity);

            if (resolveMembers)
            {
                ServiceBroker.IdentityService.ResolveIdentity(fqnName, iType, IdentitySection.Members);
            }

            if (resolveContainers)
            {
                ServiceBroker.IdentityService.ResolveIdentity(fqnName, iType, IdentitySection.Containers);
            }


        }
    }
}

