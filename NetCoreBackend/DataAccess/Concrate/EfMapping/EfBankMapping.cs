using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrate.EfMapping
{
    public class EfBankMapping : EfBaseEntityMapping<Bank>
    {
        public override void Configure(EntityTypeBuilder<Bank> builder)
        {
            base.Configure(builder);

            var defaultDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new Bank { Id = 1, Name = "İş Bankası", Created = defaultDate, Modified = defaultDate },
                new Bank { Id = 2, Name = "Ziraat Bankası", Created = defaultDate, Modified = defaultDate },
                new Bank { Id = 3, Name = "TEB", Created = defaultDate, Modified = defaultDate },
                new Bank { Id = 4, Name = "Raifaisen Bank", Created = defaultDate, Modified = defaultDate },
                new Bank { Id = 5, Name = "Procredit Bank", Created = defaultDate, Modified = defaultDate }
            );


        }
    }
}