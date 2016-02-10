namespace G19BuildScreen.BuildInformationGetter
{
    using Microsoft.TeamFoundation.Build.Client;

    public interface IBuildInformationGetter
    {
        G19BuildScreenAppletModel GetBuildInformation(IBuildServer buildServer, string buildDefinition, string teamProject);
    }
}