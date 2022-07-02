using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Models
{
    public class GenerateInvoicePdf
    {
        public DateTime currentDate { get; set; }
        public string State { get; set; }
        public int CreditNoteFlag { get; set; }
        public string creditMemoIDQB { get; set; }
        public string EngagementNum { get; set; }
        public Clients clients { get; set; }
        public InvoiceInstance invoiceInstance { get; set; }
        public ClientsContacts BillingContact { get; set; }
        public ClientsContacts MainContact { get; set; }
        public List<InvoiceTmp> invoiceTmpList { get; set; }
        public List<CreditMemoTmp> creditMemoTmpList { get; set; }

    }
}
