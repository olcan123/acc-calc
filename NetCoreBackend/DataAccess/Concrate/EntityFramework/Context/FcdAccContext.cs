using System.Reflection;
using Core.Entities.Concrete;
using Entities.Concrate;
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
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

                var configuration = builder.Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException($"Connection string 'DefaultConnection' missing for {environment}!");
                }

                optionsBuilder.UseNpgsql(connectionString);
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressPartner> AddressPartners { get; set; }
        public DbSet<AddressWarehouse> AddressWarehouses { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankAccountCompany> BankAccountCompanies { get; set; }
        public DbSet<BankAccountPartner> BankAccountPartners { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactPartner> ContactPartners { get; set; }
        public DbSet<ContactWarehouse> ContactWarehouses { get; set; }
        public DbSet<ContactDetail> ContactDetails { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<BusinessPartner> BusinessPartners { get; set; }
        public DbSet<Employee> Employees { get; set; }


    }
}
