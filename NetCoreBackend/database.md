# Veritabanı Şemasi

Bu belge, uygulamanın veritabanı şemasını tanımlar. Her tablo, ilgili entity sınıfı ile temsil edilir ve tablolar arasındaki ilişkiler belirtilir.

## Tablolar

### Account

*   **Açıklama:** Muhasebe hesaplarını temsil eder.
*   **Enum Tanımlamaları:**
    *   `AccountTypeOption`: Hesap türlerini tanımlar (Asset=1, Liability=2, Equity=3, Revenue=4, Expense=5).
    *   `NormalBalanceOption`: Normal bakiye türlerini tanımlar (Debit=1, Credit=2).
*   **Alanlar:**
    *   `AccountId` (int, PK): Hesap ID'si.
    *   `AccountCode` (string): Hesap kodu.
    *   `AccountName` (string): Hesap adı.
    *   `ParentAccountId` (int, FK, nullable): Üst hesap ID'si (kendine referans).
    *   `IsActive` (bool): Hesap aktif mi? (Varsayılan: true)
    *   `IsPostable` (bool): Hesaba kayıt yapılabilir mi? (Varsayılan: true)
    *   `NormalBalance` (enum, nullable): Normal bakiye tipi (Debit=1, Credit=2)
    *   `AccountType` (enum): Hesap türü (Asset=1, Liability=2, Equity=3, Revenue=4, Expense=5)
    *   `Description` (string): Açıklama.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `ParentAccount` (Many-to-One): Üst hesap.
    *   `Children` (One-to-Many): Alt hesaplar.
    *   `LedgerEntries` (One-to-Many): Bu hesaba ait muhasebe kayıtları.

### Address

*   **Açıklama:** Adres bilgilerini temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Adres ID'si.
    *   `Location` (string): Konum.
    *   `Type` (string): Adres türü.
    *   `Street` (string): Sokak. 
    *   `City` (string): Şehir.
    *   `State` (string): Eyalet.
    *   `Country` (string): Ülke.
    *   `ZipCode` (string): Posta kodu.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `AddressWarehouses` (One-to-Many): Adresin ilişkilendirildiği depolar.
    *   `AddressPartners` (One-to-Many): Adresin ilişkilendirildiği is ortaklari.

### AddressPartner

*   **Açıklama:** Adreslerin iş ortakları ile ilişkisini temsil eder.
*   **Alanlar:**
    *   `AddressId` (int, FK): Adres ID'si (`Address` tablosuna referans).
    *   `PartnerId` (int, FK): İş ortağı ID'si (`Partner` tablosuna referans).
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Address` (Many-to-One): İlişkili adres.
    *   `Partner` (Many-to-One): İlişkili is ortagi.

### AddressWarehouse

*   **Açıklama:** Adreslerin depolar ile ilişkisini temsil eder.
*   **Alanlar:**
    *   `AddressId` (int, FK): Adres ID'si (`Address` tablosuna referans).
    *   `WarehouseId` (int, FK): Depo ID'si (`Warehouse` tablosuna referans).
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Address` (Many-to-One): İlişkili adres.
    *   `Warehouse` (Many-to-One): İlişkili depo. 

### Bank

*   **Açıklama:** Bankaları temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Banka ID'si.
    *   `Name` (string): Banka adı.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `BankAccounts` (One-to-Many): Bu bankaya ait banka hesapları.
    
### BankAccount

*   **Açıklama:** Banka hesaplarını temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Banka hesap ID'si.
    *   `BankId` (int, FK): Banka ID'si (`Bank` tablosuna referans).
    *   `BranchName` (string): Şube adı.
    *   `AccountNumber` (string): Hesap numarası.
    *   `IBAN` (string): IBAN numarası.
    *   `SwiftCode` (string): Swift kodu.
    *   `CurrencyId` (int, FK): Para birimi ID'si (`Currency` tablosuna referans).
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Bank` (Many-to-One): İlişkili banka.
    *   `Currency` (Many-to-One): İlişkili para birimi.
    *   `BankAccountCompanies` (One-to-Many): Bu hesaba sahip şirketler.
    *   `BankAccountPartners` (One-to-Many): Bu hesaba sahip iş ortakları.

### BankAccountCompany

*   **Açıklama:** Banka hesaplarının şirketlerle ilişkisini temsil eder.
*   **Alanlar:**
    *   `BankAccountId` (int, FK): Banka hesap ID'si (`BankAccount` tablosuna referans).
    *   `CompanyId` (int, FK): Şirket ID'si (`Company` tablosuna referans).
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `BankAccount` (Many-to-One): İlişkili banka hesabı.
    *   `Company` (Many-to-One): İlişkili sirket.

### BankAccountPartner

*   **Açıklama:** Banka hesaplarının iş ortakları ile ilişkisini temsil eder.
*   **Alanlar:**
    *   `BankAccountId` (int, FK): Banka hesap ID'si (`BankAccount` tablosuna referans).
    *   `PartnerId` (int, FK): İş ortağı ID'si (`Partner` tablosuna referans).
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `BankAccount` (Many-to-One): İlişkili banka hesabı.
    *   `Partner` (Many-to-One): İlişkili is ortagi.

### Barcode

*   **Açıklama:** Ürün barkodlarını temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Barkod ID'si.
    *   `Code` (string): Barkod kodu.
    *   `Type` (string): Barkod türü.
    *   `ProductId` (int, FK): Ürün ID'si (`Product` tablosuna referans).
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Product` (Many-to-One): İlişkili ürün.
    
