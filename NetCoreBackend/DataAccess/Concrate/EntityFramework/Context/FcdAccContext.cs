using System.Reflection;
using Core.Entities.Concrete;
using Entities.Concrate;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Concrate.EntityFramework.Context
{
    public class FcdAccContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

                var configuration = builder.Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException($"Connection string 'DefaultConnection' missing for {environment}!");
                }

                optionsBuilder.UseSqlServer(connectionString);
                optionsBuilder.UseLinqToDB();
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }


        //SECTION AUTHENTICATION
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }


        //SECTION COMPANY
        public DbSet<Company> Companies { get; set; }

        //SECTION ADDRESS
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressPartner> AddressPartners { get; set; }
        public DbSet<AddressWarehouse> AddressWarehouses { get; set; }

        //SECTION WAREHOUSE
        public DbSet<Warehouse> Warehouses { get; set; }

        //SECTION BANKING
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankAccountCompany> BankAccountCompanies { get; set; }
        public DbSet<BankAccountPartner> BankAccountPartners { get; set; }

        //SECTION PARTNER
        public DbSet<Partner> Partners { get; set; }

        //SECTION CONTACT
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactPartner> ContactPartners { get; set; }
        public DbSet<ContactWarehouse> ContactWarehouses { get; set; }
        public DbSet<ContactDetail> ContactDetails { get; set; }


        // SECTION - Product (Ana Tablolar)
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<Barcode> Barcodes { get; set; }
        public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
        public DbSet<Vat> Vats { get; set; }

        // SECTION - Product (Opsiyonel / Geliştirme Bekliyor)
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductDocument> ProductDocuments { get; set; }

        // SECTION - Fatura ve Muhasebe
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<LedgerEntry> LedgerEntries { get; set; }
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public DbSet<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; }
        public DbSet<PurchaseInvoiceExpense> PurchaseInvoiceExpenses { get; set; }
        public DbSet<SaleInvoice> SaleInvoices { get; set; }
        public DbSet<SaleInvoiceLine> SaleInvoiceLines { get; set; }
    }
}
