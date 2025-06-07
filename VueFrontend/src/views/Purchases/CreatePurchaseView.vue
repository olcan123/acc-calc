<template>
  <!-- Page Header -->
  <div
    class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4"
  >
    <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
      {{
        isImportPurchase
          ? "İthalat Satın Alma Faturası Oluştur"
          : "Lokal Satın Alma Faturası Oluştur"
      }}
    </h2>

    <button
      type="submit"
      form="purchaseForm"
      :disabled="loading || (submitCount > 0 && !meta.valid)"
      class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed"
    >
      <span v-if="loading">⏳</span>
      <span v-else-if="submitCount > 0 && !meta.valid">🚫</span>
      <span v-else>💾</span>
      {{ loading ? "Kaydediliyor..." : "Kaydet" }}
    </button>
  </div>

  <!-- Main Form -->
  <form @submit="onSubmit" id="purchaseForm" class="space-y-6">
    <!-- Purchase Invoice Info Button -->
    <PurchaseInvoiceInfoCard />
    <!-- Purchase Invoice Lines Section -->
    <div class="bg-white dark:bg-gray-800 rounded-lg shadow">
      <div class="p-6 border-b border-gray-200 dark:border-gray-700">
        <div class="flex justify-between items-center">
          <h3 class="text-lg font-medium text-gray-800 dark:text-white">
            Fatura Satırları
          </h3>
          <div class="flex gap-2">
            <button
              type="button"
              @click="recalculateAllLines"
              class="flex items-center gap-2 text-white bg-blue-600 hover:bg-blue-700 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2 dark:bg-blue-500 dark:hover:bg-blue-600 dark:focus:ring-blue-800 transition-colors"
            >
              <span>🔄</span>
              Tekrar Hesapla
            </button>
            <button
              type="button"
              @click="addNewLine"
              class="flex items-center gap-2 text-white bg-green-600 hover:bg-green-700 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-4 py-2 dark:bg-green-500 dark:hover:bg-green-600 dark:focus:ring-green-800"
            >
              <span>➕</span>
              Yeni Satır
            </button>
          </div>
        </div>
      </div>
      <!-- Invoice Lines Table -->
      <div class="overflow-x-auto max-w-full">
        <table
          class="min-w-max text-sm text-left text-gray-500 dark:text-gray-400"
        >
          <PurchaseTableHeader />
          <tbody>
            <PurchaseTableRow />
          </tbody>
        </table>
      </div>
      <!-- Totals Summary -->
      <PurchaseTotalSummary />
    </div>
  </form>
  <!-- Purchase Invoice Modal - Always rendered to preserve form state -->
  <PurchaseInvoiceModal />
</template>

<script setup>
import { computed, watch, provide } from "vue";
import { useRoute } from "vue-router";
import { useForm, useFieldArray } from "vee-validate";
import { storeToRefs } from "pinia";
import { usePurchaseStore } from "@/stores/purchase.store";
import { usePartnerStore } from "@/stores/partner.store";
import { useProductStore } from "@/stores/product.store";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { useUnitOfMeasureStore } from "@/stores/unit-of-measure.store";
import { useVatStore } from "@/stores/vat.store";
import { useAccountStore } from "@/stores/account.store";
import { useModalStore } from "@/stores/modal.store";
import { useCurrencyStore } from "@/stores/currency.store";
import { usePurchaseCalculations } from "@/composables/usePurchaseCalculations";

// Components
import {
  PurchaseInvoiceModal,
  PurchaseInvoiceInfoCard,
  PurchaseTableHeader,
  PurchaseTableRow,
  PurchaseTotalSummary,
} from "@/components/Views/Purchase";

// Stores
const purchaseStore = usePurchaseStore();
const partnerStore = usePartnerStore();
const productStore = useProductStore();
const warehouseStore = useWarehouseStore();
const unitOfMeasureStore = useUnitOfMeasureStore();
const vatStore = useVatStore();
const accountStore = useAccountStore();
const modalStore = useModalStore();
const currencyStore = useCurrencyStore();

// Route to determine invoice type
const route = useRoute();

// Check if this is an import purchase
const isImportPurchase = computed(() => {
  return route.path.includes("/import");
});

// Store reactive data
const { loading } = storeToRefs(purchaseStore);

