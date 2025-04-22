using Core.Entities;
using Entities.Concrate;

public class Partner : BaseEntity
{
    public Partner()
    {
        BankAccounts = new HashSet<BankAccount>();
        Contacts = new HashSet<Contact>();
        Addresses = new HashSet<Address>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public PartnerType PartnerType { get; set; } // "Individual", "Company"
    public string IdentityNumber { get; set; }

    // Navigation Properties
    public Employee Employee { get; set; }
    public BusinessPartner BusinessPartner { get; set; }
    public ICollection<BankAccount> BankAccounts { get; set; }
    public ICollection<Contact> Contacts { get; set; }
    public ICollection<Address> Addresses { get; set; }
}


public enum PartnerType
{
    Individual,
    Company
}