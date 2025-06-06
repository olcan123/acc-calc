import { computed } from "vue";

/**
 * Purchase Invoice Line hesaplama composable'ı
 * @param {Object} vatStore - VAT store referansı
 * @returns {Object} Hesaplama metodları ve utility fonksiyonları
 */
export function usePurchaseCalculations(vatStore) {
  /**
   * VAT ID'den VAT oranını getir
   * @param {number} vatId - VAT ID (1: %0, 2: %8, 3: %18)   * @returns {number} VAT oranı (decimal olarak, örn: 0.18)
   */
  const getVatRate = (vatId) => {
    if (!vatId) return 0;

    // VAT store'dan dinamik olarak VAT oranını al
    if (vatStore?.vats?.value && Array.isArray(vatStore.vats.value)) {
      const vatData = vatStore.vats.value.find(
        (vat) => vat.id === Number(vatId)
      );
      if (vatData && vatData.rate !== undefined) {
        return vatData.rate / 100; // Rate backend'de % olarak tutuluyor, decimal'e çevir
      }
    }
    // Fallback: Sabit VAT oranları (eğer store'dan alınamazsa)
    const vatRates = {
      1: 0, // %0
      2: 0.08, // %8
      3: 0.18, // %18
    };

    return vatRates[vatId] || 0;
  };

  /**
   * Temel hesaplamaları yapan ana fonksiyon
   * @param {Object} lineData - Fatura satırı verisi
   * @param {string} skipField - Bu alanı yeniden hesaplama (manuel değiştirildiği için)
   * @returns {Object} Hesaplanmış değerler
   */  const calculateLineValues = (lineData, skipField = null) => {
    const {
      quantity = 0,
      unitPrice = 0,
      discountRate = 0,
      exciseTaxRate = 0,      // ÖTV oranı
      customsRate = 0,        // Gümrük oranı
      vatId = null,
      expenseAmount = 0,      // Expense amount'u da dahil et
      amount: manualAmount = null,
      totalPrice: manualTotalPrice = null,
      totalAmount: manualTotalAmount = null,
      costPrice: manualCostPrice = null,
      costAmount: manualCostAmount = null,
    } = lineData;    // 1. Amount hesaplama (manuel değiştirilmediyse quantity × unitPrice)
    let amount;
    if (skipField === "amount" && manualAmount !== null) {
      // Amount manuel olarak değiştirildi, mevcut değeri kullan
      amount = manualAmount;
    } else {
      // Normal hesaplama: quantity × unitPrice
      amount = quantity * unitPrice;
    }    // 2. Discount hesaplamaları
    let discountAmount = amount * (discountRate / 100);

    // 3. Import tax hesaplamaları (ÖTV ve Gümrük)
    // Tax base = amount + expenseAmount (doğru hesaplama)
    const taxBase = amount + expenseAmount;
    const exciseTaxAmount = taxBase * (exciseTaxRate / 100);
    const customsAmount = taxBase * (customsRate / 100);

    // 4. Cost hesaplamaları
    let costPrice, costAmount;

    if (skipField === "costPrice" && manualCostPrice !== null) {
      // Cost Price manuel olarak değiştirildi
      costPrice = manualCostPrice;
      costAmount = amount - discountAmount + expenseAmount + exciseTaxAmount + customsAmount;
    } else if (skipField === "costAmount" && manualCostAmount !== null) {
      // Cost Amount manuel olarak değiştirildi
      costAmount = manualCostAmount;
      // Cost Price, tüm vergi ve masraflar dahil edilmiş cost amount'tan hesaplanır
      costPrice = quantity > 0 ? (costAmount - expenseAmount - exciseTaxAmount - customsAmount + discountAmount) / quantity : 0;
    } else {
      // Normal hesaplama
      costPrice = unitPrice - (unitPrice * (discountRate / 100)) + (exciseTaxAmount / (quantity || 1)) + (customsAmount / (quantity || 1)) + (expenseAmount / (quantity || 1));
      costAmount = amount - discountAmount + expenseAmount + exciseTaxAmount + customsAmount;
    }    // 5. VAT hesaplamaları (cost amount üzerinden)
    const vatRate = getVatRate(vatId);
    const vatTaxAmount = costAmount * vatRate;

    // 6. Total hesaplamaları (Cost Amount + VAT bazında)
    let totalPrice, totalAmount;

    if (skipField === "totalPrice" && manualTotalPrice !== null) {
      // Total Price manuel olarak değiştirildi
      totalPrice = manualTotalPrice;
      totalAmount = costAmount + vatTaxAmount; // Normal hesaplama
    } else if (skipField === "totalAmount" && manualTotalAmount !== null) {
      // Total Amount manuel olarak değiştirildi
      totalAmount = manualTotalAmount;
      // Total Price, cost amount + VAT bazında hesaplanır
      totalPrice = quantity > 0 ? (costAmount + vatTaxAmount) / quantity : 0;
    } else {
      // Normal hesaplama - Total Price birim bazında cost amount + VAT
      totalPrice = quantity > 0 ? (costAmount + vatTaxAmount) / quantity : 0;
      totalAmount = costAmount + vatTaxAmount;
    }

    return {
      amount: parseFloat(amount.toFixed(4)),
      discountAmount: parseFloat(discountAmount.toFixed(4)),
      exciseTaxAmount: parseFloat(exciseTaxAmount.toFixed(4)),
      customsAmount: parseFloat(customsAmount.toFixed(4)),
      costPrice: parseFloat(costPrice.toFixed(4)),
      costAmount: parseFloat(costAmount.toFixed(4)),
      vatTaxAmount: parseFloat(vatTaxAmount.toFixed(4)),
      totalPrice: parseFloat(totalPrice.toFixed(4)),
      totalAmount: parseFloat(totalAmount.toFixed(4))
    };
  };
  /**
   * Tek bir değer değiştiğinde tüm hesaplamaları güncelle
   * @param {Function} setFieldValue - VeeValidate setFieldValue fonksiyonu
   * @param {number} index - Satır indeksi
   * @param {Object} currentValues - Mevcut form değerleri
   * @param {string} changedField - Değişen alan adı
   */ const updateCalculations = (
    setFieldValue,
    index,
    currentValues,
    changedField = null
  ) => {
    const lineData = currentValues.purchaseInvoiceLines?.[index];
    if (!lineData) return;

    const calculated = calculateLineValues(lineData, changedField);    // Sadece değişmeyen alanları güncelle (kullanıcı input'u korunur)
    const fieldsToUpdate = {
      amount: calculated.amount,
      discountAmount: calculated.discountAmount,
      exciseTaxAmount: calculated.exciseTaxAmount,
      customsAmount: calculated.customsAmount,
      costPrice: calculated.costPrice,
      costAmount: calculated.costAmount,
      vatTaxAmount: calculated.vatTaxAmount,
      totalPrice: calculated.totalPrice,
      totalAmount: calculated.totalAmount,
    };

    // Eğer kullanıcı bu alanı manuel değiştiriyorsa, o alanı güncelleme
    if (changedField && fieldsToUpdate[changedField] !== undefined) {
      delete fieldsToUpdate[changedField];
    } // Tüm hesaplanmış değerleri güncelle
    Object.entries(fieldsToUpdate).forEach(([field, value]) => {
      setFieldValue(`purchaseInvoiceLines[${index}].${field}`, value);
    });
  };

  /**
   * Quantity değiştiğinde hesaplamalar
   */
  const onQuantityChange = (
    setFieldValue,
    index,
    currentValues,
    newQuantity
  ) => {
    setFieldValue(`purchaseInvoiceLines[${index}].quantity`, newQuantity);
    updateCalculations(setFieldValue, index, currentValues, "quantity");
  };

  /**
   * Unit Price değiştiğinde hesaplamalar
   */
  const onUnitPriceChange = (
    setFieldValue,
    index,
    currentValues,
    newUnitPrice
  ) => {
    setFieldValue(`purchaseInvoiceLines[${index}].unitPrice`, newUnitPrice);
    updateCalculations(setFieldValue, index, currentValues, "unitPrice");
  };

  /**
   * Discount Rate değiştiğinde hesaplamalar
   */
  const onDiscountRateChange = (
    setFieldValue,
    index,
    currentValues,
    newDiscountRate
  ) => {
    setFieldValue(
      `purchaseInvoiceLines[${index}].discountRate`,
      newDiscountRate
    );
    updateCalculations(setFieldValue, index, currentValues, "discountRate");
  };
  /**
   * VAT değiştiğinde hesaplamalar
   */
  const onVatChange = (setFieldValue, index, currentValues, newVatId) => {
    setFieldValue(`purchaseInvoiceLines[${index}].vatId`, newVatId);
    setTimeout(() => {
      updateCalculations(setFieldValue, index, currentValues, "vatId");
    }, 50); // VAT değişikliğinden sonra hesaplamaları güncelle
  };
  /**
   * Amount değiştiğinde unit price hesaplaması (quantity sabit kaldığında)
   * Amount = Quantity × Unit Price formülünden Unit Price = Amount / Quantity
   */
  const onAmountChange = (setFieldValue, index, currentValues, newAmount) => {
    // Amount değerini ayarla
    setFieldValue(`purchaseInvoiceLines[${index}].amount`, newAmount);

    // Mevcut quantity değerini al
    const lineData = currentValues.purchaseInvoiceLines?.[index];
    const quantity = lineData?.quantity || 1; // 0'a bölme hatasını önlemek için minimum 1

    // Unit Price'ı yeniden hesapla: unitPrice = amount / quantity
    const newUnitPrice = quantity > 0 ? newAmount / quantity : 0;
    // Unit Price'ı güncelle
    setFieldValue(
      `purchaseInvoiceLines[${index}].unitPrice`,
      parseFloat(newUnitPrice.toFixed(4))
    );

    // Diğer hesaplamaları güncelle (amount değişikliğini hariç tut)
    setTimeout(() => {
      updateCalculations(setFieldValue, index, currentValues, "amount");
    }, 50);
  };
  /**
   * Total Price değiştiğinde hesaplama (discount rate sıfırlanır)
   * Total Price = Cost Price + VAT olduğu için ters hesaplama yapar
   */
  const onTotalPriceChange = (
    setFieldValue,
    index,
    currentValues,
    newTotalPrice
  ) => {
    // Total Price değerini ayarla
    setFieldValue(`purchaseInvoiceLines[${index}].totalPrice`, newTotalPrice);

    // Discount rate'i sıfırla (total price manuel değiştirildiğinde)
    setFieldValue(`purchaseInvoiceLines[${index}].discountRate`, 0);

    const lineData = currentValues.purchaseInvoiceLines?.[index];
    const vatRate = getVatRate(lineData?.vatId);

    // Total Price'dan Cost Price'ı hesapla: costPrice = totalPrice / (1 + vatRate)
    const newCostPrice =
      vatRate > 0 ? newTotalPrice / (1 + vatRate) : newTotalPrice;
    // Cost Price'ı güncelle (bu Unit Price olacak çünkü discount rate = 0)
    setFieldValue(
      `purchaseInvoiceLines[${index}].unitPrice`,
      parseFloat(newCostPrice.toFixed(4))
    );

    // Diğer hesaplamaları güncelle (totalPrice değişikliğini hariç tut)
    setTimeout(() => {
      updateCalculations(setFieldValue, index, currentValues, "totalPrice");
    }, 50);
  };
  /**
   * Total Amount değiştiğinde hesaplama (discount rate sıfırlanır)
   * Total Amount = Cost Amount + VAT Amount olduğu için ters hesaplama yapar
   */
  const onTotalAmountChange = (
    setFieldValue,
    index,
    currentValues,
    newTotalAmount
  ) => {
    // Total Amount değerini ayarla
    setFieldValue(`purchaseInvoiceLines[${index}].totalAmount`, newTotalAmount);

    // Discount rate'i sıfırla (total amount manuel değiştirildiğinde)
    setFieldValue(`purchaseInvoiceLines[${index}].discountRate`, 0);

    const lineData = currentValues.purchaseInvoiceLines?.[index];
    const vatRate = getVatRate(lineData?.vatId);
    const quantity = lineData?.quantity || 1;

    // Total Amount'tan Cost Amount'ı hesapla: costAmount = totalAmount / (1 + vatRate)
    const newCostAmount =
      vatRate > 0 ? newTotalAmount / (1 + vatRate) : newTotalAmount;

    // Cost Amount'tan Amount'ı hesapla (discount = 0 olduğu için costAmount = amount)
    const newAmount = newCostAmount;

    // Amount'tan Unit Price'ı hesapla: unitPrice = amount / quantity
    const newUnitPrice = quantity > 0 ? newAmount / quantity : 0;
    // Değerleri güncelle
    setFieldValue(
      `purchaseInvoiceLines[${index}].amount`,
      parseFloat(newAmount.toFixed(4))
    );
    setFieldValue(
      `purchaseInvoiceLines[${index}].unitPrice`,
      parseFloat(newUnitPrice.toFixed(4))
    );

    // Diğer hesaplamaları güncelle (totalAmount değişikliğini hariç tut)
    setTimeout(() => {
      updateCalculations(setFieldValue, index, currentValues, "totalAmount");
    }, 50);
  };
  /**
   * Cost Price değiştiğinde hesaplama (discount rate sıfırlanır)
   * Cost Price manuel değiştirildiğinde Unit Price'ı hesaplar
   */
  const onCostPriceChange = (
    setFieldValue,
    index,
    currentValues,
    newCostPrice
  ) => {
    // Cost Price değerini ayarla
    setFieldValue(`purchaseInvoiceLines[${index}].costPrice`, newCostPrice);

    // Discount rate'i sıfırla (cost price manuel değiştirildiğinde)
    setFieldValue(`purchaseInvoiceLines[${index}].discountRate`, 0);
    // Cost Price = Unit Price - Discount olduğu için ve discount = 0 olduğunda
    // Cost Price = Unit Price olur
    setFieldValue(
      `purchaseInvoiceLines[${index}].unitPrice`,
      parseFloat(newCostPrice.toFixed(4))
    );

    // Diğer hesaplamaları güncelle (costPrice değişikliğini hariç tut)
    setTimeout(() => {
      updateCalculations(setFieldValue, index, currentValues, "costPrice");
    }, 50);
  };
  /**
   * Cost Amount değiştiğinde hesaplama (discount rate sıfırlanır)
   * Cost Amount manuel değiştirildiğinde Amount ve Unit Price'ı hesaplar
   */
  const onCostAmountChange = (
    setFieldValue,
    index,
    currentValues,
    newCostAmount
  ) => {
    // Cost Amount değerini ayarla
    setFieldValue(`purchaseInvoiceLines[${index}].costAmount`, newCostAmount);

    // Discount rate'i sıfırla (cost amount manuel değiştirildiğinde)
    setFieldValue(`purchaseInvoiceLines[${index}].discountRate`, 0);

    const lineData = currentValues.purchaseInvoiceLines?.[index];
    const quantity = lineData?.quantity || 1;

    // Cost Amount = Amount - Discount Amount olduğu için ve discount = 0 olduğunda
    // Cost Amount = Amount olur
    const newAmount = newCostAmount;
    // Amount'tan Unit Price'ı hesapla: unitPrice = amount / quantity
    const newUnitPrice = quantity > 0 ? newAmount / quantity : 0;

    // Değerleri güncelle
    setFieldValue(
      `purchaseInvoiceLines[${index}].amount`,
      parseFloat(newAmount.toFixed(4))
    );
    setFieldValue(
      `purchaseInvoiceLines[${index}].unitPrice`,
      parseFloat(newUnitPrice.toFixed(4))
    );

    // Diğer hesaplamaları güncelle (costAmount değişikliğini hariç tut)
    setTimeout(() => {
      updateCalculations(setFieldValue, index, currentValues, "costAmount");
    }, 50);
  };

  /**
   * Tüm satırlar için toplam hesaplamaları
   */
  const calculateTotals = (purchaseInvoiceLines = []) => {
    const totals = purchaseInvoiceLines.reduce(
      (acc, line) => {
        acc.totalAmount += line.amount || 0;
        acc.totalDiscountAmount += line.discountAmount || 0;
        acc.totalCostAmount += line.costAmount || 0;
        acc.totalVatAmount += line.vatTaxAmount || 0;
        acc.grandTotal += line.totalAmount || 0;
        return acc;
      },
      {
        totalAmount: 0,
        totalDiscountAmount: 0,
        totalCostAmount: 0,
        totalVatAmount: 0,
        grandTotal: 0,
      }
    ); // Decimal precision
    Object.keys(totals).forEach((key) => {
      totals[key] = parseFloat(totals[key].toFixed(4));
    });

    return totals;
  };

  /**
   * VAT oranlarını formatla (display için)
   */
  const formatVatRate = (vatId) => {
    const rate = getVatRate(vatId);
    return `%${(rate * 100).toFixed(0)}`;
  };
  /**
   * Expense dağılımını hesapla ve invoice lines'a proportional olarak dağıt
   * @param {Array} purchaseInvoiceLines - Fatura satırları
   * @param {Array} purchaseInvoiceExpenses - Masraf/gider kayıtları
   * @returns {Array} Güncellenmiş fatura satırları (expenseAmount ve costAmount ile)
   */
  const distributeExpenseAmounts = (
    purchaseInvoiceLines = [],
    purchaseInvoiceExpenses = []
  ) => {
    // Toplam expense amount'u hesapla
    const totalExpenseAmount = purchaseInvoiceExpenses.reduce(
      (sum, expense) => {
        return sum + (expense.amount || 0);
      },
      0
    );

    // Eğer hiç masraf yoksa, tüm expenseAmount'ları 0 yap
    if (totalExpenseAmount <= 0) {
      return purchaseInvoiceLines.map((line) => ({
        ...line,
        expenseAmount: 0,
        costAmount: line.amount || 0, // costAmount = amount + 0
      }));
    }

    // Toplam invoice amount'u hesapla (proportional dağıtım için)
    const totalInvoiceAmount = purchaseInvoiceLines.reduce((sum, line) => {
      return sum + (line.amount || 0);
    }, 0);

    // Eğer toplam fatura tutarı 0 ise, masrafları eşit dağıt
    if (totalInvoiceAmount <= 0) {
      const equalExpensePerLine =
        totalExpenseAmount / purchaseInvoiceLines.length;
      return purchaseInvoiceLines.map((line) => ({
        ...line,
        expenseAmount: parseFloat(equalExpensePerLine.toFixed(4)),
        costAmount: parseFloat(
          ((line.amount || 0) + equalExpensePerLine).toFixed(4)
        ),
      }));
    }

    // Proportional dağıtım yap
    let distributedExpenseTotal = 0;
    const updatedLines = purchaseInvoiceLines.map((line, index) => {
      let expenseAmount = 0;

      // Son satır için kalan tutarı kullan (rounding error'ları önlemek için)
      if (index === purchaseInvoiceLines.length - 1) {
        expenseAmount = totalExpenseAmount - distributedExpenseTotal;
      } else {
        // Her satır için proportional expense amount hesapla
        const lineRatio = (line.amount || 0) / totalInvoiceAmount;
        expenseAmount = totalExpenseAmount * lineRatio;
        distributedExpenseTotal += expenseAmount;
      }

      // Precision kontrolü
      expenseAmount = parseFloat(expenseAmount.toFixed(4));
      const costAmount = parseFloat(
        ((line.amount || 0) + expenseAmount).toFixed(4)
      );

      return {
        ...line,
        expenseAmount,
        costAmount,
      };
    });

    return updatedLines;
  };

  /**
   * Form values'ı güncelle - expense dağılımı sonrası
   * @param {Object} formValues - Form values objesi
   * @param {Function} setFieldValue - VeeValidate setFieldValue fonksiyonu
   */ const updateExpenseDistribution = (formValues, setFieldValue) => {
    const { purchaseInvoiceLines = [], purchaseInvoiceExpenses = [] } =
      formValues;

    // Expense dağılımını hesapla
    const updatedLines = distributeExpenseAmounts(
      purchaseInvoiceLines,
      purchaseInvoiceExpenses
    );

    // Her satır için güncellenmiş değerleri form'a set et ve hesaplamaları yeniden yap
    updatedLines.forEach((line, index) => {
      setFieldValue(
        `purchaseInvoiceLines[${index}].expenseAmount`,
        line.expenseAmount
      );

      // Expense amount değiştiği için tüm hesaplamaları yeniden yap
      setTimeout(() => {
        updateCalculations(
          setFieldValue,
          index,
          { purchaseInvoiceLines: updatedLines },
          null
        );
      }, 100);
    });

    return updatedLines;
  };
  return {
    // Ana hesaplama fonksiyonları
    calculateLineValues,
    updateCalculations,
    calculateTotals,

    // Expense dağılım fonksiyonları
    distributeExpenseAmounts,
    updateExpenseDistribution,

    // Olay işleyicileri
    onQuantityChange,
    onUnitPriceChange,
    onDiscountRateChange,
    onVatChange,
    onAmountChange,
    onTotalPriceChange,
    onTotalAmountChange,
    onCostPriceChange,
    onCostAmountChange,

    // Utility fonksiyonları
    getVatRate,
    formatVatRate,
  };
}
