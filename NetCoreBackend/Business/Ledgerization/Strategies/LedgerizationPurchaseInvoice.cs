using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Concrate; // Bu namespace projenizde doğru olmalı

namespace Business.Ledgerization.Strategies
{
    public class LedgerizationPurchaseInvoice
    {
        /// <summary>
        /// Satın alma faturası için tüm defter kayıtlarını oluşturur.
        /// </summary>
        /// <param name="ledgerId">Defter ID'si.</param>
        /// <param name="purchaseInvoice">Satın alma faturası nesnesi.</param>
        /// <param name="purchaseInvoiceLines">Satın alma faturası satırları listesi.</param>
        /// <param name="purchaseInvoiceExpenses">Satın alma faturası masrafları listesi (opsiyonel).</param>
        /// <returns>Oluşturulan defter kayıtlarının listesi.</returns>
        public List<LedgerEntry> CreateAllPurchaseInvoiceLedgerEntries(int ledgerId, PurchaseInvoice purchaseInvoice, List<PurchaseInvoiceLine> purchaseInvoiceLines, List<PurchaseInvoiceExpense> purchaseInvoiceExpenses = null)
        {
            List<LedgerEntry> ledgerEntries = new List<LedgerEntry>();
            int lineNo = 1; // Defter kayıtları için başlangıç satır numarası

            // 1. Giderler, Envanter ve Yatırım Satırları için Defterleştirme
            var purchaseCostAmountsGroupByPurchaseAccountId = purchaseInvoiceLines
                .GroupBy(x => x.PurchaseAccountId)
                .Select(g => new
                {
                    PurchaseAccountId = g.Key,
                    SumCostAmount = g.Sum(x => x.CostAmount)
                }).ToList();

            foreach (var item in purchaseCostAmountsGroupByPurchaseAccountId)
            {
                ledgerEntries.Add(new LedgerEntry
                {
                    LedgerId = ledgerId,
                    PartnerId = null,
                    LineNo = lineNo++,
                    AccountId = item.PurchaseAccountId,
                    Description = $"{purchaseInvoice.InvoiceNo} - Mal/Hizmet Gideri",
                    Debit = item.SumCostAmount,
                    Credit = 0,
                });
            }            // 2. Satıcı Hesabı için Defterleştirme
            decimal totalInvoiceAmount = purchaseInvoice.InvoiceType == InvoiceType.ImportInvoice 
                ? purchaseInvoiceLines.Sum(x => x.Amount)
                : purchaseInvoiceLines.Sum(x => x.TotalAmount);

            ledgerEntries.Add(new LedgerEntry
            {
                LedgerId = ledgerId,
                PartnerId = purchaseInvoice.PartnerId,
                LineNo = lineNo++,
                AccountId = purchaseInvoice.VendorAccountId,
                Description = $"{purchaseInvoice.InvoiceNo} - Satıcı Borcu",
                Debit = 0,
                Credit = totalInvoiceAmount,
            });

            // 3. KDV Hesabı için Defterleştirme
            var vatAmountsGroupByVatAccountId = purchaseInvoiceLines
                .Where(x => x.VatId != 1) // KDV 1 ise dahil etme
                .GroupBy(x => x.VatId)
                .Select(g => new
                {
                    VatId = g.Key,
                    TotalVatAmount = g.Sum(x => x.VatTaxAmount)
                }).ToList();

            foreach (var item in vatAmountsGroupByVatAccountId)
            {
                int vatAccountId = GetVatAccountIdForDebit(item.VatId);

                ledgerEntries.Add(new LedgerEntry
                {
                    LedgerId = ledgerId,
                    PartnerId = null,
                    LineNo = lineNo++,
                    AccountId = vatAccountId,
                    Description = $"{purchaseInvoice.InvoiceNo} - Indirilecek KDV ({item.VatId} No'lu KDV)",
                    Debit = item.TotalVatAmount,
                    Credit = 0,
                });
            }

            // 4. İthalat Faturası ise KDV Alacak Hesabı için Defterleştirme
            if (purchaseInvoice.InvoiceType == InvoiceType.ImportInvoice)
            {
                foreach (var item in vatAmountsGroupByVatAccountId)
                {
                    int vatAccountId = GetVatAccountIdForCreditImport(item.VatId);

                    ledgerEntries.Add(new LedgerEntry
                    {
                        LedgerId = ledgerId,
                        PartnerId = null,
                        LineNo = lineNo++,
                        AccountId = vatAccountId,
                        Description = $"{purchaseInvoice.InvoiceNo} - İthalat KDV Borci ({item.VatId} No'lu KDV)",
                        Debit = 0,
                        Credit = item.TotalVatAmount,
                    });
                }
            }

            // 5. Gümrük Vergisi için Defterleştirme (Sadece İthalat Faturası ise geçerli olabilir, ancak genelde her faturada olmaz)
            // Bu kısım genellikle sadece ithalat faturaları için geçerlidir.
            // Eğer her satın alma faturasında gümrük vergisi olabiliyorsa bu if'i kaldırabilirsiniz.
            if (purchaseInvoice.InvoiceType == InvoiceType.ImportInvoice && purchaseInvoiceLines.Any(x => x.CustomsAmount > 0))
            {
                decimal customsTaxAmount = purchaseInvoiceLines.Sum(x => x.CustomsAmount);
                int customsTaxAccountId = 58; // Gümrük vergisi hesap kodu

                ledgerEntries.Add(new LedgerEntry
                {
                    LedgerId = ledgerId,
                    PartnerId = null,
                    LineNo = lineNo++,
                    AccountId = customsTaxAccountId,
                    Description = $"{purchaseInvoice.InvoiceNo} - Gümrük Vergisi",
                    Debit = 0,
                    Credit = customsTaxAmount,
                });
            }
            // 6. ÖTV için Defterleştirme (Sadece ithalat faturası ve ilgili ürünlerde)
            // ÖTV sadece ithalat faturalarında ve belirli ürün kategorilerinde uygulanır.
            if (purchaseInvoice.InvoiceType == InvoiceType.ImportInvoice && purchaseInvoiceLines.Any(x => x.ExciseTaxAmount > 0))
            {
                decimal exciseTaxAmount = purchaseInvoiceLines.Sum(x => x.ExciseTaxAmount);
                int exciseTaxAccountId = 61; // ÖTV hesap kodu

                ledgerEntries.Add(new LedgerEntry
                {
                    LedgerId = ledgerId,
                    PartnerId = null,
                    LineNo = lineNo++,
                    AccountId = exciseTaxAccountId,
                    Description = $"{purchaseInvoice.InvoiceNo} - ÖTV",
                    Debit = 0,
                    Credit = exciseTaxAmount,
                });
            }

            // 7. Diğer Satıcı Masrafları için Defterleştirme (Eğer masraf varsa)
            if (purchaseInvoiceExpenses != null && purchaseInvoiceExpenses.Any(x => x.Amount > 0))
            {
                var otherVendorEntries = RegisterOtherVendorLedgerization(ledgerId, purchaseInvoice, purchaseInvoiceExpenses);

                // Satır numarasını devam ettir
                foreach (var entry in otherVendorEntries)
                {
                    entry.LineNo = lineNo++;
                }

                ledgerEntries.AddRange(otherVendorEntries);
            }

            return ledgerEntries;
        }

