using Core.Entities;
using Entities.Concrate;

public class Partner : BaseEntity
{
    public Partner()
    {
        BankAccountPartners = new HashSet<BankAccountPartner>();
        ContactPartners = new HashSet<ContactPartner>();
        AddressPartners = new HashSet<AddressPartner>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public PartnerType PartnerType { get; set; } // "Individual", "Company"
    public string IdentityNumber { get; set; }

    // Navigation Properties
    public Employee Employee { get; set; }
    public BusinessPartner BusinessPartner { get; set; }
    public ICollection<BankAccountPartner> BankAccountPartners { get; set; }
    public ICollection<ContactPartner> ContactPartners { get; set; }
    public ICollection<AddressPartner> AddressPartners { get; set; }
}


public enum PartnerType
{
    Individual,
    Company
}