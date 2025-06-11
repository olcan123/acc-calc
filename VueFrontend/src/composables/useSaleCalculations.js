/**
 * Sale Invoice Line hesaplama composable'ı
 * @param {Ref} vats - VAT array reactive referansı
 * @returns {Object} Hesaplama metodları ve utility fonksiyonları
 */
export function useSaleCalculations(vats) {
  /**
   * Sayısal değerleri belirli hassasiyetle formatlar (floating point precision sorunlarını önler)
   * @param {number} value - Formatlanacak değer
   * @param {number} precision - Ondalık basamak sayısı (varsayılan: 4)
   * @returns {number} Formatlanmış değer
   */  const formatPrecision = (value, precision = 4) => {
    if (!value || isNaN(value)) return 0;
    return Math.round(value * Math.pow(10, precision)) / Math.pow(10, precision);
  };  /**
   * VAT ID'den VAT oranını getir
   * @param {number} vatId - VAT ID (1: %0, 2: %8, 3: %18)
   * @returns {number} VAT oranı (decimal olarak, örn: 0.18)
   */
  const getVatRate = (vatId) => {
    if (!vats?.value || !Array.isArray(vats.value)) {
      return 0;
    }
    
    const vat = vats.value.find((v) => v.id === vatId);
    return vat ? vat.rate / 100 : 0;
  };

  /**
   * Temel hesaplamaları yapan ana fonksiyon
   * @param {Object} lineData - Fatura satırı verisi
   * @param {string} skipField - Bu alanı yeniden hesaplama (manuel değiştirildiği için)
   * @returns {Object} Hesaplanmış değerler
   */  const calculateLineValues = (lineData, skipField = null) => {
    const quantity = parseFloat(lineData.quantity) || 0;
    const unitPrice = parseFloat(lineData.unitPrice) || 0;
    const discountRate = parseFloat(lineData.discountRate) || 0;
    const vatRate = getVatRate(lineData.vatId);

    // Amount = Quantity × Unit Price
    const amount = formatPrecision(quantity * unitPrice, 4);

    // Discount Amount = Amount × (Discount Rate / 100)
    const discountAmount = formatPrecision(amount * (discountRate / 100), 4);

    // VAT Amount = (Amount - Discount Amount) × VAT Rate
    const vatTaxAmount = formatPrecision((amount - discountAmount) * vatRate, 4);

    // Total Price = Unit Price + (Unit Price × VAT Rate) - KDV dahil birim fiyat
    const totalPrice = formatPrecision(unitPrice + (unitPrice * vatRate), 2);

    // Total Amount = Amount - Discount Amount + VAT Amount
    const totalAmount = formatPrecision(amount - discountAmount + vatTaxAmount, 2);

    return {
      quantity: formatPrecision(quantity, 4),
      unitPrice: formatPrecision(unitPrice, 4),
      discountRate: formatPrecision(discountRate, 2),
      amount: formatPrecision(amount, 4),
      discountAmount: formatPrecision(discountAmount, 4),
      vatTaxAmount: formatPrecision(vatTaxAmount, 4),
      totalPrice: formatPrecision(totalPrice, 2),
      totalAmount: formatPrecision(totalAmount, 2),    };
  };

  /**
   * Tek bir değer değiştiğinde tüm hesaplamaları güncelle
   * @param {Function} setFieldValue - VeeValidate setFieldValue fonksiyonu
   * @param {number} index - Satır indeksi
   * @param {Object} currentValues - Mevcut form değerleri
   * @param {string} changedField - Değişen alan adı
   */
  const updateCalculations = (
    setFieldValue,
    index,
    currentValues,
    changedField = null
  ) => {
    const lineData = currentValues.saleInvoiceLines?.[index];
    if (!lineData) return;

    const calculated = calculateLineValues(lineData, changedField);    // Sadece hesaplanan değerleri güncelle (manuel değiştirilen alanı atla)
    Object.keys(calculated).forEach((key) => {
      if (key !== changedField) {
        setFieldValue(`saleInvoiceLines[${index}].${key}`, calculated[key]);
      }
    });
  };

  /**
   * Quantity değiştiğinde hesaplamalar
   */  const onQuantityChange = (
    setFieldValue,
    index,
    currentValues,
    newQuantity
  ) => {
    setFieldValue(`saleInvoiceLines[${index}].quantity`, parseFloat(newQuantity) || 0);
    updateCalculations(setFieldValue, index, currentValues, "quantity");
  };

  /**
   * Unit Price değiştiğinde hesaplamalar
   */  const onUnitPriceChange = (
    setFieldValue,
    index,
    currentValues,
    newUnitPrice
  ) => {
    setFieldValue(`saleInvoiceLines[${index}].unitPrice`, parseFloat(newUnitPrice) || 0);
    updateCalculations(setFieldValue, index, currentValues, "unitPrice");
  };

  /**
   * Discount Rate değiştiğinde hesaplamalar
   */  const onDiscountRateChange = (
    setFieldValue,
    index,
    currentValues,
    newDiscountRate
  ) => {
    setFieldValue(`saleInvoiceLines[${index}].discountRate`, parseFloat(newDiscountRate) || 0);
    updateCalculations(setFieldValue, index, currentValues, "discountRate");
  };

  /**
   * VAT değiştiğinde hesaplamalar
   */  const onVatChange = (setFieldValue, index, currentValues, newVatId) => {
    setFieldValue(`saleInvoiceLines[${index}].vatId`, newVatId);
    updateCalculations(setFieldValue, index, currentValues, "vatId");
  };

  /**
   * Amount değiştiğinde unit price hesaplaması (quantity sabit kaldığında)
   * Amount = Quantity × Unit Price formülünden Unit Price = Amount / Quantity
   */  const onAmountChange = (setFieldValue, index, currentValues, newAmount) => {
    const quantity = parseFloat(currentValues.saleInvoiceLines?.[index]?.quantity) || 1;
    const newUnitPrice = parseFloat(newAmount) / quantity;
    
    setFieldValue(`saleInvoiceLines[${index}].amount`, parseFloat(newAmount) || 0);
    setFieldValue(`saleInvoiceLines[${index}].unitPrice`, formatPrecision(newUnitPrice, 4));
    updateCalculations(setFieldValue, index, currentValues, "amount");
  };  /**
   * Total Price değiştiğinde hesaplama (KDV dahil fiyattan KDV hariç hesaplama)
   * Total Price = Unit Price + VAT olduğu için ters hesaplama yapar
   */
  const onTotalPriceChange = (
    setFieldValue,
    index,
    currentValues,
    newTotalPrice
  ) => {
    const quantity = parseFloat(currentValues.saleInvoiceLines?.[index]?.quantity) || 1;
    const vatId = currentValues.saleInvoiceLines?.[index]?.vatId;
    const vatRate = getVatRate(vatId);
    const discountRate = parseFloat(currentValues.saleInvoiceLines?.[index]?.discountRate) || 0;
    
    // Total Price = Unit Price + (Unit Price × VAT Rate)
    // Total Price = Unit Price × (1 + VAT Rate)
    // Unit Price = Total Price / (1 + VAT Rate)
    const newUnitPrice = parseFloat(newTotalPrice) / (1 + vatRate);
    const newAmount = newUnitPrice * quantity;
    
    // Discount Amount = Amount × (Discount Rate / 100)
    const discountAmount = formatPrecision(newAmount * (discountRate / 100), 4);
    
    // VAT Amount = (Amount - Discount Amount) × VAT Rate
    const vatTaxAmount = formatPrecision((newAmount - discountAmount) * vatRate, 4);
    
    // Total Amount = Amount - Discount Amount + VAT Amount
    const totalAmount = formatPrecision(newAmount - discountAmount + vatTaxAmount, 2);
    
    // Tüm değerleri güncelle
    setFieldValue(`saleInvoiceLines[${index}].unitPrice`, formatPrecision(newUnitPrice, 4));
    setFieldValue(`saleInvoiceLines[${index}].amount`, formatPrecision(newAmount, 4));
    setFieldValue(`saleInvoiceLines[${index}].discountAmount`, discountAmount);
    setFieldValue(`saleInvoiceLines[${index}].vatTaxAmount`, vatTaxAmount);
    setFieldValue(`saleInvoiceLines[${index}].totalPrice`, formatPrecision(newTotalPrice, 2));
    setFieldValue(`saleInvoiceLines[${index}].totalAmount`, totalAmount);
  };
  /**
   * Total Amount değiştiğinde hesaplama
   * Total Amount'tan geriye doğru hesaplama yapar
   */  const onTotalAmountChange = (
    setFieldValue,
    index,
    currentValues,
    newTotalAmount
  ) => {
    const quantity = parseFloat(currentValues.saleInvoiceLines?.[index]?.quantity) || 1;
    const vatId = currentValues.saleInvoiceLines?.[index]?.vatId;
    const vatRate = getVatRate(vatId);
    const discountRate = parseFloat(currentValues.saleInvoiceLines?.[index]?.discountRate) || 0;
    
    // Total Amount = Amount - Discount Amount + VAT Amount
    // Total Amount = Amount - (Amount × Discount Rate / 100) + ((Amount - Discount Amount) × VAT Rate)
    // Total Amount = Amount × (1 - Discount Rate / 100) × (1 + VAT Rate)
    // Amount = Total Amount / ((1 - Discount Rate / 100) × (1 + VAT Rate))
    
    const discountMultiplier = 1 - (discountRate / 100);
    const vatMultiplier = 1 + vatRate;
    const totalMultiplier = discountMultiplier * vatMultiplier;
    
    const newAmount = parseFloat(newTotalAmount) / totalMultiplier;
    const newUnitPrice = newAmount / quantity;
    
    // Hesaplanan değerleri set et
    const discountAmount = formatPrecision(newAmount * (discountRate / 100), 4);
    const vatTaxAmount = formatPrecision((newAmount - discountAmount) * vatRate, 4);
    const totalPrice = formatPrecision(newUnitPrice + (newUnitPrice * vatRate), 2);
    
    // Tüm değerleri güncelle
    setFieldValue(`saleInvoiceLines[${index}].unitPrice`, formatPrecision(newUnitPrice, 4));
    setFieldValue(`saleInvoiceLines[${index}].amount`, formatPrecision(newAmount, 4));
    setFieldValue(`saleInvoiceLines[${index}].discountAmount`, discountAmount);
    setFieldValue(`saleInvoiceLines[${index}].vatTaxAmount`, vatTaxAmount);
    setFieldValue(`saleInvoiceLines[${index}].totalPrice`, totalPrice);
    setFieldValue(`saleInvoiceLines[${index}].totalAmount`, formatPrecision(newTotalAmount, 2));
  };
  /**
   * Legacy method for backward compatibility
   * @param {Object} lineData - Fatura satırı verisi
   * @returns {Object} Hesaplanmış değerler
   */
  const calculateLineTotal = (lineData) => {
    return calculateLineValues(lineData);
  };

  /**
   * Tüm satırlar için toplam hesaplamaları
   * @param {Array} saleInvoiceLines - Satış fatura satırları
   * @returns {Object} Toplam değerler
   */
  const calculateTotals = (saleInvoiceLines = []) => {
    const totals = saleInvoiceLines.reduce(
      (acc, line) => {
        acc.totalAmount += line.amount || 0;
        acc.totalDiscountAmount += line.discountAmount || 0;
        acc.netAmount += (line.amount || 0) - (line.discountAmount || 0);
        acc.totalVatAmount += line.vatTaxAmount || 0;
        acc.grandTotal += line.totalAmount || 0;
        return acc;
      },
      {
        totalAmount: 0,
        totalDiscountAmount: 0,
        netAmount: 0,
        totalVatAmount: 0,
        grandTotal: 0,
      }
    );

    // Decimal precision
    Object.keys(totals).forEach((key) => {
      totals[key] = formatPrecision(totals[key], 4);
    });

    return totals;
  };

  return {
    formatPrecision,
    getVatRate,
    calculateLineValues,
    calculateLineTotal, // Legacy method
    calculateTotals, // Add calculateTotals function
    updateCalculations,
    onQuantityChange,
    onUnitPriceChange,
    onDiscountRateChange,
    onVatChange,
    onAmountChange,
    onTotalPriceChange,
    onTotalAmountChange,
  };
}
