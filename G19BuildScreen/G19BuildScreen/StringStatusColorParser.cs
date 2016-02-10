using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G19BuildScreen
{
    using System.Windows.Media;

    public static class StringStatusColorParser
    {
        public static Color GetColorForStatus(string status)
        {
            switch (status)
            {
                case "Successful":
                    return Colors.LimeGreen;
                    break;
                default:
                    return Colors.Fuchsia;
                    break;
            }
        }
    }
}