### Category

*   **Açıklama:** Ürün kategorilerini temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Kategori ID'si.
    *   `Name` (string): Kategori adı.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `ProductCategories` (One-to-Many): Bu kategoriye ait ürünler.
    
### Company

*   **Açıklama:** Şirket bilgilerini temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Şirket ID'si.
    *   `Name` (string): Şirket adı.
    *   `TradeName` (string): Ticari unvan.
    *   `UidNumber` (string): Vergi kimlik numarası.
    *   `VatNumber` (string): Vergi numarası.
    *   `Period` (string): Dönem.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `BankAccountCompanies` (One-to-Many): Şirkete ait banka hesapları.
    *   `Warehouses` (One-to-Many): Şirkete ait depolar. 

### Contact

*   **Açıklama:** İletişim bilgilerini temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): İletişim ID'si.
    *   `Name` (string): İletişim adı.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `ContactDetails` (One-to-Many): İletişim detayları.
    *   `ContactWarehouses` (One-to-Many): İletişimin ilişkilendirildiği depolar. 
    *   `ContactPartners` (One-to-Many): İletişimin ilişkilendirildiği is ortaklari.

### ContactDetail

*   **Açıklama:** İletişim detaylarını (telefon, e-posta vb.) temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): İletişim detay ID'si.
    *   `ContactId` (int, FK): İletişim ID'si (`Contact` tablosuna referans).
    *   `Name` (string): Detay adı (örn. "Telefon", "E-posta").
    *   `Value` (string): Detay değeri (örn. "555-1234", "info@example.com").
    *   `Description` (string): Aciklama.
    *   `IsActive` (bool): Aktif mi?
    *   `IsPrimary` (bool): Birincil iletişim detayı mı?
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Contact` (Many-to-One): İlişkili iletişim.

### ContactPartner

*   **Açıklama:** İletişimlerin iş ortakları ile ilişkisini temsil eder.
*   **Alanlar:**
    *   `ContactId` (int, FK): İletişim ID'si (`Contact` tablosuna referans).
    *   `PartnerId` (int, FK): İş ortağı ID'si (`Partner` tablosuna referans).
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Contact` (Many-to-One): İlişkili iletişim.
    *   `Partner` (Many-to-One): İlişkili is ortagi.

### ContactWarehouse

