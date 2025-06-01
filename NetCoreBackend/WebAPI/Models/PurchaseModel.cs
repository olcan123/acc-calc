using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;

namespace WebAPI.Models
{
    public class PurchaseModel
    {
        public Ledger Ledger { get; set; }
        public List<PurchaseInvoice> PurchaseInvoices { get; set; }
        public List<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; }
        public List<PurchaseInvoiceExpense> PurchaseInvoiceExpenses { get; set; }
    }
    
}