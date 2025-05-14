# Ledger & LedgerEntry Şeması

## Tablo: `ledger`

| Sütun           | Veri Tipi      | Açıklama                                                       |
| --------------- | -------------- | -------------------------------------------------------------- |
| `ledger_id`     | `UUID` **PK**  | Birincil anahtar, otomatik üretilir (ör. `gen_random_uuid()`). |
| `document_type` | `SMALLINT`     | 1 = Alış, 2 = Satış, 3 = Üretim…                               |
| `document_date` | `DATE`         | Belge / fiş tarihi.                                            |
| `reference_no`  | `VARCHAR(50)`  | Kaynak belge veya fiş numarası.                                |
| `description`   | `VARCHAR(255)` | İsteğe bağlı açıklama metni.                                   |
| `status`        | `SMALLINT`     | 0 = Taslak, 1 = Kesin (Posted), 2 = Ters Kayıt (Reversed).     |

---

## Tablo: `ledger_entry`

| Sütun           | Veri Tipi                             | Açıklama                                     |
| --------------- | ------------------------------------- | -------------------------------------------- |
| `entry_id`      | `UUID` **PK**                         | Birincil anahtar, otomatik üretilir.         |
| `ledger_id`     | `UUID` **FK → ledger.ledger\_id**     | Satırın bağlı olduğu fiş başlığı.            |
| `line_no`       | `INTEGER`                             | Fiş içindeki sıra numarası.                  |
| `account_id`    | `UUID` **FK → account.account\_id**   | Genel muhasebe hesap kodu.                   |
| `description`   | `VARCHAR(255)`                        | İsteğe bağlı satır açıklaması.               |
| `debit`         | `NUMERIC(18,4)`                       | Yerel para biriminde borç tutarı (Debit).    |
| `credit`        | `NUMERIC(18,4)`                       | Yerel para biriminde alacak tutarı (Credit). |
| `currency_id`   | `UUID` **FK → currency.currency\_id** | İşlemin döviz cinsi.                         |
| `currency_rate` | `NUMERIC(12,6)`                       | Yerel para birimine çevrim kuru.             |
| `debit_lc`      | `NUMERIC(18,4)`                       | Borç tutarının yerel para karşılığı.         |
| `credit_lc`     | `NUMERIC(18,4)`                       | Alacak tutarının yerel para karşılığı.       |
| `created_at`    | `TIMESTAMPTZ`                         | Satırın eklenme zamanı.                      |

---

> **Not**: Parasal hassasiyet için tüm tutar alanları `DECIMAL/NUMERIC` tipinde tanımlanmıştır.

## Tablo: `purchase_invoice`

| Sütun                      | Veri Tipi       | Açıklama                                                          |
| -------------------------- | --------------- | ----------------------------------------------------------------- |
| `purchase_invoice_id`      | `UUID` **PK**   | Tabloya özgü benzersiz kimlik (otomatik `gen_random_uuid()`).     |
| `invoice_type`             | `SMALLINT`      | **0** = Tedarikçi Faturası   •  **1** = Gümrük Beyannamesi        |
| `supplier_id`              | `UUID`          | Tedarikçi kimliği (yalnız `invoice_type = 0` satırlarında dolar). |
| `invoice_no`               | `VARCHAR(50)`   | Tedarikçi fatura numarası (yalnız `invoice_type = 0`).            |
| `invoice_date`             | `DATE`          | Fatura tarihi (yalnız `invoice_type = 0`).                        |
| `import_supplier_doc_no`   | `VARCHAR(50)`   | Gümrük belge numarası (yalnız `invoice_type = 1`).                |
| `import_supplier_doc_date` | `DATE`          | Gümrük belgesinin tescil tarihi (yalnız `invoice_type = 1`).      |
| `currency_id`              | `UUID`          | Belgenin para birimi (FK → `currency`).                           |
| `exchange_rate`            | `NUMERIC(12,6)` | Kaydedildiği günkü kur (yerel / döviz).                           |
| `status`                   | `SMALLINT`      | **0** = Taslak, **1** = Onaylı, **2** = Muhasebeleştirildi        |


## Tablo `purchase_line`

