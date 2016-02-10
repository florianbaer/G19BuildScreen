namespace G19BuildScreen.BuildInformationGetter
{
    using Microsoft.TeamFoundation.Build.Client;
    using Microsoft.TeamFoundation.TestManagement.Client;

    public class FinishedBuildInformationGetter : IBuildInformationGetter
    {
        public G19BuildScreenAppletModel GetBuildInformation(IBuildServer buildServer, string buildDefinition, string teamProject)
        {
            G19BuildScreenAppletModel model = new G19BuildScreenAppletModel();

            IBuildDefinition buildDetailSpec = buildServer.GetBuildDefinition(teamProject, buildDefinition);

            IBuildDetail build = buildServer.GetBuild(buildDetailSpec.LastBuildUri);

            switch (build.Status)
            {
                case BuildStatus.Succeeded:
                    model.Status = build.Status.ToString();
                    break;
                case BuildStatus.Failed:
                    model.Status = build.Status.ToString();
                    break;
                case BuildStatus.InProgress:
                    model.Status = build.Status.ToString();
                    break;
                case BuildStatus.PartiallySucceeded:
                    model.Status = build.Status.ToString();
                    break;
                case BuildStatus.Stopped:
                    model.Status = build.Status.ToString();
                    break;
                default:
                    break;
            }


            model.RequestedBy = build.RequestedBy;
            model.TeamProjectName = build.TeamProject;
            model.TimeRequested = build.StartTime;
            model.BuildDefinitionName = build.BuildDefinition.Name;
            model.BuildUri = build.Uri;
            return model;
        }
    }
}