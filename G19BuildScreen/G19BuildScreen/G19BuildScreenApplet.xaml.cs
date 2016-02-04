using System;
using System.Net;
using System.Windows.Controls;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using TeamProject = Microsoft.TeamFoundation.Framework.Client.Catalog.Objects.TeamProject;

namespace G19BuildScreen
{
    /// <summary>
    ///     Interaction logic for G19BuildScreenApplet.xaml
    /// </summary>
    public partial class G19BuildScreenApplet : UserControl
    {
        public G19BuildScreenApplet()
        {
            InitializeComponent();
            //// this.GetBuilds();
        }

        public void GetBuilds()
        {
            NetworkCredential cred = new NetworkCredential("user", "pass");
            
            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(new Uri("uri"), cred);

            tfs.Authenticate();
            var buildService = (IBuildServer)tfs.GetService(typeof(IBuildServer));
            {
                if (buildService != null)
                {
                    IBuildDefinition buildDetailSpec = buildService.GetBuildDefinition("project", "Definition");

                    IBuildDetail build = buildService.GetBuild(buildDetailSpec.LastBuildUri);

                    if (build.Status == BuildStatus.Succeeded)
                    {
                        Console.WriteLine("Successful");
                    }
                }
            }


        }
    }
}