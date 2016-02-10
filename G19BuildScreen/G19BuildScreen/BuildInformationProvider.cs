namespace G19BuildScreen
{
    using System.Collections.Generic;

    using G19BuildScreen.Authorizers;
    using G19BuildScreen.BuildInformationGetter;

    using Microsoft.TeamFoundation.Build.Client;
    using Microsoft.TeamFoundation.Client;

    public class BuildInformationProvider
    {
        private IBuildInformationAuthorizer authorizer;

        private TfsTeamProjectCollection tfs;

        private IBuildInformationGetter informationGetter;

        private IBuildServer buildService;

        public BuildInformationProvider(IBuildInformationAuthorizer authorizer, IBuildInformationGetter informationGetter)
        {
            this.informationGetter = informationGetter;
            this.authorizer = authorizer;
        }

        private void Authorize()
        {
            tfs = this.authorizer.Authorize(new Dictionary<string, string>());
        }

        public G19BuildScreenAppletModel GetBuildInformation(string buildDefinition, string teamProject)
        {
            this.Authorize();
            buildService = (IBuildServer)this.tfs.GetService(typeof(IBuildServer));

            G19BuildScreenAppletModel information = new QueuedBuildInformationGetter().GetBuildInformation(this.buildService, buildDefinition, teamProject);

            if (information == null)
            {
                information = this.informationGetter.GetBuildInformation(
                    this.buildService,
                    buildDefinition,
                    teamProject);

                information.TestResults =
                    new TestResultProvider(this.tfs, teamProject).GetTestResult(information.BuildUri);
            }

            return information;
        }
    }
}