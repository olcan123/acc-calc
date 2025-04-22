using Core.Entities;
using Entities.Concrate;

public class BusinessPartner : BaseEntity
{
    public int Id { get; set; }
    public int PartnerId { get; set; }
    public string VatNumber { get; set; }
    public string PartnerRole { get; set; } // "Customer", "Supplier", "Both"

    //Ilerde Eklenecek Zoellikler
    // public decimal CreditLimit { get; set; } = 0;
    // public string PaymentTerms { get; set; }
    // public string SupplyTerms { get; set; }

    // Navigation Property
    public Partner Partner { get; set; }
}
