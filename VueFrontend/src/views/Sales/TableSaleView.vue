<template>
  <div class="flex justify-between items-center mb-4">
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Satış Faturaları
    </h2>
    <div class="flex gap-2">
      <button
        @click="router.push({ name: 'create-local-sale' })"
        type="button"
        class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
      >
        Yeni Lokal Fatura
      </button>
      <button
        @click="router.push({ name: 'create-export-sale' })"
        type="button"
        class="flex items-center gap-2 text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800"
      >
        Yeni İhracat Faturası
      </button>
    </div>
  </div>

  <ConfirmDialog />
  <DataTable
    :value="sales"
    v-model:filters="filters"
    dataKey="id"
    responsiveLayout="scroll"
    :loading="loading"
    paginator
    :rows="10"
    filterDisplay="menu"
    sortMode="multiple"
    :globalFilterFields="['invoiceNo', 'partner.name', 'partner.tradeName']"
  >
    <template #header>
      <div class="flex flex-wrap justify-between items-center gap-4">
        <!-- Clear Filter Button -->
        <button
          @click="clearFilter"
          type="button"
          class="flex items-center gap-2 text-gray-700 bg-gray-100 hover:bg-gray-200 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-4 py-2 focus:outline-none dark:bg-gray-600 dark:hover:bg-gray-700 dark:text-white dark:focus:ring-gray-800"
        >
          <span>🗑️</span>
          Filtreleri Temizle
        </button>

        <!-- Global Search -->
        <div class="flex items-center gap-2">
          <div class="relative">
            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <svg class="w-4 h-4 text-gray-500 dark:text-gray-400" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd"></path>
              </svg>
            </div>
            <input
              v-model="filters['global'].value"
              type="text"
              placeholder="Anahtar kelime ile ara..."
              class="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-md leading-5 bg-white placeholder-gray-500 focus:outline-none focus:placeholder-gray-400 focus:ring-1 focus:ring-blue-500 focus:border-blue-500 sm:text-sm dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
            />
          </div>
        </div>
      </div>
    </template>

    <template #empty>
      <div class="text-center py-8">
        <div class="text-gray-500 dark:text-gray-400">
          <svg class="mx-auto h-12 w-12 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
          </svg>
          <h3 class="mt-2 text-sm font-medium text-gray-900 dark:text-gray-100">Satış faturası bulunamadı</h3>
          <p class="mt-1 text-sm text-gray-500 dark:text-gray-400">Arama kriterlerinizi değiştirerek tekrar deneyin.</p>
        </div>
      </div>
    </template>

    <template #loading>
      <div class="text-center py-8">
        <div class="text-gray-500 dark:text-gray-400">
          <div class="animate-spin inline-block w-6 h-6 border-[3px] border-current border-t-transparent text-blue-600 rounded-full" role="status" aria-label="loading">
            <span class="sr-only">Yükleniyor...</span>
          </div>
          <p class="mt-2 text-sm">Satış faturaları yükleniyor. Lütfen bekleyin...</p>
        </div>
      </div>
    </template>    <!-- Sıra Numarası -->
    <Column header="#" style="width: 60px">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>

    <!-- Fatura No -->
    <Column field="invoiceNo" header="Fatura No" sortable style="min-width: 120px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Fatura no ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>    <!-- Fatura Tarihi -->
    <Column field="invoiceDate" header="Fatura Tarihi" sortable style="min-width: 120px" dataType="date">
      <template #body="{ data }">
        {{ formatDate(data.invoiceDate) }}
      </template>
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="date"
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
        />
      </template>
    </Column>

    <!-- Partner (Müşteri) -->
    <Column field="partner.name" header="Müşteri" sortable style="min-width: 150px">
      <template #body="{ data }">
        {{ partnerNames[data.partnerId] || "-" }}
      </template>
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Müşteri ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>

    <!-- Fatura Tipi -->
    <Column field="saleInvoiceType" header="Tip" sortable style="min-width: 120px">
      <template #body="{ data }">
        <span
          class="px-2 py-1 text-xs font-medium rounded-full"
          :class="getInvoiceTypeColor(data.saleInvoiceType)"
        >
          {{ getInvoiceTypeLabel(data.saleInvoiceType) }}
        </span>
      </template>
      <template #filter="{ filterModel }">
        <select
          v-model="filterModel.value"
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
        >
          <option value="">Tümü</option>
          <option value="1">Lokal</option>
          <option value="2">İhracat</option>
        </select>
      </template>
    </Column>

    <!-- Ödeme Durumu -->
    <Column field="isPaid" header="Ödeme" sortable style="min-width: 80px">
      <template #body="{ data }">
        <span
          class="px-2 py-1 text-xs font-medium rounded-full"
          :class="data.isPaid ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'"
        >
          {{ data.isPaid ? "Ödendi" : "Ödenmedi" }}
        </span>
      </template>
      <template #filter="{ filterModel }">
        <select
          v-model="filterModel.value"
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
        >
          <option value="">Tümü</option>
          <option value="true">Ödendi</option>
          <option value="false">Ödenmedi</option>
        </select>
      </template>
    </Column>

    <!-- Toptan Satış -->
    <Column field="isWholeSale" header="Toptan" sortable style="min-width: 80px">      <template #body="{ data }">
        <span
          class="px-2 py-1 text-xs font-medium rounded-full"
          :class="data.isWholeSale ? 'bg-blue-100 text-blue-800' : 'bg-gray-100 text-gray-800'"
        >
          {{ data.isWholeSale ? "Evet" : "Hayır" }}
        </span>
      </template>
      <template #filter="{ filterModel }">
        <select
          v-model="filterModel.value"
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
        >
          <option value="">Tümü</option>
          <option value="true">Evet</option>
          <option value="false">Hayır</option>
        </select>
      </template>
    </Column>

    <!-- Nakit Ödeme -->
    <Column field="cashPaymentAmount" header="Nakit Ödeme" sortable style="min-width: 120px" dataType="numeric">
      <template #body="{ data }">
        {{ formatCurrency(data.cashPaymentAmount || 0, "€") }}
      </template>
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="number"
          step="0.01"
          min="0"
          placeholder="Min nakit ödeme..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>

    <!-- Toplam Tutar -->
    <Column header="Toplam Tutar" sortable style="min-width: 120px" dataType="numeric">
      <template #body="{ data }">
        {{ formatCurrency(calculateInvoiceTotal(data), "€") }}
      </template>
    </Column>    <!-- İşlemler -->
    <Column header="İşlemler" style="min-width: 200px">
      <template #body="{ data }">        <div class="flex gap-2">
          <!-- Edit Button -->
          <EditLinkButton 
            :to="{
              name: data.saleInvoiceType === 2 ? 'update-export-sale' : 'update-local-sale',
              params: { id: data.id },
            }"
          />

          <!-- Delete Button -->
          <DeleteButton @click="handleDelete(data)" />

          <!-- More Actions Dropdown -->
          <TableDropdownButton buttonText="İşlemler">
            <a
              href="#"
              @click.prevent="viewDetails(data)"
              class="dropdown-item"
            >
              Detayları Görüntüle
            </a>

            <a href="#" @click.prevent="viewLines(data)" class="dropdown-item">
              Fatura Satırları
            </a>

            <a
              href="#"
              @click.prevent="viewLedgerDetails(data)"
              class="dropdown-item"
            >
              Defter Kayıtları
            </a>

            <hr class="my-2 border-gray-200 dark:border-gray-600" />
            <a
              href="#"
              @click.prevent="duplicateSale(data)"
              class="dropdown-item"
            >
              Faturayı Kopyala
            </a>
          </TableDropdownButton>
        </div>
      </template>
    </Column>
  </DataTable>
  <!-- Ledger Modal -->
  <LedgerModal v-model="showLedgerModal">
    <LedgerDetailsModal v-if="selectedLedgerId" :ledger-id="selectedLedgerId" />
  </LedgerModal>
