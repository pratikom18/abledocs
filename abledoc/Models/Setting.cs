using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Models
{
    public class Setting
    {
        public int ID { get; set; }
        public string PageName { get; set; }
        public string SectionName { get; set; }
        public int OrderNo { get; set; }
        public bool Active { get; set; }
    }
}
