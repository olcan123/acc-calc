<template>
  <!-- Page Header -->
  <div
    class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4"
  >
    <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
      {{
        isExportSale
          ? "İhracat Satış Faturası Düzenle"
          : "Lokal Satış Faturası Düzenle"
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
        {{ loading ? "Güncelleniyor..." : "Güncelle" }}
      </button>
    </div>
  </div>

  <!-- Loading State -->
  <div v-if="loadingData" class="flex justify-center items-center py-8">
    <div
      class="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"
    ></div>
  </div>
  <!-- Content -->
  <form
    @submit="onSubmit"
    id="saleForm"
    v-if="!loadingData && formValues.saleInvoices?.[0]?.id"
  >
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
  <!-- Error State -->
  <div v-else-if="!loadingData" class="flex justify-center items-center py-8">
    <div class="text-center">
      <div class="text-red-600 text-lg mb-2">⚠️</div>
      <div class="text-gray-600 dark:text-gray-400">
        Satış faturası bulunamadı
      </div>
    </div>
  </div>

  <!-- Sale Invoice Modal -->
  <SaleInvoiceModal />
</template>

<script setup>
import { ref, computed, onMounted, watch } from "vue";
import { useRoute, useRouter } from "vue-router";
import { storeToRefs } from "pinia";
import { useToast } from "vue-toastification";
import { useForm, useFieldArray } from "vee-validate";

// Stores
import { useSaleStore } from "@/stores/sale.store";
import { usePartnerStore } from "@/stores/partner.store";
import { useProductStore } from "@/stores/product.store";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { useUnitOfMeasureStore } from "@/stores/unit-of-measure.store";
import { useVatStore } from "@/stores/vat.store";
import { useAccountStore } from "@/stores/account.store";
import { useBarcodeStore } from "@/stores/barcode.store";
import { useModalStore } from "@/stores/modal.store";
import { useCurrencyStore } from "@/stores/currency.store";

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
const currencyStore = useCurrencyStore();
const barcodeStore = useBarcodeStore();

// Reactive data from stores
const { loading, sale } = storeToRefs(saleStore);

// Local state
const loadingData = ref(true);

// Create initial values based on existing data structure
const createInitialValues = () => {
  return {
    ledger: {
      id: null,
      documentType: 1, // Sale Invoice
      documentDate: new Date().toISOString().split("T")[0],
      referenceNo: "",
      description: "Satış Faturası",
      status: 1,
    },
    saleInvoices: [
      {
        id: null,
        saleInvoiceType: 1,
        partnerId: null,
        customerAccountId: null,
        invoiceNo: "",
        invoiceDate: new Date().toISOString().split("T")[0],
        warehouseId: null,
        currencyId: 1,
        isPaid: false,
        isWholeSale: false,
        cashPaymentAmount: 0,
        note: "",
        status: 1,
      },
    ],
    saleInvoiceLines: [],
  };
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

// Computed properties
const isExportSale = computed(() => {
  return formValues.saleInvoices?.[0]?.saleInvoiceType === 2;
});

const defaultWarehouseId = computed(() => {
  return formValues.saleInvoices?.[0]?.warehouseId || null;
});

// Methods
const loadSaleData = async () => {
  try {
    loadingData.value = true;
    const saleId = route.params.id;

    if (!saleId) {
      throw new Error("Sale ID not provided");
    } // Load sale data
    await saleStore.fetchSale(saleId);

    if (!sale.value) {
      toast.error("Satış faturası bulunamadı");
      return;
    }

    // The sale data structure is different from purchase - the sale object itself contains the invoice data
    if (sale.value && sale.value.id) {
      // The sale object itself is the invoice, not nested in saleInvoices array
      const invoice = sale.value;
      const ledger = sale.value.ledger;

      // Reset form with existing data
      resetForm({
        values: {
          ledger: {
            id: ledger?.id || null,
            documentType: ledger?.documentType || 1,
            documentDate: ledger?.documentDate || invoice.invoiceDate,
            referenceNo: ledger?.referenceNo || invoice.invoiceNo,
            description: ledger?.description || "Satış Faturası",
            status: ledger?.status || 1,
          },
          saleInvoices: [
            {
              id: invoice.id,
              ledgerId: invoice.ledgerId || ledger?.id,
              saleInvoiceType: invoice.saleInvoiceType || 1,
              partnerId: invoice.partnerId,
              customerAccountId: invoice.customerAccountId,
              invoiceNo: invoice.invoiceNo,
              invoiceDate: invoice.invoiceDate,
              warehouseId: invoice.warehouseId,
              currencyId: invoice.currencyId || 1,
              isPaid: invoice.isPaid || false,
              isWholeSale: invoice.isWholeSale || false,
              cashPaymentAmount: invoice.cashPaymentAmount || 0,
              note: invoice.note || "",
              status: invoice.status || 1,
              warehouseId:
                sale.value.saleInvoiceLines?.[0]?.warehouseId || null,
            },
          ],
          saleInvoiceLines: sale.value.saleInvoiceLines || [],
        },
      });
    } else {
      toast.error("Satış faturası verileri eksik veya hatalı");
    }
  } catch (error) {
    toast.error("Satış faturası yüklenirken bir hata oluştu");
  } finally {
    loadingData.value = false;
  }
};

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

const removeLine = (index) => {
  remove(index);
  recalculateAllLines();
};

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
};

// Form submission
const onSubmit = handleSubmit(async (values) => {
  try {
    const saleId = route.params.id;
    if (!saleId) {
      throw new Error("Sale ID not provided");
    }

    await saleStore.updateSale(values);
  } catch (error) {
    toast.error("Satış faturası güncellenirken bir hata oluştu");
  }
});

// Lifecycle
onMounted(async () => {
  // Load initial data
  try {
    await Promise.all([
      partnerStore.fetchPartners(),
      productStore.fetchProducts(),
      warehouseStore.fetchIncludeWarehouses(),
      unitOfMeasureStore.fetchUnitOfMeasures(),
      vatStore.fetchVats(),
      accountStore.fetchAccounts(),
      currencyStore.fetchCurrencies(),
      barcodeStore.fetchBarcodes(),
    ]);

    // Load sale data
    await loadSaleData();

    // Open the invoice modal after data is loaded
    modalStore.openInvoiceModal();
  } catch (error) {
    toast.error("Veri yüklenirken bir hata oluştu");
  }
});

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
