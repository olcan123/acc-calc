using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ParentAccountId = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsPostable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    NormalBalance = table.Column<int>(type: "integer", nullable: true),
                    AccountType = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Accounts_ParentAccountId",
                        column: x => x.ParentAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Street = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TradeName = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UidNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    VatNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Period = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ledgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentType = table.Column<short>(type: "smallint", nullable: false, comment: "1=PurchaseInvoice, 2=SalesInvoice, 3=Payment, 4=Receipt, 5=Journal"),
                    DocumentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReferenceNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ledgers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TradeName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PartnerType = table.Column<int>(type: "integer", nullable: false),
                    BusinessPartnerType = table.Column<int>(type: "integer", nullable: true),
                    IdentityNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    VatNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitOfMeasures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Abbreviation = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOfMeasures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserOperationClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    OperationClaimId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOperationClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: true),
                    Status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Rate = table.Column<float>(type: "real", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CompanyId1 = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warehouses_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Warehouses_Companies_CompanyId1",
                        column: x => x.CompanyId1,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContactDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContactId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactDetails_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BankId = table.Column<int>(type: "integer", nullable: false),
                    BranchName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AccountNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IBAN = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SwiftCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CurrencyId = table.Column<int>(type: "integer", nullable: false),
                    BankId1 = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Banks_BankId1",
                        column: x => x.BankId1,
                        principalTable: "Banks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BankAccounts_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AddressPartners",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "integer", nullable: false),
                    PartnerId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressPartners", x => new { x.AddressId, x.PartnerId });
                    table.ForeignKey(
                        name: "FK_AddressPartners_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AddressPartners_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactPartners",
                columns: table => new
                {
                    ContactId = table.Column<int>(type: "integer", nullable: false),
                    PartnerId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactPartners", x => new { x.ContactId, x.PartnerId });
                    table.ForeignKey(
                        name: "FK_ContactPartners_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactPartners_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LedgerEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LedgerId = table.Column<int>(type: "integer", nullable: false),
                    PartnerId = table.Column<int>(type: "integer", nullable: true),
                    LineNo = table.Column<int>(type: "integer", nullable: false),
                    AccountId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Debit = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    Credit = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    AccountId1 = table.Column<int>(type: "integer", nullable: true),
                    CurrencyId = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LedgerEntries_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LedgerEntries_Accounts_AccountId1",
                        column: x => x.AccountId1,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LedgerEntries_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LedgerEntries_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LedgerEntries_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LedgerId = table.Column<int>(type: "integer", nullable: false),
                    InvoiceType = table.Column<int>(type: "integer", nullable: false),
                    PartnerId = table.Column<int>(type: "integer", nullable: false),
                    VendorAccountId = table.Column<int>(type: "integer", nullable: false),
                    InvoiceNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImportPartnerDocNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ImportPartnerDocDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CurrencyId = table.Column<int>(type: "integer", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    Status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CashPaymentAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_Accounts_VendorAccountId",
                        column: x => x.VendorAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoices_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PurchaseAccountId = table.Column<int>(type: "integer", nullable: false),
                    SaleAccountId = table.Column<int>(type: "integer", nullable: false),
                    CustomsTaxRate = table.Column<float>(type: "numeric(5,2)", nullable: false),
                    ExciseTaxRate = table.Column<float>(type: "numeric(5,2)", nullable: false),
                    VatId = table.Column<int>(type: "integer", nullable: false),
                    ProductType = table.Column<int>(type: "integer", nullable: false),
                    UnitOfMeasureId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Accounts_PurchaseAccountId",
                        column: x => x.PurchaseAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Accounts_SaleAccountId",
                        column: x => x.SaleAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_UnitOfMeasures_UnitOfMeasureId",
                        column: x => x.UnitOfMeasureId,
                        principalTable: "UnitOfMeasures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Vats_VatId",
                        column: x => x.VatId,
                        principalTable: "Vats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AddressWarehouses",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "integer", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressWarehouses", x => new { x.AddressId, x.WarehouseId });
                    table.ForeignKey(
                        name: "FK_AddressWarehouses_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AddressWarehouses_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactWarehouses",
                columns: table => new
                {
                    ContactId = table.Column<int>(type: "integer", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactWarehouses", x => new { x.ContactId, x.WarehouseId });
                    table.ForeignKey(
                        name: "FK_ContactWarehouses_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactWarehouses_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccountCompanies",
                columns: table => new
                {
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountCompanies", x => new { x.BankAccountId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_BankAccountCompanies_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccountCompanies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccountPartners",
                columns: table => new
                {
                    BankAccountId = table.Column<int>(type: "integer", nullable: false),
                    PartnerId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccountPartners", x => new { x.BankAccountId, x.PartnerId });
                    table.ForeignKey(
                        name: "FK_BankAccountPartners_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankAccountPartners_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseInvoiceExpenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseInvoiceId = table.Column<int>(type: "integer", nullable: false),
                    PartnerId = table.Column<int>(type: "integer", nullable: false),
                    VendorAccountId = table.Column<int>(type: "integer", nullable: false),
                    PartnerInvoiceNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PartnerInvoiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpenseType = table.Column<int>(type: "integer", nullable: false),
                    RevaluationAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AmountFc = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    IsPaid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CashPaymentAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseInvoiceExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceExpenses_Accounts_VendorAccountId",
                        column: x => x.VendorAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceExpenses_Partners_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceExpenses_PurchaseInvoices_PurchaseInvoiceId",
                        column: x => x.PurchaseInvoiceId,
                        principalTable: "PurchaseInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Barcodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barcodes", x => x.Id);
                    table.CheckConstraint("CK_Barcodes_Id_Positive", "\"Id\" > 0");
                    table.ForeignKey(
                        name: "FK_Barcodes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => new { x.ProductId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ProductCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    FileUrl = table.Column<string>(type: "text", nullable: true),
                    DocumentType = table.Column<string>(type: "text", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDocuments_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true),
                    Label = table.Column<string>(type: "text", nullable: true),
                    IsMain = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    Category = table.Column<byte>(type: "smallint", nullable: false),
                    Side = table.Column<byte>(type: "smallint", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPrices_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseInvoiceLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseInvoiceId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    WarehouseId = table.Column<int>(type: "integer", nullable: false),
                    UnitOfMeasureId = table.Column<int>(type: "integer", nullable: false),
                    VatId = table.Column<int>(type: "integer", nullable: false),
                    PurchaseAccountId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    ExpenseAmount = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    DiscountRate = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    ExciseTaxRate = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    ExciseTaxAmount = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    CustomsRate = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    CustomsAmount = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    RevaluationAmount = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    VatTaxRate = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    VatTaxAmount = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    CostAmount = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,6)", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseInvoiceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceLines_Accounts_PurchaseAccountId",
                        column: x => x.PurchaseAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceLines_PurchaseInvoices_PurchaseInvoiceId",
                        column: x => x.PurchaseInvoiceId,
                        principalTable: "PurchaseInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceLines_UnitOfMeasures_UnitOfMeasureId",
                        column: x => x.UnitOfMeasureId,
                        principalTable: "UnitOfMeasures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceLines_Vats_VatId",
                        column: x => x.VatId,
                        principalTable: "Vats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseInvoiceLines_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountType", "Code", "Description", "IsActive", "Modified", "Name", "NormalBalance", "ParentAccountId" },
                values: new object[,]
                {
                    { 1, 1, "1000", "Kasa, bankalar ve diğer hazır değerleri içerir.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Paraja dhe ekuivalentët e parasë (Nakit ve Nakit Benzerleri)", 1, null },
                    { 2, 1, "1050", "Bir yıldan kısa sürede nakde çevrilecek veya alınıp satılacak finansal yatırımları içerir.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Investimet financiare afatshkurtra (Kısa Vadeli Finansal Yatırımlar)", 1, null },
                    { 3, 1, "1100", "İşletmenin ticari faaliyetlerinden kaynaklanan ve diğer çeşitli kısa vadeli alacaklarını içerir.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Llogaritë e arkëtueshme tregtare dhe të tjera (Ticari ve Diğer Alacaklar)", 1, null },
                    { 4, 1, "1150", "Satılmak veya üretimde kullanılmak üzere elde tutulan ticari mallar, mamuller, yarı mamuller, hammaddeler vb. varlıkları içerir.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Stoget (Stoklar)", 1, null },
                    { 5, 1, "1200", "Yukarıdaki gruplara girmeyen diğer dönen varlık kalemlerini içerir (Gelir tahakkukları, peşin ödenmiş giderler vb.).", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pasuritë e tjera afatshkurtra (Diğer Dönen Varlıklar)", 1, null },
                    { 6, 1, "1300", "Başka işletmelerde önemli etki sahibi olunan uzun vadeli iştirak yatırımlarını içerir.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Investimet në pjesëmarrje (İştiraklerdeki Yatırımlar)", 1, null },
                    { 7, 1, "1350", "Başka işletmeler üzerinde kontrol gücü sahibi olunan uzun vadeli bağlı ortaklık yatırımlarını içerir.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Investimet në filiale (Bağlı Ortaklıklardaki Yatırımlar)", 1, null },
                    { 8, 1, "1400", "Bir yıldan uzun süreyle elde tutulacak diğer finansal yatırımları içerir.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Investimet financiare afatgjata (Uzun Vadeli Finansal Yatırımlar)", 1, null },
                    { 9, 1, "1450", "İşletme faaliyetlerinde kullanılmak üzere elde tutulan arsa, bina, makine, teçhizat gibi maddi duran varlıkları içerir.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Prona, pajisjet dhe impiantet (Maddi Duran Varlıklar)", 1, null },
                    { 10, 1, "1500", "Kira geliri elde etmek veya değer artışı sağlamak amacıyla elde tutulan gayrimenkulleri içerir.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Prona investuese (Yatırım Amaçlı Gayrimenkuller)", 1, null },
                    { 11, 1, "1550", "Henüz tamamlanmamış ve aktifleştirilmemiş maddi duran varlık yatırımlarını içerir.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Investimet në vijim (Yapılmakta Olan Yatırımlar)", 1, null },
                    { 12, 1, "1600", "Fiziksel varlığı olmayan ancak işletmeye fayda sağlayan haklar, şerefiye, patent gibi varlıkları içerir.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pasuritë e paprekshme (Maddi Olmayan Duran Varlıklar)", 1, null },
                    { 13, 1, "1650", "Gelecekte vergi matrahından indirilebilecek veya daha az vergi ödenmesini sağlayacak unsurlardan kaynaklanan varlıkları içerir.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pasuritë e shtyra tatimore (Ertelenmiş Vergi Varlıkları)", 1, null },
                    { 14, 1, "1700", "Yukarıdaki duran varlık gruarına girmeyen diğer uzun vadeli varlıkları içerir.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pasuritë e tjera afatgjata (Diğer Uzun Vadeli Varlıklar)", 1, null },
                    { 15, 2, "2000", "Banka Kredileri (Nakit Avansları) / Bank Overdraft", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mbitërheqja bankare", 2, null },
                    { 16, 2, "2050", "Ticari ve Diğer Borçlar (Trade and Other Payables)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Llogaritë e pagueshme tregtare dhe të tjera", 2, null },
                    { 17, 2, "2100", "Krediler ve Borçlar, Kısa Vadeli Kısım (Loans and Borrowings, Short-term Portion)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kreditë dhe huatë, pjesa afatshkurtër", 2, null },
                    { 18, 2, "2150", "Ödenecek Faiz (Interest Payable)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Interesi i pagueshëm", 2, null },
                    { 19, 2, "2200", "Ödenecek Kurumlar Vergisi (Income Tax Payable)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tatimin në fitim i pagueshëm", 2, null },
                    { 20, 2, "2250", "Kısa Vadeli Karşılıklar (Short-term Provisions)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Provizionet afatshkurta", 2, null },
                    { 21, 2, "2300", "Finansal Kiralama Yükümlülükleri, Kısa Vadeli Kısım (Lease Liabilities, Short-term Portion)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet ndaj lizingut, pjesa afatshkurtër", 2, null },
                    { 22, 2, "2350", "Diğer Kısa Vadeli Yükümlülükler (Other Short-term Liabilities)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet e tjera afatshkurtra", 2, null },
                    { 23, 2, "2400", "Krediler ve Borçlar, Uzun Vadeli Kısım (Loans and Borrowings, Long-term Portion)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kreditë dhe huatë, pjesa afatgjatë", 2, null },
                    { 24, 2, "2450", "Uzun Vadeli Karşılıklar (Long-term Provisions)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Provizionet afatgjata", 2, null },
                    { 25, 2, "2500", "Finansal Kiralama Yükümlülükleri, Uzun Vadeli Kısım (Lease Liabilities, Long-term Portion)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet ndaj lizingut, pjesa afatgjatë", 2, null },
                    { 26, 2, "2550", "Ertelenmiş Vergi Yükümlülükleri (Deferred Tax Liabilities)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet e shtyra tatimore", 2, null },
                    { 27, 2, "2600", "Diğer Uzun Vadeli Yükümlülükler (Other Long-term Liabilities)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet e tjera afatgjata", 2, null },
                    { 28, 3, "3000", "Sermaye (Share Capital)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kapitali aksionar", 2, null },
                    { 29, 3, "3100", "Birikmiş Karlar (Retained Earnings)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fitimet e mbajtura", 2, null },
                    { 30, 3, "3200", "Diğer Yedekler (Other Reserves)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rezervat e tjera", 2, null },
                    { 31, 4, "4000", "Gelirler / Hasılat (Revenue)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Të Hyrat", 2, null },
                    { 32, 4, "4100", "Diğer Gelirler (Other Income)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Të ardhura tjera", 2, null },
                    { 33, 4, "4200", "Finansal Gelirler (Financial Income)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Të ardhurat financiare", 2, null },
                    { 34, 5, "5000", "Satışların Maliyeti (Cost of Sales)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kostoja e shitjes", 1, null },
                    { 35, 5, "5100", "Dağıtım Giderleri (Distribution Expenses)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Shpenzimet e shpërndarjes", 1, null },
                    { 36, 5, "5200", "İdari Giderler (Administrative Expenses)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Shpenzimet administrative", 1, null },
                    { 37, 5, "5300", "Diğer Giderler (Other Expenses)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Shpenzimet e tjera", 1, null },
                    { 38, 5, "5400", "Finansal Giderler (Financial Expenses)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Shpenzimet financiare", 1, null },
                    { 39, 5, "5500", "Kurumlar Vergisi Gideri (Income Tax Expense)", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Shpenzimet e tatimit në fitim", 1, null }
                });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "Id", "Modified", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "İş Bankası" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ziraat Bankası" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "TEB" },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Raifaisen Bank" },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Procredit Bank" }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Modified", "Name", "Period", "TradeName", "UidNumber", "VatNumber" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pintech d.o.o.", "2024", "Pintech Solutions", "123456789", "AL123456789" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Modified", "Name" },
                values: new object[,]
                {
                    { 1, "EUR", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Euro" },
                    { 2, "USD", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "US Dollar" },
                    { 3, "ALL", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Albanian Lek" }
                });

            migrationBuilder.InsertData(
                table: "UnitOfMeasures",
                columns: new[] { "Id", "Abbreviation", "Modified", "Name" },
                values: new object[,]
                {
                    { 1, "AD", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Adet" },
                    { 2, "KG", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kilogram" },
                    { 3, "LT", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Litre" },
                    { 4, "MT", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Metre" },
                    { 5, "PK", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Paket" },
                    { 6, "KT", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kutu" }
                });

            migrationBuilder.InsertData(
                table: "Vats",
                columns: new[] { "Id", "IsActive", "Modified", "Name", "Rate" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "TVSH %0", 0f },
                    { 2, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "TVSH %8", 8f }
                });

            migrationBuilder.InsertData(
                table: "Vats",
                columns: new[] { "Id", "IsActive", "IsDefault", "Modified", "Name", "Rate" },
                values: new object[] { 3, true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "TVSH %18", 18f });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountType", "Code", "Description", "IsActive", "Modified", "Name", "NormalBalance", "ParentAccountId" },
                values: new object[,]
                {
                    { 40, 1, "1000.1000", "Kasada bulunan nakit para.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kasë (Kasa)", 1, 1 },
                    { 41, 1, "1000.2000", "Banka hesapları.", true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Banka (Banka)", 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountType", "Code", "Description", "IsActive", "IsPostable", "Modified", "Name", "NormalBalance", "ParentAccountId" },
                values: new object[,]
                {
                    { 42, 1, "1100.1000", "İşletmenin çeşitli faaliyetlerinden doğan alacakları.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Llogaritë e arkëtueshme (Alacak Hesapları)", 1, 3 },
                    { 43, 1, "1150.1000", "Satın alınıp üzerinde değişiklik yapılmadan satılan mallar.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mallra Tregtare (Ticari Mallar)", 1, 4 },
                    { 44, 1, "1150.2000", "Üretimde kullanılacak temel hammaddeler.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lëndë e parë (Hammadde)", 1, 4 },
                    { 45, 1, "1150.3000", "Henüz üretim süreci tamamlanmamış ürünler.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Produkte në proces (Yarı Mamuller)", 1, 4 },
                    { 46, 1, "1150.4000", "Üretim süreci tamamlanmış, satışa hazır ürünler.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Produkte të gatshme (Mamuller)", 1, 4 },
                    { 47, 1, "1200.7008", "%8 oranında alım ve giderler üzerinden hesaplanan ve indirilecek KDV.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "TVSH e zbritshme 8% (İndirilecek KDV %8)", 1, 5 },
                    { 48, 1, "1200.7018", "%18 oranında alım ve giderler üzerinden hesaplanan ve indirilecek KDV.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "TVSH e zbritshme 18% (İndirilecek KDV %18)", 1, 5 },
                    { 49, 1, "1450.1000", "İşletme faaliyetlerinde kullanılan demirbaşlar, makine ve teçhizat.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Nënshkrime dhe pajisje (Demirbaşlar ve Tesisat)", 1, 9 },
                    { 50, 6, "1450.6000", "Pajisje ve demirbaşlar için ayrılan birikmiş amortisman.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Amortizimi i pajisjeve (Ekipman Amortismanı)", 2, 9 },
                    { 51, 2, "2050.1000", "İşletmenin ticari faaliyetlerinden doğan borçları.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Furnitorët (Tedarikçiler)", 2, 16 },
                    { 52, 2, "2200.1000", "İşletmenin kazançları üzerinden ödenecek kurumlar vergisi.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tatimi mbi fitimin (Kurumlar Vergisi)", 2, 19 },
                    { 53, 2, "2350.1000", "Finansal kiralama sözleşmelerinden doğan yükümlülükler.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet për Qira (Kira Yükümlülükleri)", 2, 22 },
                    { 54, 2, "2350.1100", "Finansal kiralama sözleşmelerinden doğan vergi yükümlülükleri.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet Tatim mbi Qirën (Kira Vergisi Yükümlülükleri)", 2, 22 },
                    { 55, 2, "2350.2000", "İşletmenin personeline ödenecek ücret ve maaşlardan doğan yükümlülükler.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet për Paga ndaj Personelit (Personel Ücret Yükümlülükleri)", 2, 22 },
                    { 56, 2, "2350.2100", "İşletmenin personeline ödenecek ücret ve maaşlardan doğan vergi yükümlülükleri.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet për Tatimin mbi Paga (Ücret Vergisi Yükümlülükleri)", 2, 22 },
                    { 57, 2, "2350.2200", "İşletmenin sosyal güvenlik primlerinden doğan yükümlülükler.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet për Kontributer (Sosyal Güvenlik Prim Yükümlülükleri)", 2, 22 },
                    { 58, 2, "2350.7000", "İşletmenin gümrük vergilerinden doğan yükümlülükler.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet per Dogan (Gümrük Yükümlülükleri)", 2, 22 },
                    { 59, 2, "2350.7008", "İşletmenin gümrük vergilerinden doğan yükümlülükler.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet për Doganë TVSH 8% (Gümrük KDV Yükümlülükleri %8)", 2, 22 },
                    { 60, 2, "2350.7018", "İşletmenin gümrük vergilerinden doğan yükümlülükler.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet për Doganë TVSH 18% (Gümrük KDV Yükümlülükleri %18)", 2, 22 },
                    { 61, 2, "2350.7100", "İşletmenin akçiz vergilerinden doğan yükümlülükler.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet per Akciza (ÖTV Yükümlülükleri)", 2, 22 },
                    { 62, 2, "2350.6218", "İşletmenin KDV yükümlülükleri.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet për Llogaritje TVSH 18% (KDV Hesap Yükümlülükleri %18)", 2, 22 },
                    { 63, 2, "2350.6208", "İşletmenin KDV yükümlülükleri.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet për Llogaritje TVSH 8% (KDV Hesap Yükümlülükleri %8)", 2, 22 },
                    { 64, 2, "2350.6200", "İşletmenin KDV yükümlülükleri.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Detyrimet për TVSH 0% (KDV Hesap Yükümlülükleri %0)", 2, 22 },
                    { 65, 3, "3000.1000", "İşletmenin kuruluşunda veya sonraki sermaye artırımlarında ortaklar tarafından taahhüt edilen ve ödenen sermaye tutarı.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kapitali Fillestar (Başlangıç Sermayesi)", 2, 28 },
                    { 66, 3, "3100.1000", "İşletmenin içinde bulunulan hesap dönemine ait net kar veya zarar tutarı. Yıl sonunda bu hesap kapanarak birikmiş karlara veya zararlara aktarılır.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fitimi/Humbja e Vitit Rrjedhës (Bu Yılın Karı/Zararı)", 2, 29 },
                    { 67, 3, "3000.1500", "İşletmenin önceki hesap dönemlerinden devreden ve henüz dağıtılmamış veya kapatılmamış birikmiş kar veya zarar tutarları.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fitimet/Humbjet e Mbartura (Geçmiş Yıllar Karları/Zararları)", 2, 29 },
                    { 68, 4, "4000.1000", "Ticari malların satışından elde edilen gelirler.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Të ardhura nga shitja e mallrave (Ticari Mal Satış Gelirleri)", 2, 31 },
                    { 69, 4, "4000.2000", "Sunulan hizmetlerden elde edilen gelirler.", true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Të ardhura nga ofrimi i shërbimeve (Hizmet Satış Gelirleri)", 2, 31 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ParentAccountId",
                table: "Accounts",
                column: "ParentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressPartners_PartnerId",
                table: "AddressPartners",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressWarehouses_WarehouseId",
                table: "AddressWarehouses",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountCompanies_CompanyId",
                table: "BankAccountCompanies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccountPartners_PartnerId",
                table: "BankAccountPartners",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_BankId",
                table: "BankAccounts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_BankId1",
                table: "BankAccounts",
                column: "BankId1");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_CurrencyId",
                table: "BankAccounts",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Barcodes_ProductId",
                table: "Barcodes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactDetails_ContactId",
                table: "ContactDetails",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactPartners_PartnerId",
                table: "ContactPartners",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactWarehouses_WarehouseId",
                table: "ContactWarehouses",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_AccountId",
                table: "LedgerEntries",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_AccountId1",
                table: "LedgerEntries",
                column: "AccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_CurrencyId",
                table: "LedgerEntries",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_LedgerId",
                table: "LedgerEntries",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerEntries_PartnerId",
                table: "LedgerEntries",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoryId",
                table: "ProductCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDocuments_ProductId",
                table: "ProductDocuments",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductId",
                table: "ProductPrices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PurchaseAccountId",
                table: "Products",
                column: "PurchaseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SaleAccountId",
                table: "Products",
                column: "SaleAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitOfMeasureId",
                table: "Products",
                column: "UnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_VatId",
                table: "Products",
                column: "VatId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceExpenses_PartnerId",
                table: "PurchaseInvoiceExpenses",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceExpenses_PurchaseInvoiceId",
                table: "PurchaseInvoiceExpenses",
                column: "PurchaseInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceExpenses_VendorAccountId",
                table: "PurchaseInvoiceExpenses",
                column: "VendorAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceLines_ProductId",
                table: "PurchaseInvoiceLines",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceLines_PurchaseAccountId",
                table: "PurchaseInvoiceLines",
                column: "PurchaseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceLines_PurchaseInvoiceId",
                table: "PurchaseInvoiceLines",
                column: "PurchaseInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceLines_UnitOfMeasureId",
                table: "PurchaseInvoiceLines",
                column: "UnitOfMeasureId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceLines_VatId",
                table: "PurchaseInvoiceLines",
                column: "VatId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoiceLines_WarehouseId",
                table: "PurchaseInvoiceLines",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_CurrencyId",
                table: "PurchaseInvoices",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_LedgerId",
                table: "PurchaseInvoices",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_PartnerId",
                table: "PurchaseInvoices",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseInvoices_VendorAccountId",
                table: "PurchaseInvoices",
                column: "VendorAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_CompanyId",
                table: "Warehouses",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_CompanyId1",
                table: "Warehouses",
                column: "CompanyId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressPartners");

            migrationBuilder.DropTable(
                name: "AddressWarehouses");

            migrationBuilder.DropTable(
                name: "BankAccountCompanies");

            migrationBuilder.DropTable(
                name: "BankAccountPartners");

            migrationBuilder.DropTable(
                name: "Barcodes");

            migrationBuilder.DropTable(
                name: "ContactDetails");

            migrationBuilder.DropTable(
                name: "ContactPartners");

            migrationBuilder.DropTable(
                name: "ContactWarehouses");

            migrationBuilder.DropTable(
                name: "LedgerEntries");

            migrationBuilder.DropTable(
                name: "OperationClaims");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ProductDocuments");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductPrices");

            migrationBuilder.DropTable(
                name: "PurchaseInvoiceExpenses");

            migrationBuilder.DropTable(
                name: "PurchaseInvoiceLines");

            migrationBuilder.DropTable(
                name: "UserOperationClaims");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "PurchaseInvoices");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "UnitOfMeasures");

            migrationBuilder.DropTable(
                name: "Vats");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Ledgers");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
