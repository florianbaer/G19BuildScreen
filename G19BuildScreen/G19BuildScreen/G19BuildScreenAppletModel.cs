using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G19BuildScreen
{
    using Microsoft.TeamFoundation.TestManagement.Client;

    public class G19BuildScreenAppletModel
    {
        public G19BuildScreenAppletModel()
        {
            this.TestResults = new TestResults();
        }

        public string BuildDefinitionName { get; set; }
        public string RequestedBy { get; set; }

        public string Status { get; set; }

        public TestResults TestResults { get; set; }

        public string TeamProjectName { get; set; }

        public DateTime TimeRequested { get; set; }

        public Uri BuildUri { get; set; }
    }
}