*   **Açıklama:** İletişimlerin depolar ile ilişkisini temsil eder.
*   **Alanlar:**
    *   `ContactId` (int, FK): İletişim ID'si (`Contact` tablosuna referans).
    *   `WarehouseId` (int, FK): Depo ID'si (`Warehouse` tablosuna referans).
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Contact` (Many-to-One): İlişkili iletişim.
    *   `Warehouse` (Many-to-One): İlişkili depo. 

### Currency

*   **Açıklama:** Para birimlerini temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Para birimi ID'si.
    *   `Code` (string): Para birimi kodu (örn. "TRY", "USD").
    *   `Name` (string): Para birimi adı (örn. "Türk Lirası", "ABD Doları").
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `LedgerEntries` (One-to-Many): Bu para biriminde yapılan defter hareketleri.
    *   `PurchaseInvoices` (One-to-Many): Bu para biriminde düzenlenen satın alma faturaları.
    *   `BankAccounts` (One-to-Many): Bu para biriminde olan banka hesapları.

### Ledger

*   **Açıklama:** Defter kayıtlarını temsil eder.
*   **Enum Tanımlamaları:**
    *   `LedgerDocumentType`: Belge türlerini tanımlar (PurchaseInvoice=1, SalesInvoice=2, Payment=3, Receipt=4, Journal=5).
*   **Alanlar:**
    *   `Id` (int, PK): Defter ID'si.
    *   `DocumentType` (short): Belge türü.
    *   `DocumentDate` (DateTime): Belge tarihi.
    *   `ReferenceNo` (string): Referans numarası.
    *   `Description` (string): Aciklama.
    *   `Status` (short): Durum.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `LedgerEntries` (One-to-Many): Bu deftere ait hareketler.
    *   `PurchaseInvoices` (One-to-Many): Bu defterle ilişkili satin alma faturalari.

### LedgerEntry

*   **Açıklama:** Defter hareketlerini temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Hareket ID'si.
    *   `LedgerId` (int, FK): Defter ID'si (`Ledger` tablosuna referans).
    *   `PartnerId` (int, FK): Is ortagi ID'si (`Partner` tablosuna referans).
    *   `LineNo` (int): Satır numarası.
    *   `AccountId` (int, FK): Hesap ID'si (`Account` tablosuna referans).
    *   `Description` (string): Aciklama.
    *   `Debit` (decimal): Borç tutarı (yerel para birimi).
    *   `Credit` (decimal): Alacak tutarı (yerel para birimi).
    *   `CurrencyId` (int, FK): Para birimi ID'si (`Currency` tablosuna referans).
    *   `CurrencyRate` (decimal): Kur.
    *   `DebitFc` (decimal): Borç tutarı (yabancı para birimi).
    *   `CreditFc` (decimal): Alacak tutarı (yabancı para birimi).
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Ledger` (Many-to-One): İlişkili defter.
    *   `Account` (Many-to-One): İlişkili hesap.
    *   `Currency` (Many-to-One): İlişkili para birimi.
    *   `Partner` (Many-to-One): İlişkili is ortagi.

### Partner

*   **Açıklama:** İş ortaklarını (tedarikçi, müşteri vb.) temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): İş ortağı ID'si.
    *   `Name` (string): İş ortağı adı.
    *   `TradeName` (string): Ticari unvan.
    *   `PartnerType` (enum): İş ortağı türü (Business, Individual, Employee).
    *   `BusinessPartnerType` (enum, nullable): İş ortağı iş türü (Supplier, Customer, Both).
    *   `IdentityNumber` (string): Kimlik numarası.
    *   `VatNumber` (string): Vergi numarası.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `PurchaseInvoices` (One-to-Many): İş ortağına ait satın alma faturaları.
    *   `PurchaseInvoiceExpenses` (One-to-Many): İş ortağına ait satın alma faturası giderleri.
    *   `BankAccountPartners` (One-to-Many): İş ortağının banka hesapları.
    *   `ContactPartners` (One-to-Many): İş ortağının iletişim bilgileri.
    *   `AddressPartners` (One-to-Many): İş ortağının adresleri.
    *   `LedgerEntries` (One-to-Many): İş ortağı ile ilgili defter hareketleri.

### Product

*   **Açıklama:** Ürünleri temsil eder.
*   **Enum Tanımlamaları:**
    *   `ProductType`: Ürün türlerini tanımlar (StockableMerchandise=1, RawMaterial=2, WorkInProgress=3, FinishedGoods=4, Service=5, FixedAsset=7, Expense=8, Advance=9).
*   **Alanlar:**
    *   `Id` (int, PK): Ürün ID'si.
    *   `Name` (string): Ürün adı.
    *   `Description` (string): Açıklama.
    *   `CustomsTaxRate` (float): Gümrük vergisi orani.
    *   `ExciseTaxRate` (float): ÖTV oranı.
    *   `VatId` (int, FK): KDV ID'si (`Vat` tablosuna referans).
    *   `ProductType` (enum): Ürün türü.
    *   `UnitOfMeasureId` (int, FK): Ölçü birimi ID'si (`UnitOfMeasure` tablosuna referans).
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Vat` (Many-to-One): İlişkili KDV oranı.
    *   `UnitOfMeasure` (Many-to-One): İlişkili ölçü birimi.
    *   `Barcodes` (One-to-Many): Ürüne ait barkodlar.
    *   `ProductCategories` (One-to-Many): Ürünün kategorileri.
    *   `ProductPrices` (One-to-Many): Ürünün fiyatlari.
    *   `ProductImages` (One-to-Many): Ürünün resimleri.
    *   `ProductDocuments` (One-to-Many): Ürünün belgeleri.

### ProductCategory

*   **Açıklama:** Ürünlerin kategorilerle ilişkisini temsil eder.
*   **Alanlar:**
    *   `ProductId` (int, FK): Ürün ID'si (`Product` tablosuna referans).
    *   `CategoryId` (int, FK): Kategori ID'si (`Category` tablosuna referans).
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Product` (Many-to-One): İlişkili ürün.
    *   `Category` (Many-to-One): İlişkili kategori. 

