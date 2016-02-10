namespace G19BuildScreen.Authorizers
{
    using System.Collections.Generic;

    using Microsoft.TeamFoundation.Client;

    public interface IBuildInformationAuthorizer
    {
        TfsTeamProjectCollection Authorize(IDictionary<string, string> authorizeInformation);
    }
}