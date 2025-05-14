using Core.Entities;
using Entities.Concrate;

public class ProductDocument : BaseEntity
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public string FileName { get; set; }         // Örn: user_manual.pdf
    public string FileUrl { get; set; }          // Dosyanın tam adresi
    public string DocumentType { get; set; }     // Örn: Kullanım Kılavuzu, Teknik Şartname
    public DateTime UploadedAt { get; set; }
}
