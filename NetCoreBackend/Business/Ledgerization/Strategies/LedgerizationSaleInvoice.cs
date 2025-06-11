using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Concrate;

namespace Business.Ledgerization.Strategies
{
    public class LedgerizationSaleInvoice
    {
        /// <summary>
        /// Satış faturası için tüm defter kayıtlarını oluşturur.
        /// </summary>
        /// <param name="ledgerId">Defter ID'si.</param>
        /// <param name="saleInvoice">Satış faturası nesnesi.</param>
        /// <param name="saleInvoiceLines">Satış faturası satırları listesi.</param>
        /// <returns>Oluşturulan defter kayıtlarının listesi.</returns>
        public List<LedgerEntry> CreateAllSaleInvoiceLedgerEntries(int ledgerId, SaleInvoice saleInvoice, List<SaleInvoiceLine> saleInvoiceLines)
        {
            List<LedgerEntry> ledgerEntries = new List<LedgerEntry>();
            int lineNo = 1; // Defter kayıtları için başlangıç satır numarası

            // 1. Satış Hesapları için Defterleştirme (Alacak)
            var salesAmountsGroupBySaleAccountId = saleInvoiceLines
                .GroupBy(x => x.SaleAccountId)
                .Select(g => new
                {
                    SaleAccountId = g.Key,
                    SumAmount = g.Sum(x => x.Amount)
                }).ToList();

            foreach (var item in salesAmountsGroupBySaleAccountId)
            {
                ledgerEntries.Add(new LedgerEntry
                {
                    LedgerId = ledgerId,
                    LineNo = lineNo++,
                    AccountId = item.SaleAccountId,
                    PartnerId = saleInvoice.PartnerId,
                    Description = $"Satış - {saleInvoice.InvoiceNo}",
                    Debit = 0,
                    Credit = item.SumAmount,
                });
            }

            // 2. Alacak Hesapları için Defterleştirme (Borç)
            decimal totalInvoiceAmount = saleInvoice.SaleInvoiceType == SaleInvoiceType.ExportInvoice
                ? saleInvoiceLines.Sum(x => x.Amount)
                : saleInvoiceLines.Sum(x => x.TotalAmount);

            ledgerEntries.Add(new LedgerEntry
            {
                LedgerId = ledgerId,
                LineNo = lineNo++,
                AccountId = saleInvoice.CustomerAccountId, // Alacak Hesabı
                PartnerId = saleInvoice.PartnerId,
                Description = $"Alacak - {saleInvoice.InvoiceNo}",
                Debit = totalInvoiceAmount,
                Credit = 0,
            });

            // 3. KDV Hesapları için Defterleştirme (Alacak)
            var vatAmountsGroupByVatId = saleInvoiceLines
                .Where(x => x.VatId != 1) // KDV'si olan satırları filtrele
                .GroupBy(x => x.VatId)
                .Select(g => new
                {
                    VatId = g.Key,
                    SumVatAmount = g.Sum(x => x.VatTaxAmount)
                }).ToList();


            foreach (var item in vatAmountsGroupByVatId)
            {
                int vatAccountId = GetVatAccountIdForCredit(item.VatId);

                ledgerEntries.Add(new LedgerEntry
                {
                    LedgerId = ledgerId,
                    LineNo = lineNo++,
                    AccountId = vatAccountId, // KDV Hesabı
                    PartnerId = saleInvoice.PartnerId,
                    Description = $"KDV - {saleInvoice.InvoiceNo}",
                    Debit = 0,
                    Credit = item.SumVatAmount,
                });
            }

            return ledgerEntries;
        }

        private int GetVatAccountIdForCredit(int vatId)
        {
            return vatId switch
            {
                2 => 63, // %8 KDV Hesabı
                3 => 62, // %18 KDV Hesabı,
                _ => throw new ArgumentException("Geçersiz KDV ID'si")
            };
        }
    }
}
