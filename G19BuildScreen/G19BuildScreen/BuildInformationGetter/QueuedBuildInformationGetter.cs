namespace G19BuildScreen.BuildInformationGetter
{
    using System.Linq;
    using System.Windows.Media;

    using Microsoft.TeamFoundation.Build.Client;

    public class QueuedBuildInformationGetter : IBuildInformationGetter
    {
        public G19BuildScreenAppletModel GetBuildInformation(IBuildServer buildServer, string buildDefinition, string teamProject)
        {
            G19BuildScreenAppletModel model = new G19BuildScreenAppletModel();

            if (buildServer != null)
            {
                IQueuedBuildSpec queed = buildServer.CreateBuildQueueSpec(teamProject, buildDefinition);

                IQueuedBuildQueryResult queryResult = buildServer.QueryQueuedBuilds(queed);

                IQueuedBuild queuedBuild = queryResult.QueuedBuilds.FirstOrDefault();
                if (queuedBuild == null)
                {
                    return null;
                }
                model.BuildDefinitionName = queuedBuild.Build.BuildDefinition.Name;

                switch (queuedBuild.Status)
                {
                    case QueueStatus.InProgress:
                        model.Status = queuedBuild.Status.ToString();
                        break;
                    case QueueStatus.Canceled:
                        model.Status = queuedBuild.Status.ToString();
                        break;
                    default:
                        model.Status = queuedBuild.Status.ToString();
                        break;
                }
                model.RequestedBy = queuedBuild.RequestedBy;
                model.TeamProjectName = queuedBuild.TeamProject;
                model.TimeRequested = queuedBuild.QueueTime;
            }
            return model;
        }
    }
}