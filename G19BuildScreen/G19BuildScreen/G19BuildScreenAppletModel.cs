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
        public string BuildDefinitionName { get; set; }
        public string RequestedBy { get; set; }

        public string Status { get; set; }

        public TestResults TestResults { get; set; }
    }
}
