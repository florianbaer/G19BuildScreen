namespace G19BuildScreen.Authorizers
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using Microsoft.TeamFoundation.Client;
    using Microsoft.TeamFoundation.Server;

    public class NetworkBuildInformationAuthorizer : IBuildInformationAuthorizer
    {
        private string tfsUri;

        public NetworkBuildInformationAuthorizer(string tfsUri)
        {
            this.tfsUri = tfsUri;
        }

        public TfsTeamProjectCollection Authorize(IDictionary<string, string> authorizeInformation)
        {
            ////TeamProjectPicker picker = new TeamProjectPicker(TeamProjectPickerMode.SingleProject, true);
            ////picker.ShowDialog();
            ////ProjectInfo[] projects = picker.SelectedProjects[0].;

            NetworkCredential cred = new NetworkCredential(authorizeInformation["username"], authorizeInformation["password"]);

            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(new Uri(this.tfsUri), cred);

            tfs.Authenticate();

            return tfs;
        }
    }
}