// Auto-open modal on page load
modalStore.openInvoiceModal();

// Computed property for default warehouse ID
const defaultWarehouseId = computed(() => {
  return formValues.purchaseInvoices?.[0]?.warehouseId || null;
});

// Create initial values based on route type
const createInitialValues = () => {
  const baseValues = {
    ledger: {
      documentType: 1, // Purchase Invoice
      documentDate: new Date().toISOString().split("T")[0],
      referenceNo: "",
      description: isImportPurchase.value
        ? "İthalat Satın Alma Faturası"
        : "Lokal Satın Alma Faturası",
      status: 1,
    },
    purchaseInvoices: [
      {
        invoiceType: isImportPurchase.value ? 2 : 1, // ImportInvoice: 2, LocalInvoice: 1
        partnerId: null,
        vendorAccountId: null,
        invoiceNo: "",
        invoiceDate: new Date().toISOString().split("T")[0],
        status: 1,
        note: "",
        isPaid: false,
        cashPaymentAmount: 0,
        currencyId: 1, // Default to first currency
        exchangeRate: 1,
      },
    ],
    purchaseInvoiceExpenses: [],
    purchaseInvoiceLines: [
      {
        productId: null,
        warehouseId: null,
        unitOfMeasureId: null,
        vatId: null,
        purchaseAccountId: null,
        quantity: 1,
        unitPrice: 0,
        amount: 0,
        expenseAmount: 0,
        discountRate: 0,
        discountAmount: 0,
        exciseTaxRate: 0,
        exciseTaxAmount: 0,
        customsRate: 0,
        customsAmount: 0,
        revaluationAmount: 0,
        vatTaxAmount: 0,
        costPrice: 0,
        costAmount: 0,
        totalPrice: 0,
        totalAmount: 0,
      },
    ],
  };

  // Add currency fields for import purchases
  if (isImportPurchase.value) {
    baseValues.purchaseInvoices[0].importPartnerDocNo = "";
    baseValues.purchaseInvoices[0].importPartnerDocDate = new Date()
      .toISOString()
      .split("T")[0];
  }

  return baseValues;
};

// Form setup
const {
  handleSubmit,
  meta,
  submitCount,
  values: formValues,
  setFieldValue,
  resetForm,
} = useForm({
  initialValues: createInitialValues(),
});

// Field array for invoice lines
const { push } = useFieldArray("purchaseInvoiceLines");

// Initialize purchase calculations composable
const { 
  updateExpenseDistribution, 
  calculateLineValues, 
  updateCalculations,
  onExchangeRateChange
} = usePurchaseCalculations(vatStore);

// Provide exchange rate change handler for child components
provide('onExchangeRateChange', (newExchangeRate) => {
  onExchangeRateChange(setFieldValue, () => formValues, newExchangeRate);
});

// Fetch data on mount
async function fetchData() {
  try {
    // First fetch currencies to set default currency ID
    await currencyStore.fetchCurrencies();

    // Then fetch other data
    await Promise.all([
      partnerStore.fetchPartners(),
      productStore.fetchProducts(),
      warehouseStore.fetchIncludeWarehouses(),
      unitOfMeasureStore.fetchUnitOfMeasures(),
      vatStore.fetchVats(),
      accountStore.fetchAccounts(),
    ]);
    // Tüm veriler başarıyla çekildikten sonra yapılacak işlemler
  } catch (error) {
    // Herhangi bir fetch işlemi başarısız olursa burası çalışır
    console.error("Veri çekme sırasında bir hata oluştu:", error);
  }
}

// Fonksiyonu çağırmak için:
fetchData();

// Line operations
const addNewLine = () => {
  push({
    productId: null,
    warehouseId: defaultWarehouseId.value,
    unitOfMeasureId: null,
    vatId: null,
    purchaseAccountId: null,
    quantity: 1,
    unitPrice: 0,
    amount: 0,
    expenseAmount: 0,
    discountRate: 0,
    discountAmount: 0,
    exciseTaxRate: 0, //isImport
    exciseTaxAmount: 0, //isImport
    customsRate: 0, //isImport
    customsAmount: 0, //isImport
    revaluationAmount: 0, //isImport
    vatTaxRate: 0,
    vatTaxAmount: 0,
    costPrice: 0,
    costAmount: 0,
    totalPrice: 0,
    totalAmount: 0,
  });
};

