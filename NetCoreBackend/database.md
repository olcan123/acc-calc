# Veritabanı Şemasi

Bu belge, uygulamanın veritabanı şemasını tanımlar. Her tablo, ilgili entity sınıfı ile temsil edilir ve tablolar arasındaki ilişkiler belirtilir.

## Tablolar

### Account

*   **Açıklama:** Muhasebe hesaplarını temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Hesap ID'si.
    *   `Code` (string): Hesap kodu.
    *   `Name` (string): Hesap adi.
    *   `AccountTypeId` (short, FK): Hesap türü ID'si (`AccountType` tablosuna referans).
    *   `ParentAccountId` (int, FK): Üst hesap ID'si (kendine referans).
    *   `Status` (short): Hesap durumu.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `AccountType` (Many-to-One): İlişkili hesap türü.
    *   `ParentAccount` (Many-to-One): Üst hesap.
    *   `ChildAccounts` (One-to-Many): Alt hesaplar.

### AccountType

*   **Açıklama:** Muhasebe hesap türlerini temsil eder.
*   **Alanlar:**
    *   `Id` (short, PK): Hesap türü ID'si.
    *   `Name` (string): Hesap türü adı.
    *   `Description` (string): Aciklama.
    *   `IsDebitBalance` (bool): Normal bakiyesi borç mu?
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Accounts` (One-to-Many): Bu türe ait hesaplar.

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
    *   `Currency` (string): Para birimi.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Bank` (Many-to-One): İlişkili banka.
    *   `BankAccountCompanies` (One-to-Many): Bu hesaba sahip sirketler.
    *   `BankAccountPartners` (One-to-Many): Bu hesaba sahip is ortaklari.

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
    *   `PurchaseInvoices` (One-to-Many): Bu para biriminde düzenlenen satin alma faturalari.

### Ledger

*   **Açıklama:** Defter kayıtlarını temsil eder.
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
    *   `PartnerType` (enum): İş ortağı türü (`Business`, `Individual`, `Employee`).
    *   `BusinessPartnerType` (enum, nullable): İş ortağı iş türü (`Supplier`, `Customer`, `Both`).
    *   `IdentityNumber` (string): Kimlik numarası.
    *   `VatNumber` (string): Vergi numarası.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `PurchaseInvoices` (One-to-Many): Is ortagina ait satin alma faturalari.
    *   `PurchaseInvoiceExpenses` (One-to-Many): Is ortagina ait satin alma faturasi giderleri.
    *   `BankAccountPartners` (One-to-Many): Is ortaginin banka hesaplari.
    *   `ContactPartners` (One-to-Many): Is ortaginin iletişim bilgileri.
    *   `AddressPartners` (One-to-Many): Is ortaginin adresleri.
    *   `LedgerEntries` (One-to-Many): Is ortagi ile ilgili defter hareketleri.

### Product

*   **Açıklama:** Ürünleri temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Ürün ID'si.
    *   `Name` (string): Ürün adı.
    *   `Description` (string): Açıklama.
    *   `CustomsTaxRate` (float): Gümrük vergisi orani.
    *   `ExciseTaxRate` (float): ÖTV oranı.
    *   `VatId` (int, FK): KDV ID'si (`Vat` tablosuna referans).
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
*   **Alanlar:**
    *   `Id` (int, PK): Fiyat ID'si.
    *   `ProductId` (int, FK): Ürün ID'si (`Product` tablosuna referans).
    *   `UnitPrice` (decimal): Birim fiyat.
    *   `Category` (enum): Fiyat kategorisi (`Regular`, `Promo`). 
    *   `Side` (enum): Fiyat yönü (`Purchase`, `Sales`).
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
    *   `InvoiceType` (short): Fatura türü.
    *   `PartnerId` (int, FK): Is ortagi ID'si (`Partner` tablosuna referans).
    *   `InvoiceNo` (string): Fatura numarası.
    *   `InvoiceDate` (DateTime): Fatura tarihi.
    *   `ImportPartnerDocNo` (string): İthalatçı iş ortağı belge numarası.
    *   `ImportPartnerDocDate` (DateTime): İthalatçı iş ortağı belge tarihi.
    *   `CurrencyId` (int, FK): Para birimi ID'si (`Currency` tablosuna referans).
    *   `ExchangeRate` (decimal): Kur.
    *   `Status` (short): Durum.
    *   `Created` (DateTime): Oluşturulma tarihi (BaseEntity'den).
    *   `Modified` (DateTime): Güncellenme tarihi (BaseEntity'den).
*   **İlişkiler:**
    *   `Partner` (Many-to-One): İlişkili is ortagi.
    *   `Currency` (Many-to-One): İlişkili para birimi.
    *   `Ledger` (Many-to-One): İlişkili defter.
    *   `PurchaseInvoiceLines` (One-to-Many): Fatura satırları.
    *   `PurchaseInvoiceExpenses` (One-to-Many): Fatura giderleri. 

### PurchaseInvoiceExpense 

*   **Açıklama:** Satın alma faturası giderlerini temsil eder.
*   **Alanlar:**
    *   `Id` (int, PK): Gider ID'si.
    *   `PurchaseInvoiceId` (int, FK): Fatura ID'si (`PurchaseInvoice` tablosuna referans).
    *   `PartnerId` (int, FK): İş ortağı ID'si (`Partner` tablosuna referans).
    *   `PartnerInvoiceNo` (string): İş ortağı fatura numarası.
    *   `PartnerInvoiceDate` (DateTime): Is ortagi fatura tarihi.
    *   `ExpenseType` (short): Gider türü.
    *   `Amount` (decimal): Tutar (yerel para birimi).
    *   `AmountFc` (decimal): Tutar (yabancı para birimi).
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
    *   `VatTaxRate` (decimal): KDV oranı.
    *   `VatTaxAmount` (decimal): KDV tutarı.
    *   `CostPrice` (decimal): Maliyet fiyatı.
    *   `CostAmount` (decimal): Maliyet tutarı.
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
