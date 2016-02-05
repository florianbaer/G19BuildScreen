using System;
using System.Net;
using System.Windows.Controls;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using TeamProject = Microsoft.TeamFoundation.Framework.Client.Catalog.Objects.TeamProject;

namespace G19BuildScreen
{
    using System.Configuration;

    /// <summary>
    ///     Interaction logic for G19BuildScreenApplet.xaml
    /// </summary>
    public partial class G19BuildScreenApplet : UserControl
    {
        string tfsUsername;
        string tfsPassword;
        string tfsUri;
        public G19BuildScreenApplet()
        {
            InitializeComponent();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            tfsUsername = config.AppSettings.Settings["Username"].Value;
            
            tfsPassword = config.AppSettings.Settings["Password"].Value;

            tfsUri = config.AppSettings.Settings["Uri"].Value;
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