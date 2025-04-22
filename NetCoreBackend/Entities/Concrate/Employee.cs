using Core.Entities;

public class Employee : BaseEntity
{
    public int Id { get; set; }
    public int PartnerId { get; set; }
    public string EmployeeNumber { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation Property
    public Partner Partner { get; set; }
}
