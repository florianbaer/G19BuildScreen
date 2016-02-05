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
    using System.Linq;
    using System.Windows.Media;
    using System.Windows.Threading;
    using Microsoft.TeamFoundation.TestManagement.Client;
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

        TfsTeamProjectCollection tfs;
        DispatcherTimer timer;

        int failed;
        int inconclusive;
        int passed;
        int error;
        int totalTests;

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
            
            tfs = new TfsTeamProjectCollection(new Uri(this.tfsUri), cred);

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
                            this.DefinitionNameValueLabel.Content = queuedBuild.Build.BuildDefinition.Name;

                            switch (queuedBuild.Status)
                            {
                                case QueueStatus.InProgress:
                                    this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                                    this.SuccessfulLabelValue.Content = queuedBuild.Status.ToString();
                                    break;
                                case QueueStatus.Canceled:
                                    this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.Coral);
                                    this.SuccessfulLabelValue.Content = queuedBuild.Status.ToString();
                                    break;
                                default:
                                    this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.Fuchsia);
                                    this.SuccessfulLabelValue.Content = "Not applicable";
                                    break;
                            }
                        }
                    }
                    else
                    {

                        IBuildDetail build = buildService.GetBuild(buildDetailSpec.LastBuildUri);

                        this.DefinitionNameValueLabel.Content = build.BuildDefinition.Name;
                        
                        this.GetTestResult(build.Uri);

                        switch (build.Status)
                        {
                            case BuildStatus.Succeeded:
                                this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                                this.SuccessfulLabelValue.Content = build.Status.ToString();
                                break;
                            case BuildStatus.Failed:
                                this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                                this.SuccessfulLabelValue.Content = build.Status.ToString();
                                break;
                            case BuildStatus.InProgress:
                                this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                                this.SuccessfulLabelValue.Content = build.Status.ToString();
                                break;
                            case BuildStatus.PartiallySucceeded:
                                this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.Orange);
                                this.SuccessfulLabelValue.Content = build.Status.ToString();
                                break;
                            case BuildStatus.Stopped:
                                this.StatusBorder.BorderBrush = new SolidColorBrush(Colors.PaleVioletRed);
                                this.SuccessfulLabelValue.Content = build.Status.ToString();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }


        }

        private void GetTestResult(Uri buildUri)
        {
            var testManagementService = tfs.GetService<ITestManagementService>();
            var testRuns = testManagementService.GetTeamProject("DvdManager").TestRuns.ByBuild(buildUri);
            
            var testRun = testRuns.FirstOrDefault();


            if (testRun != null)
            {

                
                totalTests = testRun.Statistics.TotalTests;

                error = testRun.QueryResultsByOutcome(TestOutcome.Error).Count;

                passed = testRun.QueryResultsByOutcome(TestOutcome.Passed).Count;

                failed = testRun.QueryResultsByOutcome(TestOutcome.Failed).Count;

                
                inconclusive = testRun.QueryResultsByOutcome(TestOutcome.Inconclusive).Count;

                this.TestResultsLabel.Content =
                    $"Total :{this.totalTests} \n Passed :{passed} \n Error :{error} \n Failed :{failed} \n Inconclusive :{this.inconclusive}";
            }

        }
    }
}