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
                case "Succeeded":
                    return Colors.LimeGreen;
                    break;
                case "InProgress":
                    return Colors.White;
                    break;
                default:
                    return Colors.Fuchsia;
                    break;
            }
        }
    }
}
