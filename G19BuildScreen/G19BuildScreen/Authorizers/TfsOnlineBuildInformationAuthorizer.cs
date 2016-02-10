using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G19BuildScreen.Authorizers
{
    using System.Net;

    using Microsoft.TeamFoundation.Client;

    public class TfsOnlineBuildInformationAuthorizer : IBuildInformationAuthorizer
    {
        public TfsTeamProjectCollection Authorize(IDictionary<string, string> authorizeInformation)
        {
            NetworkCredential netCred = new NetworkCredential(
                "yourbasicauthusername@live.com",
                "yourbasicauthpassword");
            BasicAuthCredential basicCred = new BasicAuthCredential(netCred);
            TfsClientCredentials tfsCred = new TfsClientCredentials(basicCred);
            tfsCred.AllowInteractive = false;

            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(
                new Uri("https://YourAccountName.visualstudio.com/DefaultCollection"),
                tfsCred);

            tpc.Authenticate();
            return tpc;
        }
    }
}
