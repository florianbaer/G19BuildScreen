// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="G19BuildScreenApplet.xaml.cs" company="BaerDev">
// // Copyright (c) BaerDev. All rights reserved.
// // </copyright>
// // <summary>
// // The file 'G19BuildScreenApplet.xaml.cs'.
// // </summary>
// // --------------------------------------------------------------------------------------------------------------------

namespace G19BuildScreen
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Threading;
    using Microsoft.TeamFoundation.Build.Client;
    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.TestManagement.Client;

    /// <summary>
    ///     Interaction logic for G19BuildScreenApplet.xaml
    /// </summary>
    public partial class G19BuildScreenApplet : UserControl
    {
        private readonly string BuildDefinition;

        private readonly string TeamProject;

        private readonly string tfsPassword;

        private readonly string tfsUri;

        private readonly string tfsUsername;

        private int error;

        private int failed;

        private int inconclusive;

        private int passed;

        private TfsTeamProjectCollection tfs;

        private DispatcherTimer timer;

        private int totalTests;

        public G19BuildScreenApplet()
        {
            this.InitializeComponent();

            //// TeamProjectPicker picker = new TeamProjectPicker(TeamProjectPickerMode.MultiProject, true);
            //// picker.ShowDialog();
            //// ProjectInfo[] projects = picker.SelectedProjects;

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // Write values

            this.tfsUsername = config.AppSettings.Settings["Username"].Value;

            this.tfsPassword = config.AppSettings.Settings["Password"].Value;

            this.tfsUri = config.AppSettings.Settings["Uri"].Value;
            this.TeamProject = config.AppSettings.Settings["TeamProject"].Value;

            // Save the changes in App.config file.
            config.Save(ConfigurationSaveMode.Modified);
            this.BuildDefinition = config.AppSettings.Settings["BuildDefinition"].Value;

            this.timer = new DispatcherTimer(
                TimeSpan.FromSeconds(5.0),
                DispatcherPriority.Render,
                this.UpdateUserInterface,
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

            this.tfs = new TfsTeamProjectCollection(new Uri(this.tfsUri), cred);

            this.tfs.Authenticate();
            var buildService = (IBuildServer)this.tfs.GetService(typeof(IBuildServer));
            {
                if (buildService != null)
                {
                    IBuildDefinition buildDetailSpec = buildService.GetBuildDefinition(
                        this.TeamProject,
                        this.BuildDefinition);

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
            var testManagementService = this.tfs.GetService<ITestManagementService>();
            var testRuns = testManagementService.GetTeamProject("DvdManager").TestRuns.ByBuild(buildUri);

            var testRun = testRuns.FirstOrDefault();

            if (testRun != null)
            { 
                this.totalTests = testRun.Statistics.TotalTests;

                this.error = testRun.QueryResultsByOutcome(TestOutcome.Error).Count;

                this.passed = testRun.QueryResultsByOutcome(TestOutcome.Passed).Count;

                this.failed = testRun.QueryResultsByOutcome(TestOutcome.Failed).Count;

                this.inconclusive = testRun.QueryResultsByOutcome(TestOutcome.Inconclusive).Count;

                this.TestResultsLabel.Content =
                    $"Total :{this.totalTests} \n Passed :{this.passed} \n Error :{this.error} \n Failed :{this.failed} \n Inconclusive :{this.inconclusive}";
            }
        }
    }
}