### ProductDocument

*   **Açıklama:** Ürün belgelerini temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Belge ID'si.
    *   `ProductId` (int, FK): Ürün ID'si (`Product` tablosuna referans).
    *   `FileName` (string): Dosya adı.
    *   `FileUrl` (string): Dosya URL'si. 
    *   `DocumentType` (string): Belge türü.
    *   `UploadedAt` (DateTime): Yüklenme tarihi.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Product` (Many-to-One): İlişkili ürün.

### ProductImage

*   **Açıklama:** Ürün resimlerini temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Resim ID'si.
    *   `ProductId` (int, FK): Ürün ID'si (`Product` tablosuna referans).
    *   `Url` (string): Resim URL'si.
    *   `Label` (string): Etiket. 
    *   `IsMain` (bool): Ana resim mi?
    *   `DisplayOrder` (int): Gösterim sırası.
    *   `UploadedAt` (DateTime): Yüklenme tarihi.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Product` (Many-to-One): İlişkili ürün.

### ProductPrice

*   **Açıklama:** Ürün fiyatlarını temsil eder.
*   **Enum Tanımlamaları:**
    *   `PriceCategory`: Fiyat kategorilerini tanımlar (Regular=0, Promo=1).
    *   `PriceSide`: Fiyat yönünü tanımlar (Purchase=0, Sales=1).
*   **Alanlar:**
    *   `Id` (int, PK): Fiyat ID'si.
    *   `ProductId` (int, FK): Ürün ID'si (`Product` tablosuna referans).
    *   `UnitPrice` (decimal): Birim fiyat.
    *   `Category` (enum): Fiyat kategorisi (Regular=0, Promo=1). 
    *   `Side` (enum): Fiyat yönü (Purchase=0, Sales=1).
    *   `ValidFrom` (DateTime, nullable): Geçerlilik başlangıç tarihi.
    *   `ValidTo` (DateTime, nullable): Geçerlilik bitiş tarihi.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Product` (Many-to-One): İlişkili ürün.

### PurchaseInvoice

*   **Açıklama:** Satın alma faturalarını temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Fatura ID'si.
    *   `LedgerId` (int, FK): Defter ID'si (`Ledger` tablosuna referans).
    *   `InvoiceType` (enum): Fatura türü (LocalInvoice=1, ImportInvoice=2, DebitNote=3, CreditNote=4).
    *   `PartnerId` (int, FK): İş ortağı ID'si (`Partner` tablosuna referans).
    *   `InvoiceNo` (string): Fatura numarası.
    *   `InvoiceDate` (DateTime): Fatura tarihi.
    *   `ImportPartnerDocNo` (string): İthalatçı iş ortağı belge numarası.
    *   `ImportPartnerDocDate` (DateTime): İthalatçı iş ortağı belge tarihi.
    *   `CurrencyId` (int, FK): Para birimi ID'si (`Currency` tablosuna referans).
    *   `ExchangeRate` (decimal): Kur.
    *   `Status` (short): Durum.
    *   `Note` (string): Fatura notu.
    *   `IsPaid` (bool): Ödenmiş mi?
    *   `CashPaymentAmount` (decimal): Nakit ödeme tutarı.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Partner` (Many-to-One): İlişkili iş ortağı.
    *   `Currency` (Many-to-One): İlişkili para birimi.
    *   `Ledger` (Many-to-One): İlişkili defter.
    *   `PurchaseInvoiceLines` (One-to-Many): Fatura satırları.
    *   `PurchaseInvoiceExpenses` (One-to-Many): Fatura giderleri. 

### PurchaseInvoiceExpense 

*   **Açıklama:** Satın alma faturası giderlerini temsil eder.
*   **Enum Tanımlamaları:**
    *   `ExpenseType`: Gider türlerini tanımlar (Freight=1, Insurance=2, CustomsExpense=3, OtherExpense=99).