</template>

<script setup>
import { ref, computed, onMounted } from "vue";
import { FilterMatchMode, FilterOperator } from "@primevue/core";
import { useRouter } from "vue-router";
import { useConfirm } from "primevue/useconfirm";
import { storeToRefs } from "pinia";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import ConfirmDialog from "primevue/confirmdialog";
import { useSaleStore } from "@/stores/sale.store";
import { usePartnerStore } from "@/stores/partner.store";
import { useCurrencyStore } from "@/stores/currency.store";
import TableDropdownButton from "@/components/UI/Buttons/TableDropdownButton.vue";
import EditLinkButton from "@/components/UI/Buttons/EditLinkButton.vue";
import DeleteButton from "@/components/UI/Buttons/DeleteButton.vue";
import LedgerModal from "@/components/UI/Modal/LedgerModal.vue";
import LedgerDetailsModal from "@/components/Views/Sale/LedgerDetailsModal.vue";

const router = useRouter();
const confirm = useConfirm();

// Stores
const saleStore = useSaleStore();
const partnerStore = usePartnerStore();
const currencyStore = useCurrencyStore();

// Reactive state
const { sales, loading } = storeToRefs(saleStore);
const { partners } = storeToRefs(partnerStore);

// Modal state
const showLedgerModal = ref(false);
const selectedLedgerId = ref(null);

