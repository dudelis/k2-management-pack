using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using SourceCode.ComponentModel;
using SourceCode.Deployment.Management;

namespace K2Field.Powershell.Module
{
    [Cmdlet(VerbsCommon.New, "K2Package")]
    public class NewK2Package : Cmdlet
    {
        private readonly PackageDeploymentManager _packageDeploymentManager = new PackageDeploymentManager();

        [Parameter(Mandatory = true, Position = 1,
            HelpMessage = "Full path to the kspx file, which should appear in the end.")]
        public string Path { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 2,
            HelpMessage = "Category in which the objects reside"
        )]
        public string Category { get; set; }

        [Parameter(Mandatory = true, Position = 3,
            HelpMessage = "Should the package be validated in the end")]
        public bool Validate { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 4,
            HelpMessage = "$false: Dependencies will be included into the package, $true: Dependencies will be included as reference.")]
        public SwitchParameter IncludeDependenciesAsReference { get; set; }

        protected override void BeginProcessing()
        {
            try
            {
                _packageDeploymentManager.CreateConnection();
                _packageDeploymentManager.Connection.Open(ModuleHelper.BuildConnectionString());
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
                Session session = _packageDeploymentManager.CreateSession("K2Module" + DateTime.Now.ToString());
                session.SetOption("NoAnalyze", true);
                PackageItemOptions options = PackageItemOptions.Create();
                var typeRef = new TypeRef(Category, "urn:SourceCode/Categories");
                var query = QueryItemOptions.Create(typeRef);
                var results = session.FindItems(query).Result;
                foreach (var result in results)
                {
                    options.Include(result, PackageItemMode.IncludeDependencies);
                }
                options.PackageItemMode = IncludeDependenciesAsReference ? PackageItemMode.IncludeDependenciesAsReference : PackageItemMode.IncludeDependencies;
                session.PackageItems(options);
                var fileStream = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                session.Model.Save(fileStream);
                fileStream.Close();
                _packageDeploymentManager.CloseSession(session.Name);
            }
            catch (Exception ex)
            {
                ErrorHelper.Write(ex);
            }
        }
        protected override void EndProcessing()
        {
            _packageDeploymentManager.Connection?.Close();
        }

        protected override void StopProcessing()
        {
            _packageDeploymentManager.Connection?.Close();
        }
    }
}
