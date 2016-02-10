namespace G19BuildScreen.Authorizers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;

    using Microsoft.TeamFoundation.Client;

    public class ActiveDirectoryBuildInformationAuthorizer : IBuildInformationAuthorizer
    {
        private string tfsUri;

        public ActiveDirectoryBuildInformationAuthorizer(string tfsUri)
        {
            this.tfsUri = tfsUri;
        }

        public TfsTeamProjectCollection Authorize(IDictionary<string, string> authorizeInformation)
        {
            var tfsCredentials = new TfsClientCredentials();
            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(new Uri(this.tfsUri));

            tfs.Authenticate();
            return tfs;
        }
    }
}