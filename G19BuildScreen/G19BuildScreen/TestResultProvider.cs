using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G19BuildScreen
{
    using Microsoft.TeamFoundation.TestManagement.Client;

    public class TestResultProvider
    {
        private TestManagementService testManagementProvider;

        private string teamProject;

        public TestResultProvider(TestManagementService testManagementService, string teamProject)
        {
            this.testManagementProvider = testManagementService;
            this.teamProject = teamProject;
        }

        public TestResults GetTestResult(Uri buildUri)
        {
            TestResults results = new TestResults();

            var testRuns = testManagementProvider.GetTeamProject(this.teamProject).TestRuns.ByBuild(buildUri);

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
