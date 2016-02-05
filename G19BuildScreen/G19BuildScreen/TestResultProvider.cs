namespace G19BuildScreen
{
    using System;
    using System.Linq;

    using Microsoft.TeamFoundation.TestManagement.Client;

    public class TestResultProvider
    {
        private string teamProject;

        private TestManagementService testManagementProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestResultProvider"/> class.
        /// </summary>
        /// <param name="testManagementService">The test management service.</param>
        /// <param name="teamProject">The team project.</param>
        public TestResultProvider(TestManagementService testManagementService, string teamProject)
        {
            this.testManagementProvider = testManagementService;
            this.teamProject = teamProject;
        }

        public TestResults GetTestResult(Uri buildUri)
        {
            TestResults results = new TestResults();
            var testRuns = this.testManagementProvider.GetTeamProject(this.teamProject).TestRuns.ByBuild(buildUri);
            var testRun = testRuns.FirstOrDefault();
            if (testRun != null)
            {
                results.Total = testRun.Statistics.TotalTests;
                results.Error = testRun.QueryResultsByOutcome(TestOutcome.Error).Count;
                results.Passed = testRun.QueryResultsByOutcome(TestOutcome.Passed).Count;
                results.Failed = testRun.QueryResultsByOutcome(TestOutcome.Failed).Count;

                results.Inconclusive = testRun.QueryResultsByOutcome(TestOutcome.Inconclusive).Count;
            }

            return results;
        }
    }
}