<template>
  <!-- Page Header -->
  <div
    class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4"
  >
    <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
      {{
        isExportSale
          ? "İhracat Satış Faturası Oluştur"
          : "Lokal Satış Faturası Oluştur"
      }}
    </h2>

    <div class="flex gap-2">
      <button
        type="button"
        @click="router.push({ name: 'table-sale' })"
        class="flex items-center gap-2 text-gray-700 bg-gray-200 hover:bg-gray-300 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-gray-600 dark:text-white dark:hover:bg-gray-700 dark:focus:ring-gray-800"
      >
        ← Geri Dön
      </button>
      <button
        type="submit"
        form="saleForm"
        :disabled="loading"
        class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed"
      >
        <span v-if="loading">⏳</span>
        <span v-else>💾</span>
        {{ loading ? "Kaydediliyor..." : "Kaydet" }}
      </button>
    </div>
  </div>

  <!-- Main Form -->
  <form @submit="onSubmit" id="saleForm" class="space-y-6">
    <!-- Sale Invoice Info Card -->
    <SaleInvoiceInfoCard />

    <!-- Sale Invoice Lines Section -->
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
          <SaleTableHeader />
          <tbody>
            <SaleTableRow />
          </tbody>
        </table>
      </div>

      <!-- Totals Summary -->
      <SaleTotalSummary />
    </div>
  </form>

  <!-- Sale Invoice Modal -->
  <SaleInvoiceModal />
</template>

<script setup>
import { computed, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useForm, useFieldArray } from "vee-validate";
import { storeToRefs } from "pinia";
import { useToast } from "vue-toastification";

// Stores
import { useSaleStore } from "@/stores/sale.store";
import { usePartnerStore } from "@/stores/partner.store";
import { useProductStore } from "@/stores/product.store";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { useUnitOfMeasureStore } from "@/stores/unit-of-measure.store";
import { useVatStore } from "@/stores/vat.store";
import { useAccountStore } from "@/stores/account.store";
import { useModalStore } from "@/stores/modal.store";

// Composables
import { useSaleCalculations } from "@/composables/useSaleCalculations";

// Components
import {
  SaleInvoiceModal,
  SaleInvoiceInfoCard,
  SaleTableHeader,
  SaleTableRow,
  SaleTotalSummary,
} from "@/components/Views/Sale";

// Router and route
const route = useRoute();
const router = useRouter();
const toast = useToast();

// Store instances
const saleStore = useSaleStore();
const partnerStore = usePartnerStore();
const productStore = useProductStore();
const warehouseStore = useWarehouseStore();
const unitOfMeasureStore = useUnitOfMeasureStore();
const vatStore = useVatStore();
const accountStore = useAccountStore();
const modalStore = useModalStore();

// Reactive data from stores
const { loading } = storeToRefs(saleStore);

// Check if this is an export sale
const isExportSale = computed(() => {
  return route.name === "create-export-sale";
});

// Auto-open modal on page load
modalStore.openInvoiceModal();

// Computed property for default warehouse ID
const defaultWarehouseId = computed(() => {
  return formValues.saleInvoices?.[0]?.warehouseId || null;
});

// Create initial values based on route type
const createInitialValues = () => {
  const baseValues = {
    ledger: {
      documentType: 1, // Sale Invoice
      documentDate: new Date().toISOString().split("T")[0],
      referenceNo: "",
      description: isExportSale.value
        ? "İhracat Satış Faturası"
        : "Lokal Satış Faturası",
      status: 1,
    },
    saleInvoices: [
      {
        saleInvoiceType: isExportSale.value ? 2 : 1, // ExportInvoice: 2, LocalInvoice: 1
        partnerId: null,
        customerAccountId: null,
        invoiceNo: "",
        invoiceDate: new Date().toISOString().split("T")[0],
        warehouseId: null,
        currencyId: 1, // Default EUR
        isPaid: false,
        isWholeSale: false,
        cashPaymentAmount: 0,
        note: "",
        status: 1,
      },
    ],    saleInvoiceLines: [
      {
        productId: null,
        warehouseId: null,
        unitOfMeasureId: null,
        vatId: isExportSale.value ? 1 : null, // Set VAT to 0% for export sales
        saleAccountId: null,
        quantity: 1,
        unitPrice: 0,
        amount: 0,
        discountRate: 0,
        discountAmount: 0,
        vatTaxAmount: 0,
        totalPrice: 0,
        totalAmount: 0,
        description: "", // Backend entity'de olan field
      },
    ],
  };

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
const { push, remove } = useFieldArray("saleInvoiceLines");

// Initialize sale calculations composable
const { calculateLineTotal } = useSaleCalculations();

// Fetch data on mount
async function fetchData() {
  try {
    // Then fetch other data
    await Promise.all([
      partnerStore.fetchPartners(),
      productStore.fetchProducts(),
      warehouseStore.fetchIncludeWarehouses(),
      unitOfMeasureStore.fetchUnitOfMeasures(),
      vatStore.fetchVats(),
      accountStore.fetchAccounts(),
    ]);
  } catch (error) {
    console.error("Veri çekme sırasında bir hata oluştu:", error);
    toast.error("Veri yüklenirken bir hata oluştu");
  }
}

// Call fetch function
fetchData();

// Line operations
const addNewLine = () => {
  push({
    productId: null,
    warehouseId: defaultWarehouseId.value,
    unitOfMeasureId: null,
    vatId: isExportSale.value ? 1 : null, // Set VAT to 0% for export sales
    saleAccountId: null,
    quantity: 1,
    unitPrice: 0,
    amount: 0,
    discountRate: 0,
    discountAmount: 0,
    vatTaxAmount: 0,
    totalPrice: 0,
    totalAmount: 0,
    description: "", // Backend entity'de olan field
  });
};

// Form submission
const onSubmit = handleSubmit(async (values) => {
  try {
    await saleStore.addSaleInvoice(values);
  } catch (error) {
    console.error("Form submission error:", error);
    toast.error("Satış faturası oluşturulurken bir hata oluştu");
  }
});

// Recalculate all lines - fix any calculation errors
const recalculateAllLines = () => {
  if (!formValues.saleInvoiceLines?.length) return;

  // Recalculate each line
  formValues.saleInvoiceLines.forEach((line, index) => {
    // Get current line data
    const lineData = {
      quantity: line.quantity || 0,
      unitPrice: line.unitPrice || 0,
      discountRate: line.discountRate || 0,
      vatId: line.vatId,
      amount: line.amount || 0,
      totalAmount: line.totalAmount || 0,
    };

    // Calculate new values
    const calculatedValues = calculateLineTotal(lineData);

    // Update form with calculated values
    Object.entries(calculatedValues).forEach(([key, value]) => {
      setFieldValue(`saleInvoiceLines[${index}].${key}`, value);
    });
  });

  console.log("✅ Tüm satırlar yeniden hesaplandı");
};

// Watch for changes to recalculate totals
watch(
  () => formValues.saleInvoiceLines,
  () => {
    // Trigger reactivity for totals
  },
  { deep: true }
);

// Watch for warehouse changes in the modal and update existing lines that don't have a warehouse set
watch(
  () => formValues.saleInvoices?.[0]?.warehouseId,
  (newWarehouseId) => {
    if (newWarehouseId && formValues.saleInvoiceLines) {
      // Update lines that don't have a warehouse set
      formValues.saleInvoiceLines.forEach((line, index) => {
        if (!line.warehouseId) {
          setFieldValue(
            `saleInvoiceLines[${index}].warehouseId`,
            newWarehouseId
          );
        }
      });
    }
  }
);
</script>
