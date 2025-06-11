import { computed } from "vue";

/**
 * Purchase Invoice Line hesaplama composable'ı
 * @param {Object} vatStore - VAT store referansı
 * @returns {Object} Hesaplama metodları ve utility fonksiyonları
 */
export function usePurchaseCalculations(vatStore) {  /**
   * Sayısal değerleri belirli hassasiyetle formatlar (floating point precision sorunlarını önler)
   * @param {number} value - Formatlanacak değer
   * @param {number} precision - Ondalık basamak sayısı (varsayılan: 4)
   * @returns {number} Formatlanmış değer
   */  
  const formatPrecision = (value, precision = 4) => {
    // Null, undefined, empty string kontrolü
    if (value === null || value === undefined || value === "" || value === 0)
      return 0;

    // Number'a dönüştür
    const numValue = Number(value);

    // NaN kontrolü
    if (isNaN(numValue)) return 0;

    // ULTRA PRECISION FIX: Çok hassas rounding için multiple approach
    // Approach 1: Number.EPSILON ile düzeltme
    const epsilon = Number.EPSILON;
    const multiplier = Math.pow(10, precision);
    
    // Approach 2: String-based rounding (en hassas)
    const stringRounded = numValue.toFixed(precision + 2);
    const preRounded = parseFloat(stringRounded);
    
    // Approach 3: Mathematical rounding with epsilon
    const rounded = Math.round((preRounded + epsilon) * multiplier) / multiplier;
    
    // Final result: parseFloat ile clean number döndür
    return parseFloat(rounded.toFixed(precision));
  };

  /**
   * Döviz kuru dönüşümü yapan yardımcı fonksiyon
   * @param {number} value - Dönüştürülecek değer
   * @param {number} exchangeRate - Döviz kuru (varsayılan: 1)
   * @returns {number} Dönüştürülmüş değer
   */
  const convertCurrency = (value, exchangeRate = 1) => {
    if (!value || !exchangeRate || exchangeRate <= 0) return value || 0;
    return value * exchangeRate;
  };
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
  };  /**
   * Temel hesaplamaları yapan ana fonksiyon
   * @param {Object} lineData - Fatura satırı verisi
   * @param {string} skipField - Bu alanı yeniden hesaplama (manuel değiştirildiği için)
   * @param {number} exchangeRate - Döviz kuru (opsiyonel, varsayılan: 1)
   * @returns {Object} Hesaplanmış değerler
   */ 
  const calculateLineValues = (
    lineData,
    skipField = null,
    exchangeRate = 1
  ) => {
    const {
      quantity = 0,
      unitPrice = 0,
      discountRate = 0,
      exciseTaxRate = 0,
      customsRate = 0,
      vatId = null,
      expenseAmount = 0,
      amount: manualAmount = null,
      totalPrice: manualTotalPrice = null,
      totalAmount: manualTotalAmount = null,
      costPrice: manualCostPrice = null,
      costAmount: manualCostAmount = null,
    } = lineData;

    // DATABASE PRECISION PROTECTION: Eğer skipField yoksa ve totalAmount var ise (database'den gelmiş)
    // o değeri exact olarak koru ve diğer değerleri ona göre hesapla
    const isDatabaseLoad = !skipField && manualTotalAmount !== null && manualTotalAmount !== undefined;
    
    // DEBUG: Database load kontrolü
    if (isDatabaseLoad) {
      console.log('💾 Database precision protection active:', {
        manualTotalAmount: manualTotalAmount,
        skipField: skipField,
        mode: 'DATABASE_LOAD'
      });
    }

    // Basic amount calculation
    const convertedUnitPrice = convertCurrency(unitPrice, exchangeRate);

    let amount;
    if (skipField === "amount" && manualAmount !== null) {
      amount = convertCurrency(manualAmount, exchangeRate);
    } else {
      amount = quantity * convertedUnitPrice;
    }

    // Discount hesaplamaları
    let discountAmount = amount * (discountRate / 100);

    // Cost Price hesaplaması
    let costPrice;
    if (skipField === "costPrice" && manualCostPrice !== null) {
      costPrice = manualCostPrice;
    } else {
      // STEP 1: UnitPrice × ExchangeRate
      costPrice = convertedUnitPrice;

      // Apply discount
      costPrice = costPrice - costPrice * (discountRate / 100);

      // STEP 2: Add expense per unit
      const expensePerUnit = quantity > 0 ? expenseAmount / quantity : 0;
      costPrice = costPrice + expensePerUnit;

      // STEP 3: Add customs and excise taxes
      const customsTaxOnCostPrice = costPrice * (customsRate / 100);
      const exciseTaxOnCostPrice = costPrice * (exciseTaxRate / 100);
      costPrice = costPrice + customsTaxOnCostPrice + exciseTaxOnCostPrice;
    }

    // Cost Amount calculation
    let costAmount;
    if (skipField === "costAmount" && manualCostAmount !== null) {
      costAmount = manualCostAmount;
      costPrice = quantity > 0 ? costAmount / quantity : 0;
    } else {
      costAmount = costPrice * quantity;
    }

    // Tax amounts for display
    const baseCostPriceForTax =
      convertedUnitPrice -
      convertedUnitPrice * (discountRate / 100) +
      (quantity > 0 ? expenseAmount / quantity : 0);
    const exciseTaxAmount =
      baseCostPriceForTax * (exciseTaxRate / 100) * quantity;
    const customsAmount = baseCostPriceForTax * (customsRate / 100) * quantity;    // VAT calculations
    const vatRate = getVatRate(vatId);
      // Total calculations - precision-safe hesaplama
    let totalPrice, totalAmount, vatTaxAmount;
    if (skipField === "totalPrice" && manualTotalPrice !== null) {
      totalPrice = manualTotalPrice;
      totalAmount = costAmount + (costAmount * vatRate);
      vatTaxAmount = costAmount * vatRate;
    } else if (skipField === "totalAmount" && manualTotalAmount !== null) {
      // Kullanıcının manuel girdiği totalAmount değerini hiç formatlamadan koru
      totalAmount = parseFloat(manualTotalAmount);
      totalPrice = quantity > 0 ? totalAmount / quantity : 0;
      // VAT amount'u consistent şekilde hesapla
      vatTaxAmount = totalAmount - costAmount;    
    } else if (isDatabaseLoad) {
      // DATABASE PROTECTION: Database'den gelen değerleri exact koru
      totalAmount = parseFloat(manualTotalAmount);
      totalPrice = quantity > 0 ? totalAmount / quantity : 0;
      vatTaxAmount = totalAmount - costAmount;
      
      console.log('💾 Database values preserved:', {
        originalTotalAmount: manualTotalAmount,
        preservedTotalAmount: totalAmount,
        calculatedVatTaxAmount: vatTaxAmount
      });
    } else {
      // PRECISION FIX: Consistent hesaplama yöntemi kullan
      // Her ikisini de aynı mantıkla hesapla: costAmount * (1 + vatRate)
      totalAmount = costAmount * (1 + vatRate);
      totalPrice = quantity > 0 ? totalAmount / quantity : 0;
      // VAT amount'u tutarlı şekilde hesapla
      vatTaxAmount = totalAmount - costAmount;
      
      // DEBUG: Precision kontrolü
      console.log('🔢 Forward calculation:', {
        costAmount: costAmount,
        vatRate: vatRate,
        calculation: `${costAmount} * (1 + ${vatRate}) = ${totalAmount}`,
        vatTaxAmount: vatTaxAmount
      });
    }    return {
      // PRECISION FIX: Sadece display için formatla, internal calculations raw tutulsun
      amount: formatPrecision(amount, 4),
      discountAmount: formatPrecision(discountAmount, 4),
      exciseTaxAmount: formatPrecision(exciseTaxAmount, 4),
      customsAmount: formatPrecision(customsAmount, 4),
      costPrice: formatPrecision(costPrice, 4),
      costAmount: formatPrecision(costAmount, 4),
      vatTaxAmount: formatPrecision(vatTaxAmount, 4),
      totalPrice: formatPrecision(totalPrice, 2),
      // ULTIMATE PROTECTION: Manuel input ve database değerleri ASLA formatlanmaz
      totalAmount: (skipField === "totalAmount" && manualTotalAmount !== null) || isDatabaseLoad
        ? parseFloat(manualTotalAmount)  // User input ve database değerlerini exact koru
        : formatPrecision(totalAmount, 2),
      
      // DEBUG: Raw değerleri de döndür (debugging için)
      _raw: {
        amount: amount,
        costAmount: costAmount,
        vatTaxAmount: vatTaxAmount,
        totalAmount: totalAmount
      }
    };
  };  /**
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
    const lineData = currentValues.purchaseInvoiceLines?.[index];
    if (!lineData) return;

    // Exchange rate'i al
    const exchangeRate = currentValues.purchaseInvoices?.[0]?.exchangeRate || 1;

    const calculated = calculateLineValues(
      lineData,
      changedField,
      exchangeRate
    );    

    // PRECISION FIX: totalAmount koruması sadece manuel değişiklikler için
    const shouldPreserveTotalAmount = changedField === "totalAmount";
      // DEBUG: updateCalculations kontrolü
    console.log('🔄 updateCalculations called:', {
      changedField: changedField,
      preserveTotalAmount: shouldPreserveTotalAmount,
      currentTotalAmount: lineData.totalAmount,
      calculatedTotalAmount: calculated.totalAmount,
      rawCalculated: calculated._raw
    });

    // Sadece değişmeyen alanları güncelle (kullanıcı input'u korunur)
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
    }

    // CRITICAL FIX: totalAmount koruması sadece user input durumunda
    if (shouldPreserveTotalAmount) {
      delete fieldsToUpdate.totalAmount;
      console.log('🛡️ totalAmount protected from update');
    }

    // Tüm hesaplanmış değerleri güncelle
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
   * Exchange Rate değiştiğinde tüm satırları yeniden hesapla
   * @param {Function} setFieldValue - VeeValidate setFieldValue fonksiyonu
   * @param {Function} getFormValues - Form values getter fonksiyonu
   * @param {number} newExchangeRate - Yeni döviz kuru
   */ const onExchangeRateChange = (
    setFieldValue,
    getFormValues,
    newExchangeRate
  ) => {
    // Exchange rate'i güncelle
    setFieldValue("purchaseInvoices[0].exchangeRate", newExchangeRate);

    // Tüm satırları yeniden hesapla
    setTimeout(() => {
      // Fresh form values al (getFormValues() her zaman güncel değerleri döner)
      const freshValues = getFormValues();

      if (freshValues.purchaseInvoiceLines?.length > 0) {
        freshValues.purchaseInvoiceLines.forEach((line, index) => {
          updateCalculations(setFieldValue, index, freshValues, null);
        });
      }
    }, 100);
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
    const newUnitPrice = quantity > 0 ? newAmount / quantity : 0; // Unit Price'ı güncelle
    setFieldValue(
      `purchaseInvoiceLines[${index}].unitPrice`,
      formatPrecision(newUnitPrice, 4)
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
      vatRate > 0 ? newTotalPrice / (1 + vatRate) : newTotalPrice; // Cost Price'ı güncelle (bu Unit Price olacak çünkü discount rate = 0)
    setFieldValue(
      `purchaseInvoiceLines[${index}].unitPrice`,
      formatPrecision(newCostPrice, 4)
    );

    // Diğer hesaplamaları güncelle (totalPrice değişikliğini hariç tut)
    setTimeout(() => {
      updateCalculations(setFieldValue, index, currentValues, "totalPrice");
    }, 50);
  };  /**
   * Total Amount değiştiğinde kapsamlı tersine hesaplama 
   * calculateLineValues'ın tam tersi mantığını uygular
   * PRECISION FIX: Forward calculation ile tutarlı formül kullanır
   */  const onTotalAmountChange = (
    setFieldValue,
    index,
    currentValues,
    newTotalAmount
  ) => {
    // ÖNCE: Kullanıcının exact değerini kaydet (hiç formatlamadan)
    const exactUserValue = parseFloat(newTotalAmount);
    
    // DEBUG: User input kontrolü
    console.log('🎯 User totalAmount input:', {
      input: newTotalAmount,
      parsed: exactUserValue,
      type: typeof newTotalAmount
    });

    // Discount rate'i sıfırla (total amount manuel değiştirildiğinde)
    setFieldValue(`purchaseInvoiceLines[${index}].discountRate`, 0);

    const lineData = currentValues.purchaseInvoiceLines?.[index];
    const vatRate = getVatRate(lineData?.vatId);
    const quantity = lineData?.quantity || 1;
    const exchangeRate = currentValues.purchaseInvoices?.[0]?.exchangeRate || 1;
    const expenseAmount = lineData?.expenseAmount || 0;
    const exciseTaxRate = lineData?.exciseTaxRate || 0;
    const customsRate = lineData?.customsRate || 0;

    // PRECISION FIX: Forward calculation ile aynı formülü kullan
    // Forward: totalAmount = costAmount * (1 + vatRate)
    // Reverse: costAmount = totalAmount / (1 + vatRate)
    const newCostAmount = vatRate > 0 ? exactUserValue / (1 + vatRate) : exactUserValue;

    // STEP 2: Cost Amount'tan Cost Price'ı hesapla
    // costAmount = costPrice × quantity
    const newCostPrice = quantity > 0 ? newCostAmount / quantity : 0;

    // STEP 3: Cost Price'tan base unit price'ı tersine hesapla
    // Forward hesaplama: costPrice = baseUnitPrice × (1 - discountRate/100) + expensePerUnit + taxes
    // Tersine hesaplama: baseUnitPrice = (costPrice - expensePerUnit - taxes) / (1 - discountRate/100)
    
    const expensePerUnit = quantity > 0 ? expenseAmount / quantity : 0;
    
    // Vergilerin ve masrafların olmadığı temiz cost price'ı bul
    // Bu iteratif bir süreç çünkü vergiler cost price üzerinden hesaplanıyor
    let baseCostPriceForTax = newCostPrice - expensePerUnit;
    
    // Vergi oranlarını tersine hesapla (yaklaşık iterasyon)
    for (let i = 0; i < 3; i++) {
      const exciseTaxAmount = baseCostPriceForTax * (exciseTaxRate / 100);
      const customsAmount = baseCostPriceForTax * (customsRate / 100);
      const totalTaxes = exciseTaxAmount + customsAmount;
      baseCostPriceForTax = newCostPrice - expensePerUnit - totalTaxes;
    }

    // Discount olmadığı için (sıfırlandığı için) baseCostPrice = unitPrice × exchangeRate
    const newUnitPrice = baseCostPriceForTax / exchangeRate;
    
    // STEP 4: Unit Price'tan Amount'ı hesapla (discount = 0 olduğu için)
    const newAmount = newUnitPrice * quantity;    // SMART FIX: UI validation için minimal formatting, calculation precision için maksimum hassasiyet
    // Amount: Raw precision (calculation için)
    // UnitPrice: Light formatting (UI validation için)  
    // CostPrice: Light formatting
    // CostAmount: Raw precision (calculation için)
    setFieldValue(`purchaseInvoiceLines[${index}].amount`, newAmount);
    setFieldValue(`purchaseInvoiceLines[${index}].unitPrice`, formatPrecision(newUnitPrice, 6));
    setFieldValue(`purchaseInvoiceLines[${index}].costPrice`, formatPrecision(newCostPrice, 6));
    setFieldValue(`purchaseInvoiceLines[${index}].costAmount`, newCostAmount);

    // SON: Total Amount'ı kullanıcının exact değeriyle set et
    setFieldValue(`purchaseInvoiceLines[${index}].totalAmount`, exactUserValue);    // DEBUG: Final değerleri kontrol et - RAW VALUES
    console.log('🏁 Final reverse calculation result (RAW):', {
      userInput: exactUserValue,
      newAmount: newAmount,
      newCostAmount: newCostAmount,
      newUnitPrice: newUnitPrice,
      formattedUnitPrice: formatPrecision(newUnitPrice, 6),
      calculatedVAT: exactUserValue - newCostAmount,
      verification: `${newCostAmount} * (1 + ${vatRate}) = ${newCostAmount * (1 + vatRate)}`,
      expectedFromCalculator: {
        amount: 423.728813559322,
        vatAmount: 76.27118644067797,
        total: 500.00
      }
    });

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
    setFieldValue(`purchaseInvoiceLines[${index}].discountRate`, 0); // Cost Price = Unit Price - Discount olduğu için ve discount = 0 olduğunda
    // Cost Price = Unit Price olur
    setFieldValue(
      `purchaseInvoiceLines[${index}].unitPrice`,
      formatPrecision(newCostPrice, 4)
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
    const quantity = lineData?.quantity || 1; // Cost Amount = Amount - Discount Amount olduğu için ve discount = 0 olduğunda
    // Cost Amount = Amount olur
    const newAmount = newCostAmount;
    // Amount'tan Unit Price'ı hesapla: unitPrice = amount / quantity
    const newUnitPrice = quantity > 0 ? newAmount / quantity : 0; // Değerleri güncelle
    setFieldValue(
      `purchaseInvoiceLines[${index}].amount`,
      formatPrecision(newAmount, 4)
    );
    setFieldValue(
      `purchaseInvoiceLines[${index}].unitPrice`,
      formatPrecision(newUnitPrice, 4)
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
      totals[key] = formatPrecision(totals[key], 4);
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
        expenseAmount: formatPrecision(equalExpensePerLine, 4),
        costAmount: formatPrecision(
          (line.amount || 0) + equalExpensePerLine,
          4
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
      } // Precision kontrolü
      expenseAmount = formatPrecision(expenseAmount, 4);
      const costAmount = formatPrecision((line.amount || 0) + expenseAmount, 4);

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
          formValues, // tam formValues kullan (exchange rate dahil)
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
    updateExpenseDistribution, // Olay işleyicileri
    onQuantityChange,
    onUnitPriceChange,
    onDiscountRateChange,
    onVatChange,
    onExchangeRateChange,
    onAmountChange,
    onTotalPriceChange,
    onTotalAmountChange,
    onCostPriceChange,
    onCostAmountChange,

    // Utility fonksiyonları
    getVatRate,
    formatVatRate,
    convertCurrency,
  };
}
