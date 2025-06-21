using Core.DataAccess.TypeConfiguration;
using Entities.Concrate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace DataAccess.Concrate.EfMapping
{
    public class EfAccountMapping : EfBaseEntityMapping<Account>
    {
        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            base.Configure(builder);

            // Primary key
            builder.HasKey(a => a.Id);

            // Property configurations
            builder.Property(a => a.Code).IsRequired().HasMaxLength(20);
            builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
            builder.Property(a => a.Description).HasMaxLength(500);
            builder.Property(a => a.IsActive).HasDefaultValue(true);
<<<<<<< HEAD
     //       builder.Property(a => a.IsPostable).HasDefaultValue(true);
=======
      //      builder.Property(a => a.IsPostable).HasDefaultValue(true);
>>>>>>> dockerv2

            // Relationship configuration
            builder.HasOne(a => a.ParentAccount)
                  .WithMany(a => a.Children)
                  .HasForeignKey(a => a.ParentAccountId)
                  .OnDelete(DeleteBehavior.Restrict) // Prevent cascading delete
                  .IsRequired(false);

            // Seed data for accounts
            var defaultDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
    // === VARLIKLAR (ASSETS) === Normal Bakiye: Borç (Debit)
    new Account
    {
        Id = 1,
        Code = "1000",
        Name = "Paraja dhe ekuivalentët e parasë (Nakit ve Nakit Benzerleri)",
        Description = "Kasa, bankalar ve diğer hazır değerleri içerir.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = false,
        ParentAccountId = null,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account
    {
        Id = 2,
        Code = "1050",
        Name = "Investimet financiare afatshkurtra (Kısa Vadeli Finansal Yatırımlar)",
        Description = "Bir yıldan kısa sürede nakde çevrilecek veya alınıp satılacak finansal yatırımları içerir.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = false,
        ParentAccountId = null,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account
    {
        Id = 3,
        Code = "1100",
        Name = "Llogaritë e arkëtueshme tregtare dhe të tjera (Ticari ve Diğer Alacaklar)",
        Description = "İşletmenin ticari faaliyetlerinden kaynaklanan ve diğer çeşitli kısa vadeli alacaklarını içerir.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = false,
        ParentAccountId = null,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account
    {
        Id = 4,
        Code = "1150",
        Name = "Stoget (Stoklar)",
        Description = "Satılmak veya üretimde kullanılmak üzere elde tutulan ticari mallar, mamuller, yarı mamuller, hammaddeler vb. varlıkları içerir.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = false,
        ParentAccountId = null,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account
    {
        Id = 5,
        Code = "1200",
        Name = "Pasuritë e tjera afatshkurtra (Diğer Dönen Varlıklar)",
        Description = "Yukarıdaki gruplara girmeyen diğer dönen varlık kalemlerini içerir (Gelir tahakkukları, peşin ödenmiş giderler vb.).",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = false,
        ParentAccountId = null,
        Created = defaultDate,
        Modified = defaultDate
    },
        new Account
        {
            Id = 6,
            Code = "1300",
            Name = "Investimet në pjesëmarrje (İştiraklerdeki Yatırımlar)",
            Description = "Başka işletmelerde önemli etki sahibi olunan uzun vadeli iştirak yatırımlarını içerir.",
            AccountType = AccountTypeOption.Asset,
            NormalBalance = NormalBalanceOption.Debit,
            IsActive = true,
            IsPostable = false,
            ParentAccountId = null,
            Created = defaultDate,
            Modified = defaultDate
        },
    new Account
    {
        Id = 7,
        Code = "1350",
        Name = "Investimet në filiale (Bağlı Ortaklıklardaki Yatırımlar)",
        Description = "Başka işletmeler üzerinde kontrol gücü sahibi olunan uzun vadeli bağlı ortaklık yatırımlarını içerir.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = false,
        ParentAccountId = null,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account
    {
        Id = 8,
        Code = "1400",
        Name = "Investimet financiare afatgjata (Uzun Vadeli Finansal Yatırımlar)",
        Description = "Bir yıldan uzun süreyle elde tutulacak diğer finansal yatırımları içerir.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = false,
        ParentAccountId = null,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account
    {
        Id = 9,
        Code = "1450",
        Name = "Prona, pajisjet dhe impiantet (Maddi Duran Varlıklar)",
        Description = "İşletme faaliyetlerinde kullanılmak üzere elde tutulan arsa, bina, makine, teçhizat gibi maddi duran varlıkları içerir.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = false,
        ParentAccountId = null,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account
    {
        Id = 10,
        Code = "1500",
        Name = "Prona investuese (Yatırım Amaçlı Gayrimenkuller)",
        Description = "Kira geliri elde etmek veya değer artışı sağlamak amacıyla elde tutulan gayrimenkulleri içerir.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = false,
        ParentAccountId = null,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account
    {
        Id = 11,
        Code = "1550",
        Name = "Investimet në vijim (Yapılmakta Olan Yatırımlar)",
        Description = "Henüz tamamlanmamış ve aktifleştirilmemiş maddi duran varlık yatırımlarını içerir.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = false,
        ParentAccountId = null,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account
    {
        Id = 12,
        Code = "1600",
        Name = "Pasuritë e paprekshme (Maddi Olmayan Duran Varlıklar)",
        Description = "Fiziksel varlığı olmayan ancak işletmeye fayda sağlayan haklar, şerefiye, patent gibi varlıkları içerir.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = false,
        ParentAccountId = null,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account
    {
        Id = 13,
        Code = "1650",
        Name = "Pasuritë e shtyra tatimore (Ertelenmiş Vergi Varlıkları)",
        Description = "Gelecekte vergi matrahından indirilebilecek veya daha az vergi ödenmesini sağlayacak unsurlardan kaynaklanan varlıkları içerir.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = false,
        ParentAccountId = null,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account
    {
        Id = 14,
        Code = "1700",
        Name = "Pasuritë e tjera afatgjata (Diğer Uzun Vadeli Varlıklar)",
        Description = "Yukarıdaki duran varlık gruarına girmeyen diğer uzun vadeli varlıkları içerir.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = false,
        ParentAccountId = null,
        Created = defaultDate,
        Modified = defaultDate
    },

                // === YÜKÜMLÜLÜKLER (LIABILITIES) === Normal Bakiye: Alacak (Credit)
                new Account
                {
                    Id = 15,
                    Code = "2000",
                    Name = "Mbitërheqja bankare",
                    Description = "Banka Kredileri (Nakit Avansları) / Bank Overdraft",
                    AccountType = AccountTypeOption.Liability,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 16,
                    Code = "2050",
                    Name = "Llogaritë e pagueshme tregtare dhe të tjera",
                    Description = "Ticari ve Diğer Borçlar (Trade and Other Payables)",
                    AccountType = AccountTypeOption.Liability,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 17,
                    Code = "2100",
                    Name = "Kreditë dhe huatë, pjesa afatshkurtër",
                    Description = "Krediler ve Borçlar, Kısa Vadeli Kısım (Loans and Borrowings, Short-term Portion)",
                    AccountType = AccountTypeOption.Liability,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 18,
                    Code = "2150",
                    Name = "Interesi i pagueshëm",
                    Description = "Ödenecek Faiz (Interest Payable)",
                    AccountType = AccountTypeOption.Liability,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 19,
                    Code = "2200",
                    Name = "Tatimin në fitim i pagueshëm",
                    Description = "Ödenecek Kurumlar Vergisi (Income Tax Payable)",
                    AccountType = AccountTypeOption.Liability,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 20,
                    Code = "2250",
                    Name = "Provizionet afatshkurta",
                    Description = "Kısa Vadeli Karşılıklar (Short-term Provisions)",
                    AccountType = AccountTypeOption.Liability,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 21,
                    Code = "2300",
                    Name = "Detyrimet ndaj lizingut, pjesa afatshkurtër",
                    Description = "Finansal Kiralama Yükümlülükleri, Kısa Vadeli Kısım (Lease Liabilities, Short-term Portion)",
                    AccountType = AccountTypeOption.Liability,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 22,
                    Code = "2350",
                    Name = "Detyrimet e tjera afatshkurtra",
                    Description = "Diğer Kısa Vadeli Yükümlülükler (Other Short-term Liabilities)",
                    AccountType = AccountTypeOption.Liability,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 23,
                    Code = "2400",
                    Name = "Kreditë dhe huatë, pjesa afatgjatë",
                    Description = "Krediler ve Borçlar, Uzun Vadeli Kısım (Loans and Borrowings, Long-term Portion)",
                    AccountType = AccountTypeOption.Liability,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 24,
                    Code = "2450",
                    Name = "Provizionet afatgjata",
                    Description = "Uzun Vadeli Karşılıklar (Long-term Provisions)",
                    AccountType = AccountTypeOption.Liability,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 25,
                    Code = "2500",
                    Name = "Detyrimet ndaj lizingut, pjesa afatgjatë",
                    Description = "Finansal Kiralama Yükümlülükleri, Uzun Vadeli Kısım (Lease Liabilities, Long-term Portion)",
                    AccountType = AccountTypeOption.Liability,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 26,
                    Code = "2550",
                    Name = "Detyrimet e shtyra tatimore",
                    Description = "Ertelenmiş Vergi Yükümlülükleri (Deferred Tax Liabilities)",
                    AccountType = AccountTypeOption.Liability,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 27,
                    Code = "2600",
                    Name = "Detyrimet e tjera afatgjata",
                    Description = "Diğer Uzun Vadeli Yükümlülükler (Other Long-term Liabilities)",
                    AccountType = AccountTypeOption.Liability,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },

                // === ÖZKAYNAK (EQUITY) === Normal Bakiye: Alacak (Credit)
                new Account
                {
                    Id = 28,
                    Code = "3000",
                    Name = "Kapitali aksionar",
                    Description = "Sermaye (Share Capital)",
                    AccountType = AccountTypeOption.Equity,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 29,
                    Code = "3100",
                    Name = "Fitimet e mbajtura",
                    Description = "Birikmiş Karlar (Retained Earnings)",
                    AccountType = AccountTypeOption.Equity,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 30,
                    Code = "3200",
                    Name = "Rezervat e tjera",
                    Description = "Diğer Yedekler (Other Reserves)",
                    AccountType = AccountTypeOption.Equity,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },

                // === GELİRLER (REVENUE) === Normal Bakiye: Alacak (Credit)
                new Account
                {
                    Id = 31,
                    Code = "4000",
                    Name = "Të Hyrat",
                    Description = "Gelirler / Hasılat (Revenue)",
                    AccountType = AccountTypeOption.Revenue,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 32,
                    Code = "4100",
                    Name = "Të ardhura tjera",
                    Description = "Diğer Gelirler (Other Income)",
                    AccountType = AccountTypeOption.Revenue,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 33,
                    Code = "4200",
                    Name = "Të ardhurat financiare",
                    Description = "Finansal Gelirler (Financial Income)",
                    AccountType = AccountTypeOption.Revenue,
                    NormalBalance = NormalBalanceOption.Credit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },

                // === GİDERLER (EXPENSE) === Normal Bakiye: Borç (Debit)
                new Account
                {
                    Id = 34,
                    Code = "5000",
                    Name = "Kostoja e shitjes",
                    Description = "Satışların Maliyeti (Cost of Sales)",
                    AccountType = AccountTypeOption.Expense,
                    NormalBalance = NormalBalanceOption.Debit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 35,
                    Code = "5100",
                    Name = "Shpenzimet e shpërndarjes",
                    Description = "Dağıtım Giderleri (Distribution Expenses)",
                    AccountType = AccountTypeOption.Expense,
                    NormalBalance = NormalBalanceOption.Debit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 36,
                    Code = "5200",
                    Name = "Shpenzimet administrative",
                    Description = "İdari Giderler (Administrative Expenses)",
                    AccountType = AccountTypeOption.Expense,
                    NormalBalance = NormalBalanceOption.Debit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 37,
                    Code = "5300",
                    Name = "Shpenzimet e tjera",
                    Description = "Diğer Giderler (Other Expenses)",
                    AccountType = AccountTypeOption.Expense,
                    NormalBalance = NormalBalanceOption.Debit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 38,
                    Code = "5400",
                    Name = "Shpenzimet financiare",
                    Description = "Finansal Giderler (Financial Expenses)",
                    AccountType = AccountTypeOption.Expense,
                    NormalBalance = NormalBalanceOption.Debit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },
                new Account
                {
                    Id = 39,
                    Code = "5500",
                    Name = "Shpenzimet e tatimit në fitim",
                    Description = "Kurumlar Vergisi Gideri (Income Tax Expense)",
                    AccountType = AccountTypeOption.Expense,
                    NormalBalance = NormalBalanceOption.Debit,
                    IsActive = true,
                    IsPostable = false,
                    ParentAccountId = null,
                    Created = defaultDate,
                    Modified = defaultDate
                },

    // Diğer hesaplar için seed verileri buraya ekleyebilirsiniz
    // ...

    // === Örnek alt hesaplar ===
    // NËNLLOGARITË E SHTUARA NGA PËRDORUESI (User-added sub-accounts)
    // Id-të e ri-caktuara duke filluar nga 40 / IsPostable vlerat sipas kodit të dhënë nga përdoruesi
    new Account // Nënllogari e "1000" (Id=1)
    {
        Id = 40, // Id e re
        Code = "1000.1000",
        Name = "Kasë (Kasa)",
        Description = "Kasada bulunan nakit para.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 1,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account // Nënllogari e "1000" (Id=1)
    {
        Id = 41, // Id e re
        Code = "1000.2000",
        Name = "Banka (Banka)",
        Description = "Banka hesapları.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 1,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account // Nënllogari e "1100" (Id=3)
    {
        Id = 42, // Id e re
        Code = "1100.1000",
        Name = "Llogaritë e arkëtueshme (Alacak Hesapları)",
        Description = "İşletmenin çeşitli faaliyetlerinden doğan alacakları.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 3,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account // Nënllogari e "1150" (Id=4)
    {
        Id = 43, // Id e re
        Code = "1150.1000",
        Name = "Mallra Tregtare (Ticari Mallar)", // Emri primar shqip, përkthimi turqisht në kllapa
        Description = "Satın alınıp üzerinde değişiklik yapılmadan satılan mallar.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 4,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account // Nënllogari e "1150" (Id=4)
    {
        Id = 44, // Id e re
        Code = "1150.2000",
        Name = "Lëndë e parë (Hammadde)",
        Description = "Üretimde kullanılacak temel hammaddeler.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 4,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account // Nënllogari e "1150" (Id=4)
    {
        Id = 45, // Id e re
        Code = "1150.3000",
        Name = "Produkte në proces (Yarı Mamuller)",
        Description = "Henüz üretim süreci tamamlanmamış ürünler.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 4,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account // Nënllogari e "1150" (Id=4)
    {
        Id = 46, // Id e re
        Code = "1150.4000",
        Name = "Produkte të gatshme (Mamuller)",
        Description = "Üretim süreci tamamlanmış, satışa hazır ürünler.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 4,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account // Nënllogari e "1200" (Id=5)
    {
        Id = 47, // Id e re
        Code = "1200.7008",
        Name = "TVSH e zbritshme 8% (İndirilecek KDV %8)",
        Description = "%8 oranında alım ve giderler üzerinden hesaplanan ve indirilecek KDV.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 5,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account // Nënllogari e "1200" (Id=5)
    {
        Id = 48, // Id e re
        Code = "1200.7018",
        Name = "TVSH e zbritshme 18% (İndirilecek KDV %18)",
        Description = "%18 oranında alım ve giderler üzerinden hesaplanan ve indirilecek KDV.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 5,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account // Nënllogari e "1450" (Id=6)
    {
        Id = 49, // Id e re
        Code = "1450.1000",
        Name = "Nënshkrime dhe pajisje (Demirbaşlar ve Tesisat)", // Supozim: "Nënshkrime" = Demirbaşlar/Fixtures
        Description = "İşletme faaliyetlerinde kullanılan demirbaşlar, makine ve teçhizat.",
        AccountType = AccountTypeOption.Asset,
        NormalBalance = NormalBalanceOption.Debit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 9,
        Created = defaultDate,
        Modified = defaultDate
    },
    new Account // Nënllogari e "1450" (Id=6)
    {
        Id = 50, // Id e re
        Code = "1450.6000",
        Name = "Amortizimi i pajisjeve (Ekipman Amortismanı)",
        Description = "Pajisje ve demirbaşlar için ayrılan birikmiş amortisman.",
        AccountType = AccountTypeOption.ContraAsset, // Kujdes: Ky është një Kontra-Aktiv
        NormalBalance = NormalBalanceOption.Credit,  // Dhe ka bilanc normal Kredi
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 9,
        Created = defaultDate,
        Modified = defaultDate
    },

    new Account // Nënllogari e "2050" (Id=16)
    {
        Id = 51, // Id e re
        Code = "2050.1000",
        Name = "Furnitorët (Tedarikçiler)",
        Description = "İşletmenin ticari faaliyetlerinden doğan borçları.",
        AccountType = AccountTypeOption.Liability,
        NormalBalance = NormalBalanceOption.Credit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 16,
        Created = defaultDate,
        Modified = defaultDate
    },

    new Account // Nënllogari e "2200" (Id=19)
    {
        Id = 52, // Id e re
        Code = "2200.1000",
        Name = "Tatimi mbi fitimin (Kurumlar Vergisi)",
        Description = "İşletmenin kazançları üzerinden ödenecek kurumlar vergisi.",
        AccountType = AccountTypeOption.Liability,
        NormalBalance = NormalBalanceOption.Credit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 19,
        Created = defaultDate,
        Modified = defaultDate
    },

    new Account // Nënllogari e "2350" (Id=22)
    {
        Id = 53, // Id e re
        Code = "2350.1000",
        Name = "Detyrimet për Qira (Kira Yükümlülükleri)",
        Description = "Finansal kiralama sözleşmelerinden doğan yükümlülükler.",
        AccountType = AccountTypeOption.Liability,
        NormalBalance = NormalBalanceOption.Credit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 22,
        Created = defaultDate,
        Modified = defaultDate
    },

    new Account// Nënllogari e "2350" (Id=22)
    {
        Id = 54, // Id e re
        Code = "2350.1100",
        Name = "Detyrimet Tatim mbi Qirën (Kira Vergisi Yükümlülükleri)",
        Description = "Finansal kiralama sözleşmelerinden doğan vergi yükümlülükleri.",
        AccountType = AccountTypeOption.Liability,
        NormalBalance = NormalBalanceOption.Credit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 22,
        Created = defaultDate,
        Modified = defaultDate
    },

    new Account // Nënllogari e "2350" (Id=22)
    {
        Id = 55, // Id e re
        Code = "2350.2000",
        Name = "Detyrimet për Paga ndaj Personelit (Personel Ücret Yükümlülükleri)",
        Description = "İşletmenin personeline ödenecek ücret ve maaşlardan doğan yükümlülükler.",
        AccountType = AccountTypeOption.Liability,
        NormalBalance = NormalBalanceOption.Credit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 22,
        Created = defaultDate,
        Modified = defaultDate
    },

    new Account // Nënllogari e "2350" (Id=22)
    {
        Id = 56, // Id e re
        Code = "2350.2100",
        Name = "Detyrimet për Tatimin mbi Paga (Ücret Vergisi Yükümlülükleri)",
        Description = "İşletmenin personeline ödenecek ücret ve maaşlardan doğan vergi yükümlülükleri.",
        AccountType = AccountTypeOption.Liability,
        NormalBalance = NormalBalanceOption.Credit,
        IsActive = true,
        IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
        ParentAccountId = 22,
        Created = defaultDate,
        Modified = defaultDate
    },

        new Account // Nënllogari e "2350" (Id=22)
        {
            Id = 57, // Id e re
            Code = "2350.2200",
            Name = "Detyrimet për Kontributer (Sosyal Güvenlik Prim Yükümlülükleri)",
            Description = "İşletmenin sosyal güvenlik primlerinden doğan yükümlülükler.",
            AccountType = AccountTypeOption.Liability,
            NormalBalance = NormalBalanceOption.Credit,
            IsActive = true,
            IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
            ParentAccountId = 22,
            Created = defaultDate,
            Modified = defaultDate
        },

        new Account // Nënllogari e "2350" (Id=22)
        {
            Id = 58, // Id e re
            Code = "2350.7000",
            Name = "Detyrimet per Dogan (Gümrük Yükümlülükleri)",
            Description = "İşletmenin gümrük vergilerinden doğan yükümlülükler.",
            AccountType = AccountTypeOption.Liability,
            NormalBalance = NormalBalanceOption.Credit,
            IsActive = true,
            IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
            ParentAccountId = 22,
            Created = defaultDate,
            Modified = defaultDate
        },

        new Account // Nënllogari e "2350" (Id=22)
        {
            Id = 59, // Id e re
            Code = "2350.7008",
            Name = "Detyrimet për Doganë TVSH 8% (Gümrük KDV Yükümlülükleri %8)",
            Description = "İşletmenin gümrük vergilerinden doğan yükümlülükler.",
            AccountType = AccountTypeOption.Liability,
            NormalBalance = NormalBalanceOption.Credit,
            IsActive = true,
            IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
            ParentAccountId = 22,
            Created = defaultDate,
            Modified = defaultDate
        },

        new Account // Nënllogari e "2350" (Id=22)
        {
            Id = 60, // Id e re
            Code = "2350.7018",
            Name = "Detyrimet për Doganë TVSH 18% (Gümrük KDV Yükümlülükleri %18)",
            Description = "İşletmenin gümrük vergilerinden doğan yükümlülükler.",
            AccountType = AccountTypeOption.Liability,
            NormalBalance = NormalBalanceOption.Credit,
            IsActive = true,
            IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
            ParentAccountId = 22,
            Created = defaultDate,
            Modified = defaultDate
        },

        new Account // Nënllogari e "2350" (Id=22)
        {
            Id = 61, // Id e re
            Code = "2350.7100",
            Name = "Detyrimet per Akciza (ÖTV Yükümlülükleri)",
            Description = "İşletmenin akçiz vergilerinden doğan yükümlülükler.",
            AccountType = AccountTypeOption.Liability,
            NormalBalance = NormalBalanceOption.Credit,
            IsActive = true,
            IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
            ParentAccountId = 22,
            Created = defaultDate,
            Modified = defaultDate
        },
        new Account // Nënllogari e "2350" (Id=22)
        {
            Id = 62, // Id e re
            Code = "2350.6218",
            Name = "Detyrimet për Llogaritje TVSH 18% (KDV Hesap Yükümlülükleri %18)",
            Description = "İşletmenin KDV yükümlülükleri.",
            AccountType = AccountTypeOption.Liability,
            NormalBalance = NormalBalanceOption.Credit,
            IsActive = true,
            IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
            ParentAccountId = 22,
            Created = defaultDate,
            Modified = defaultDate
        },

        new Account // Nënllogari e "2350" (Id=22)
        {
            Id = 63, // Id e re
            Code = "2350.6208",
            Name = "Detyrimet për Llogaritje TVSH 8% (KDV Hesap Yükümlülükleri %8)",
            Description = "İşletmenin KDV yükümlülükleri.",
            AccountType = AccountTypeOption.Liability,
            NormalBalance = NormalBalanceOption.Credit,
            IsActive = true,
            IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
            ParentAccountId = 22,
            Created = defaultDate,
            Modified = defaultDate
        },

        new Account // Nënllogari e "2350" (Id=22)
        {
            Id = 64, // Id e re
            Code = "2350.6200",
            Name = "Detyrimet për TVSH 0% (KDV Hesap Yükümlülükleri %0)",
            Description = "İşletmenin KDV yükümlülükleri.",
            AccountType = AccountTypeOption.Liability,
            NormalBalance = NormalBalanceOption.Credit,
            IsActive = true,
            IsPostable = true, // Sipas kodit të dhënë nga përdoruesi
            ParentAccountId = 22,
            Created = defaultDate,
            Modified = defaultDate
        },


        // === NËNLLOGARITË E REJA PËR KAPITALIN (Nën "3000") ===

        // 1. Başlangıç sermayesi / Kapitali Fillestar
        new Account
        {
            Id = 65, // Id e re unike (Id-ja e fundit e mëparshme ishte 64)
            Code = "3000.1000",
            Name = "Kapitali Fillestar (Başlangıç Sermayesi)",
            Description = "İşletmenin kuruluşunda veya sonraki sermaye artırımlarında ortaklar tarafından taahhüt edilen ve ödenen sermaye tutarı.",
            AccountType = AccountTypeOption.Equity,
            NormalBalance = NormalBalanceOption.Credit,
            IsActive = true,
            IsPostable = true, // Kjo llogari pranon regjistrime direkte
            ParentAccountId = 28, // Id-ja e llogarisë "3000 Kapitali aksionar"
            Created = defaultDate,
            Modified = defaultDate
        },

        // 2. Bu yılın karı/zararı / Fitimi/Humbja e Vitit Rrjedhës
        new Account
        {
            Id = 66, // Id e re unike
            Code = "3100.1000",
            Name = "Fitimi/Humbja e Vitit Rrjedhës (Bu Yılın Karı/Zararı)",
            Description = "İşletmenin içinde bulunulan hesap dönemine ait net kar veya zarar tutarı. Yıl sonunda bu hesap kapanarak birikmiş karlara veya zararlara aktarılır.",
            AccountType = AccountTypeOption.Equity, // Mund të jetë edhe 'RetainedEarnings' ose një tip specifik
            NormalBalance = NormalBalanceOption.Credit, // Fitimi e rrit kapitalin (Kredi), Humbja e zvogëlon (Debi)
            IsActive = true,
            IsPostable = true,
            ParentAccountId = 29,
            Created = defaultDate,
            Modified = defaultDate
        },

        // 3. Geçen yılların kar ve zararı / Fitimet/Humbjet e Mbartura
        new Account
        {
            Id = 67, // Id e re unike
            Code = "3000.1500",
            Name = "Fitimet/Humbjet e Mbartura (Geçmiş Yıllar Karları/Zararları)",
            Description = "İşletmenin önceki hesap dönemlerinden devreden ve henüz dağıtılmamış veya kapatılmamış birikmiş kar veya zarar tutarları.",
            AccountType = AccountTypeOption.Equity, // Ose 'RetainedEarnings'
            NormalBalance = NormalBalanceOption.Credit,
            IsActive = true,
            IsPostable = true,
            ParentAccountId = 29,
            Created = defaultDate,
            Modified = defaultDate
        },

           new Account
           {
               Id = 68, // Id e re unike
               Code = "4000.1000",
               Name = "Të ardhura nga shitja e mallrave (Ticari Mal Satış Gelirleri)",
               Description = "Ticari malların satışından elde edilen gelirler.",
               AccountType = AccountTypeOption.Revenue,
               NormalBalance = NormalBalanceOption.Credit,
               IsActive = true,
               IsPostable = true,
               ParentAccountId = 31,
               Created = defaultDate,
               Modified = defaultDate
           },
        new Account
        {
            Id = 69, // Id e re unike
            Code = "4000.2000",
            Name = "Të ardhura nga ofrimi i shërbimeve (Hizmet Satış Gelirleri)",
            Description = "Sunulan hizmetlerden elde edilen gelirler.",
            AccountType = AccountTypeOption.Revenue,
            NormalBalance = NormalBalanceOption.Credit,
            IsActive = true,
            IsPostable = true,
            ParentAccountId = 31,
            Created = defaultDate,
            Modified = defaultDate
        }


            );
        }
    }
}
