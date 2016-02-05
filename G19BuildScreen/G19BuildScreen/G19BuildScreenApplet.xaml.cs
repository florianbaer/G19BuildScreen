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
    using System.Windows.Media;
    using System.Windows.Threading;
    using Brush = System.Windows.Media.Brush;
    using Color = System.Drawing.Color;

    /// <summary>
    ///     Interaction logic for G19BuildScreenApplet.xaml
    /// </summary>
    public partial class G19BuildScreenApplet : UserControl
    {
        string tfsUsername;
        string tfsPassword;
        string tfsUri;
        DispatcherTimer timer;
        public G19BuildScreenApplet()
        {
            InitializeComponent();

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            tfsUsername = config.AppSettings.Settings["Username"].Value;
            
            tfsPassword = config.AppSettings.Settings["Password"].Value;

            tfsUri = config.AppSettings.Settings["Uri"].Value;
            
            timer = new DispatcherTimer(TimeSpan.FromSeconds(5.0), DispatcherPriority.Render, this.UpdateUserInterface, 
                Dispatcher.CurrentDispatcher);
            this.GetBuilds();
        }

        private void UpdateUserInterface(object sender, EventArgs eventArgs)
        {
            this.GetBuilds();
        }

        public void GetBuilds()
        {
            NetworkCredential cred = new NetworkCredential(this.tfsUsername, this.tfsPassword);
            
            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(new Uri(this.tfsUri), cred);

            tfs.Authenticate();
            var buildService = (IBuildServer)tfs.GetService(typeof(IBuildServer));
            {
                if (buildService != null)
                {
                    IBuildDefinition buildDetailSpec = buildService.GetBuildDefinition("DvdManager", "DvdManager_CI_Main");

                    IQueuedBuildSpec queed = buildService.CreateBuildQueueSpec("DvdManager", "DvdManager_CI_Main");

                    IQueuedBuildQueryResult queryResult = buildService.QueryQueuedBuilds(queed);

                    if (queryResult.QueuedBuilds.Length > 0)
                    {
                        foreach (IQueuedBuild queuedBuild in queryResult.QueuedBuilds)
                        {
                            switch (queuedBuild.Status)
                            {
                                case QueueStatus.InProgress:
                                    this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                                    break;
                                case QueueStatus.Canceled:
                                    this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.Coral);
                                    break;
                                default:
                                    this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.Fuchsia);
                                    break;
                            }
                        }
                    }
                    else
                    {

                        IBuildDetail build = buildService.GetBuild(buildDetailSpec.LastBuildUri);

                        switch (build.Status)
                        {
                            case BuildStatus.Succeeded:
                                this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                                break;
                            case BuildStatus.Failed:
                                this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                                break;
                            case BuildStatus.InProgress:
                                this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                                break;
                            case BuildStatus.PartiallySucceeded:
                                this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.Orange);
                                break;
                            case BuildStatus.Stopped:
                                this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.PaleVioletRed);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }


        }
    }
}