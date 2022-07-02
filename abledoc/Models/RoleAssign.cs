using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Models
{
   // [Serializable]
    public class RoleAssign
    {
        public int roleid { get; set; }
        public int menuid { get; set; }
        public bool isview { get; set; }
        public bool isadd { get; set; }
        public bool isupdate { get; set; }
        public bool isdelete { get; set; }
    }
}