// Form submission
const onSubmit = handleSubmit(async (values) => {
  try {
    await purchaseStore.addPurchaseInvoice(values);
  } catch (error) {
    console.error("Form submission error:", error);
  }
});

// Recalculate all lines - fix any calculation errors
const recalculateAllLines = () => {
  if (!formValues.purchaseInvoiceLines?.length) return;

  // Recalculate each line
  formValues.purchaseInvoiceLines.forEach((line, index) => {
    // Get current line data
    const lineData = {
      quantity: line.quantity || 0,
      unitPrice: line.unitPrice || 0,
      discountRate: line.discountRate || 0,
      exciseTaxRate: line.exciseTaxRate || 0,
      customsRate: line.customsRate || 0,
      vatId: line.vatId,
      amount: line.amount || 0,
      expenseAmount: line.expenseAmount || 0,
      totalPrice: line.totalPrice || 0,
      totalAmount: line.totalAmount || 0,
      costPrice: line.costPrice || 0,
      costAmount: line.costAmount || 0,
    };

    // Calculate new values
    const calculatedValues = calculateLineValues(lineData);

    // Update form with calculated values
    Object.entries(calculatedValues).forEach(([key, value]) => {
      setFieldValue(`purchaseInvoiceLines[${index}].${key}`, value);
    });
  });
  // Recalculate expense distribution
  if (formValues.purchaseInvoiceExpenses?.length > 0) {
    setTimeout(() => {
      updateExpenseDistribution(formValues, setFieldValue);
    }, 100); // Small delay to ensure line calculations are applied first
  }

  console.log("✅ Tüm satırlar yeniden hesaplandı");
};

// Watch for changes to recalculate totals
watch(
  () => formValues.purchaseInvoiceLines,
  () => {
    // Trigger reactivity for totals
  },
  { deep: true }
);

// Watch for warehouse changes in the modal and update existing lines that don't have a warehouse set
watch(
  () => formValues.purchaseInvoices?.[0]?.warehouseId,
  (newWarehouseId) => {
    if (newWarehouseId && formValues.purchaseInvoiceLines) {
      // Update lines that don't have a warehouse set
      formValues.purchaseInvoiceLines.forEach((line, index) => {
        if (!line.warehouseId) {
          setFieldValue(
            `purchaseInvoiceLines[${index}].warehouseId`,
            newWarehouseId
          );
        }
      });
    }
  }
);

// Watch for expense changes and redistribute expense amounts proportionally
watch(
  () => formValues.purchaseInvoiceExpenses,
  () => {
    // Only redistribute if we have both lines and expenses
    if (formValues.purchaseInvoiceLines?.length > 0) {
      updateExpenseDistribution(formValues, setFieldValue);
    }
  },
  { deep: true }
);

// Watch for invoice line amount changes and redistribute expenses
watch(
  () => formValues.purchaseInvoiceLines?.map((line) => line.amount),
  () => {
    // Only redistribute if we have expenses
    if (
      formValues.purchaseInvoiceExpenses?.length > 0 &&
      formValues.purchaseInvoiceLines?.length > 0
    ) {
      updateExpenseDistribution(formValues, setFieldValue);
    }
  },
  { deep: true }
);
</script>

<style scoped>
/* Custom styles for table horizontal scroll */
.overflow-x-auto {
  scrollbar-width: thin;
  scrollbar-color: #6b7280 #f3f4f6;
}

.overflow-x-auto::-webkit-scrollbar {
  height: 8px;
}

.overflow-x-auto::-webkit-scrollbar-track {
  background: #f3f4f6;
  border-radius: 4px;
}

.overflow-x-auto::-webkit-scrollbar-thumb {
  background: #6b7280;
  border-radius: 4px;
}

.overflow-x-auto::-webkit-scrollbar-thumb:hover {
  background: #4b5563;
}

/* Table width to enable horizontal scroll */
.min-w-max {
  min-width: max-content;
}

/* Ensure table cells don't wrap */
.overflow-x-auto table {
  white-space: nowrap;
}

/* Responsive table container */
@media (max-width: 768px) {
  .overflow-x-auto {
    max-width: calc(100vw - 2rem);
  }
}
</style>