*   **Alanlar:**
    *   `Id` (int, PK): Gider ID'si.
    *   `PurchaseInvoiceId` (int, FK): Fatura ID'si (`PurchaseInvoice` tablosuna referans).
    *   `PartnerId` (int, FK): İş ortağı ID'si (`Partner` tablosuna referans).
    *   `PartnerInvoiceNo` (string): İş ortağı fatura numarası.
    *   `PartnerInvoiceDate` (DateTime): Is ortagi fatura tarihi.
    *   `ExpenseType` (enum): Gider türü (Freight=1, Insurance=2, CustomsExpense=3, OtherExpense=99).
    *   `RevaluationAmount` (decimal): Yeniden Değerleme Tutarı.
    *   `Amount` (decimal): Tutar (yerel para birimi).
    *   `AmountFc` (decimal): Tutar (yabancı para birimi).
    *   `IsPaid` (bool): Ödenmiş mi?
    *   `CashPaymentAmount` (decimal): Nakit ödeme tutarı.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `PurchaseInvoice` (Many-to-One): İlişkili fatura.
    *   `Partner` (Many-to-One): İlişkili is ortagi.

### PurchaseInvoiceLine

*   **Açıklama:** Satın alma faturası satırlarını temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Satır ID'si.
    *   `PurchaseInvoiceId` (int, FK): Fatura ID'si (`PurchaseInvoice` tablosuna referans).
    *   `ProductId` (int, FK): Ürün ID'si (`Product` tablosuna referans).
    *   `WarehouseId` (int, FK): Depo ID'si (`Warehouse` tablosuna referans). 
    *   `UnitOfMeasureId` (int, FK): Ölçü birimi ID'si (`UnitOfMeasure` tablosuna referans).
    *   `VatId` (int, FK): KDV ID'si (`Vat` tablosuna referans).
    *   `Quantity` (decimal): Miktar.
    *   `UnitPrice` (decimal): Birim fiyat.
    *   `Amount` (decimal): Tutar.
    *   `ExpenseAmount` (decimal): Gider tutarı.
    *   `DiscountRate` (decimal): İndirim oranı.
    *   `DiscountAmount` (decimal): İndirim tutarı.
    *   `ExciseTaxRate` (decimal): ÖTV oranı.
    *   `ExciseTaxAmount` (decimal): ÖTV tutarı.
    *   `CustomsRate` (decimal): Gümrük oranı.
    *   `CustomsAmount` (decimal): Gümrük tutarı.
    *   `RevaluationAmount` (decimal): Yeniden Değerleme Tutarı.
    *   `VatTaxRate` (decimal): KDV oranı.
    *   `VatTaxAmount` (decimal): KDV tutarı.
    *   `CostPrice` (decimal): Maliyet fiyatı.
    *   `CostAmount` (decimal): Maliyet tutarı.
    *   `TotalPrice` (decimal): Toplam birim fiyat.
    *   `TotalAmount` (decimal): Toplam tutar.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `PurchaseInvoice` (Many-to-One): İlişkili fatura.
    *   `Product` (Many-to-One): İlişkili ürün. 
    *   `Warehouse` (Many-to-One): İlişkili depo.
    *   `UnitOfMeasure` (Many-to-One): İlişkili ölçü birimi.
    *   `Vat` (Many-to-One): İlişkili KDV oranı.

### UnitOfMeasure

*   **Açıklama:** Ölçü birimlerini temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Ölçü birimi ID'si.
    *   `Name` (string): Ölçü birimi adı.
    *   `Abbreviation` (string): Kısaltma.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den). 
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Products` (One-to-Many): Bu ölçü birimindeki ürünler.

### Vat

*   **Açıklama:** KDV oranlarını temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): KDV ID'si.
    *   `Name` (string): KDV oranı adı.
    *   `Rate` (float): KDV orani.
    *   `IsDefault` (bool): Varsayılan mı?
    *   `IsActive` (bool): Aktif mi?
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Products` (One-to-Many): Bu KDV oranına sahip ürünler.

### Warehouse

*   **Açıklama:** Depoları temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Depo ID'si.
    *   `CompanyId` (int, FK): Şirket ID'si (`Company` tablosuna referans).
    *   `Name` (string): Depo adı.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Company` (Many-to-One): İlişkili şirket.
    *   `AddressWarehouses` (One-to-Many): Deponun adresleri. 
    *   `ContactWarehouses` (One-to-Many): Deponun iletişim bilgileri. 

Bu `database.md` dosyası, veritabanı şemasının genel bir özetini sunar. Her tablo için alanlar ve ilişkiler detaylı bir şekilde açıklanmıştır. Bu belge, veritabanı tasarımı ve uygulamadaki veri akışını anlamak için faydalı bir kaynak olacaktır.
