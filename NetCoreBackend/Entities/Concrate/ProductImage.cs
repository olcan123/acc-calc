using Core.Entities;
using Entities.Concrate;

public class ProductImage : BaseEntity
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public string Url { get; set; }             // Görselin bulunduğu URL (veya dosya yolu)
    public string Label { get; set; }           // Örn: “Ön görünüm”, “Ambalaj fotoğrafı”
    public bool IsMain { get; set; }            // Ana görsel mi?
    public int DisplayOrder { get; set; }       // Görsel sıralaması
    public DateTime UploadedAt { get; set; }
}
