using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Models
{
    public class VarianceState
    {
        public int jobID { get; set; }
        public int displayQuote { get; set; }
        public double timedValueERPRecorded { get; set; }
        public double timedValueERPAltered { get; set; }
        public double varianceAmt { get; set; }
        public string varianceMessage { get; set; }
        public int invoiceIDQB { get; set; }
        public int invoiceID { get; set; }
        public string flag { get; set; }
    }
}
