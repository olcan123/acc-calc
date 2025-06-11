using Core.Entities;
using Entities.Concrate;

public class Partner : BaseEntity
{
    public Partner()
    {
        BankAccountPartners = new HashSet<BankAccountPartner>();
        ContactPartners = new HashSet<ContactPartner>();
        AddressPartners = new HashSet<AddressPartner>();
        PurchaseInvoices = new HashSet<PurchaseInvoice>();
        LedgerEntries = new HashSet<LedgerEntry>();
        PurchaseInvoiceExpenses = new HashSet<PurchaseInvoiceExpense>();
        SaleInvoices = new HashSet<SaleInvoice>(); // Assuming SaleInvoice is defined elsewhere
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string TradeName { get; set; }
    public PartnerType PartnerType { get; set; } // "Individual", "Company"
    public BusinessPartnerType? BusinessPartnerType { get; set; }
    public string IdentityNumber { get; set; }
    public string VatNumber { get; set; }

    // Navigation Properties
    public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; }
    public ICollection<PurchaseInvoiceExpense> PurchaseInvoiceExpenses { get; }
    public ICollection<BankAccountPartner> BankAccountPartners { get; set; }
    public ICollection<ContactPartner> ContactPartners { get; set; }
    public ICollection<AddressPartner> AddressPartners { get; set; }
    public ICollection<LedgerEntry> LedgerEntries { get; set; }
    public ICollection<SaleInvoice> SaleInvoices { get; set; } // Assuming SaleInvoice is defined elsewhere
}


public enum PartnerType
{
    Business,
    Individual,
    Employee,
}

public enum BusinessPartnerType
{
    Supplier,
    Customer,
    Both
}