| Sütun                   | Veri Tipi         | Açıklama                                                                                         |
| ----------------------- | ----------------- | ------------------------------------------------------------------------------------------------ |
| `purchase_line_id`      | `UUID` **PK**     | Satır benzersiz kimliği.                                                                         |
| `purchase_invoice_id`   | `UUID`            | Bağlı fatura başlığı (FK → `purchase_invoice`).                                                  |
| `product_id`            | `UUID`            | Ürün/Stok kartı (FK → `product`).                                                                |
| `warehouse_id`          | `UUID`            | Giriş yapılacak depo (FK → `warehouse`).                                                         |
| `uom_id`                | `UUID`            | Miktar birimi (FK → `unit_of_measure`).                                                          |
| `qty`                   | `NUMERIC(18,4)`   | Satır miktarı.                                                                                   |
| `unit_price_fc`         | `NUMERIC(18,4)`   | İskonto öncesi birim fiyat (döviz).                                                              |
| `line_gross_fc`         | `NUMERIC(18,4)`   | **qty × unit_price_fc** &nbsp;→&nbsp; brüt tutar (döviz).                                        |
| `expense_amount_lc`     | `NUMERIC(18,4)`   | Navlun, sigorta vb. **paylaştırılmış masraf toplamı** (LC).                                      |
| `discount_rate`         | `NUMERIC(5,2)`    | Satır iskonto oranı %.                                                                           |
| `discount_amount_fc`    | `NUMERIC(18,4)`   | İskonto tutarı (döviz).                                                                          |
| `otv_rate`              | `NUMERIC(5,2)`    | ÖTV oranı %.                                                                                     |
| `otv_amount_lc`         | `NUMERIC(18,4)`   | Satır ÖTV tutarı (LC).                                                                           |
| `customs_rate`          | `NUMERIC(5,2)`    | Gümrük vergisi oranı %.                                                                          |
| `customs_amount_lc`     | `NUMERIC(18,4)`   | Satır gümrük vergisi (LC).                                                                       |
| `vat_rate`              | `NUMERIC(5,2)`    | KDV oranı %.                                                                                     |
| `vat_amount_lc`         | `NUMERIC(18,4)`   | Satır KDV tutarı (LC).                                                                           |
| `cost_price_lc`         | `NUMERIC(18,4)`   | Birim maliyet (iskonto + ÖTV + Gümrük + giderler, **KDV hariç**, LC).                            |
| `cost_amount_lc`        | `NUMERIC(18,4)`   | `qty × cost_price_lc` &nbsp;→&nbsp; toplam maliyet, **KDV hariç**.                               |
| `line_price`            | `NUMERIC(18,4)`   | Birim fiyat, **KDV dâhil** (LC).                                                                 |
| `line_total`            | `NUMERIC(18,4)`   | Satır toplamı, **KDV dâhil** (LC) = `cost_amount_lc + vat_amount_lc`.                            |



## Tablo `purchase_expense`

| Sütun                     | Veri Tipi         | Açıklama                                                                                           |
| ------------------------- | ----------------- | -------------------------------------------------------------------------------------------------- |
| `expense_id`              | `UUID` **PK**     | Masraf kaydının benzersiz kimliği (`gen_random_uuid()`).                                            |
| `purchase_invoice_id`     | `UUID`            | Bağlı **purchase_invoice** başlığı (FK, `ON DELETE CASCADE`).                                       |
| `supplier_id`             | `UUID`            | Masrafı faturalandıran tedarikçi (FK → `supplier`).                                                 |
| `supplier_invoice_no`     | `VARCHAR(50)`     | Masraf/taşıma faturasının numarası.                                                                 |
| `supplier_invoice_date`   | `DATE`            | Masraf faturasının tarihi.                                                                          |
| `expense_type`            | `SMALLINT`        | Masraf türü — **1** = Navlun • **2** = Sigorta • **3** = Elleçleme • **4** = Diğer.                 |
| `amount_lc`               | `NUMERIC(18,4)`   | **Net** masraf tutarı (KDV hariç, yerel para).                                                      |
| `vat_rate`                | `NUMERIC(5,2)`    | KDV oranı %.                                                                                        |
| `vat_amount_lc`           | `NUMERIC(18,4)`   | KDV tutarı (yerel para) = `amount_lc × vat_rate / 100`.                                             |
| `gross_amount_lc`¹        | `NUMERIC(18,4)`   | Net + KDV &nbsp;→&nbsp; **toplam fatura** tutarı (yerel para).                                      |
| `note`                    | `TEXT`            | Açıklama (örn. “İstanbul → Priština karayolu navlun”).                                              |
