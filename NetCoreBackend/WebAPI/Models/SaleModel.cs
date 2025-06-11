using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;

namespace WebAPI.Models
{
    public class SaleModel
    {
        public Ledger Ledger { get; set; }
        public List<SaleInvoice> SaleInvoices { get; set; }
        public List<SaleInvoiceLine> SaleInvoiceLines { get; set; }
    }
}