        /// <summary>
        /// KDV için borç hesap kodunu döndürür.
        /// </summary>
        /// <param name="vatId">KDV ID'si.</param>
        /// <returns>İlgili hesap kodu.</returns>
        /// <exception cref="ArgumentException">Geçersiz KDV ID'si durumunda fırlatılır.</exception>
        private int GetVatAccountIdForDebit(int vatId)
        {
            return vatId switch
            {
                2 => 47, // KDV 2 için borç hesap kodu
                3 => 48, // KDV 3 için borç hesap kodu
                _ => throw new ArgumentException($"Geçersiz KDV ID: {vatId} - Borç hesabı belirlenemedi."),
            };
        }

        /// <summary>
        /// İthalat KDV'si için alacak hesap kodunu döndürür.
        /// </summary>
        /// <param name="vatId">KDV ID'si.</param>
        /// <returns>İlgili hesap kodu.</returns>
        /// <exception cref="ArgumentException">Geçersiz KDV ID'si durumunda fırlatılır.</exception>
        private int GetVatAccountIdForCreditImport(int vatId)
        {
            return vatId switch
            {
                2 => 59, // KDV 2 için ithalat alacak hesap kodu
                3 => 60, // KDV 3 için ithalat alacak hesap kodu
                _ => throw new ArgumentException($"Geçersiz KDV ID: {vatId} - İthalat alacak hesabı belirlenemedi."),
            };
        }        /// <summary>
                 /// Satın alma faturası masrafları için diğer satıcı hesaplarının defterleştirilmesi.
                 /// </summary>
                 /// <param name="ledgerId">Defter ID'si.</param>
                 /// <param name="purchaseInvoice">Satın alma faturası nesnesi.</param>
                 /// <param name="purchaseInvoiceExpenses">Satın alma faturası masrafları listesi.</param>
                 /// <returns>Oluşturulan defter kayıtlarının listesi.</returns>
        private List<LedgerEntry> RegisterOtherVendorLedgerization(int ledgerId, PurchaseInvoice purchaseInvoice, List<PurchaseInvoiceExpense> purchaseInvoiceExpenses)
        {
            List<LedgerEntry> ledgerEntries = new List<LedgerEntry>();
            int lineNo = 1;

            foreach (var item in purchaseInvoiceExpenses)
            {
                ledgerEntries.Add(new LedgerEntry
                {
                    LedgerId = ledgerId,
                    PartnerId = item.PartnerId,
                    LineNo = lineNo++,
                    AccountId = item.PartnerId, // Partner hesabı kullanılmalı
                    Description = $"{purchaseInvoice.InvoiceNo} - Diğer Satıcı Masrafı",
                    Debit = 0,
                    Credit =item.Amount,
                });
            }
            return ledgerEntries;
        }
    }
}