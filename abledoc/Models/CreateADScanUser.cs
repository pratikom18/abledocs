using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Models
{
    public class CreateADScanUser
    {
        public string Email { get; set; }
        public int CompanyID { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Phone_Num { get; set; }
        public string Company_Address { get; set; }
        public string Postal_Code { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public int ADO_Contact_ID { get; set; }
        public string Lang { get; set; }
        public string Role { get; set; }
    }
}
