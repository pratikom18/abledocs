using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Models
{
    public class GenerateQuotePdf
    {
        public Clients modelClient { get; set; }
        public ClientsContacts modelClientContact { get; set; }
        public DateTime currentDate { get; set; }
        public List<QuoteTemp> subtitle { get; set; }
        public List<QuoteTemp> offering { get; set; }
        public List<QuoteTemp> notes { get; set; }
        public List<JobsFiles> modelJobFiles { get; set; }
    }
}
