using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Models
{
    public class CustomField
    {
        public string Type { get; set; }
        public string State { get; set; }
        public Dictionary<string,string> Label { get; set; }
        public Dictionary<string, string> Values { get; set; }

        public Dictionary<string, string> Pattern { get; set; }
    }
   
}
