using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G19BuildScreen
{
    public class TestResults
    {
        public int Total { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int Inconclusive { get; set; }
        public int Error { get; set; }
    }
}