// Filter state initialization
const initFilters = () => {
  filters.value = {
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    invoiceNo: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    invoiceDate: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.DATE_IS }] 
    },
    'partner.name': { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    saleInvoiceType: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    isPaid: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    isWholeSale: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    cashPaymentAmount: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
  };
};

// Filter state
const filters = ref();
initFilters();

// Clear filters function
const clearFilter = () => {
  initFilters();
};

// Computed partner names mapping
const partnerNames = computed(() => {
  const mapping = {};
  partners.value.forEach((partner) => {
    mapping[partner.id] = partner.name;
  });
  return mapping;
});

// Lifecycle hooks
onMounted(async () => {
  await Promise.all([
    saleStore.fetchSales(),
    partnerStore.fetchPartners(),
    currencyStore.fetchCurrencies(),
  ]);
});

// Helper functions
const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString("tr-TR");
};

const formatCurrency = (value, currency = "€", precision = 2) => {
  return new Intl.NumberFormat("de-DE", {
    style: "currency",
    currency: "EUR",
    minimumFractionDigits: precision,
    maximumFractionDigits: precision,
  }).format(value || 0);
};

const getInvoiceTypeColor = (type) => {
  switch (type) {
    case 1: // LocalInvoice
      return "bg-blue-100 text-blue-800";
    case 2: // ExportInvoice
      return "bg-green-100 text-green-800";
    case 3: // DebitNote
      return "bg-yellow-100 text-yellow-800";
    case 4: // CreditNote
      return "bg-red-100 text-red-800";
    default:
      return "bg-gray-100 text-gray-800";
  }
};

const getInvoiceTypeLabel = (type) => {
  switch (type) {
    case 1:
      return "Lokal";
    case 2:
      return "İhracat";
    case 3:
      return "Borç Dekontu";
    case 4:
      return "Alacak Dekontu";
    default:
      return "Bilinmeyen";
  }
};

const calculateInvoiceTotal = (sale) => {
  if (!sale.saleInvoiceLines || sale.saleInvoiceLines.length === 0) {
    return 0;
  }
  return sale.saleInvoiceLines.reduce((total, line) => {
    return total + (line.totalAmount || 0);
  }, 0);
};

// Detail view functions
const viewDetails = async (sale) => {
  try {
    await saleStore.fetchSale(sale.id);
    // TODO: Show details in a modal or navigate to details page
    console.log("Sale details:", saleStore.sale);
  } catch (error) {
    console.error("Error fetching sale details:", error);
  }
};

const viewLines = async (sale) => {
  try {
    const lines = await saleStore.fetchSaleLines(sale.id);
    console.log("Sale lines:", lines);
    // TODO: Show lines in a modal
  } catch (error) {
    console.error("Error fetching sale lines:", error);
  }
};

// Show ledger details modal
const viewLedgerDetails = (sale) => {
  if (sale.ledger?.id) {
    selectedLedgerId.value = sale.ledger.id;
    showLedgerModal.value = true;
  } else {
    console.warn("Ledger information not found for sale:", sale.id);
  }
};

const duplicateSale = (sale) => {
  // TODO: Implement sale duplication
  console.log("Duplicating sale:", sale.id);
};

const handleEdit = (sale) => {
  const routeName = sale.saleInvoiceType === 2 ? 'update-export-sale' : 'update-local-sale';
  router.push({
    name: routeName,
    params: { id: sale.id },
  });
};

const handleDelete = (sale) => {
  console.log("Deleting sale:", sale);
  confirm.require({
    message: `"${sale.invoiceNo}" numaralı satış faturasını silmek istediğinizden emin misiniz?`,
    header: "Silme Onayı",
    icon: "pi pi-exclamation-triangle",
    rejectProps: {
      label: "İptal",
      severity: "secondary",
      outlined: true,
    },
    acceptProps: {
      label: "Sil",
      severity: "danger",
    },
    accept: async () => {
      try {
        await saleStore.deleteSale(sale.id);
        console.log("Sale deleted successfully.");
      } catch (error) {
        console.error("Delete error:", error);
      }
    },
  });
};
</script>

<style scoped>
.dropdown-item {
  display: block;
  padding: 0.5rem 1rem;
  font-size: 0.875rem;
  color: #374151;
  transition: background 0.2s;
}

.dropdown-item:hover {
  background-color: #f3f4f6;
}

@media (prefers-color-scheme: dark) {
  .dropdown-item {
    color: #d1d5db;
  }

  .dropdown-item:hover {
    background-color: #374151;
  }
}
</style>
