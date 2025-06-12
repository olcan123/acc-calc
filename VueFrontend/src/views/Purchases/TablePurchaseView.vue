<template>
  <div class="flex justify-between items-center mb-4">
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Satın Alma Faturaları
    </h2>
    <div class="flex gap-2">
      <button
        @click="router.push({ name: 'create-local-purchase' })"
        type="button"
        class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
      >
        Yeni Lokal Fatura
      </button>
      <button
        @click="router.push({ name: 'create-import-purchase' })"
        type="button"
        class="flex items-center gap-2 text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800"
      >
        Yeni İthalat Faturası
      </button>
    </div>
  </div>

  <ConfirmDialog />
  <DataTable
    :value="purchases"
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
          <h3 class="mt-2 text-sm font-medium text-gray-900 dark:text-gray-100">Satın alma faturası bulunamadı</h3>
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
          <p class="mt-2 text-sm">Satın alma faturaları yükleniyor. Lütfen bekleyin...</p>
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
    </Column>

    <!-- Fatura Tarihi -->
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

    <!-- Partner -->
    <Column field="partner.name" header="Tedarikçi" style="min-width: 150px">
      <template #body="{ data }">
        {{ partnerNames[data.partnerId] || "-" }}
      </template>
    </Column>

    <!-- Fatura Tipi -->
    <Column field="invoiceType" header="Tip" style="min-width: 120px">
      <template #body="{ data }">
        <span
          class="px-2 py-1 text-xs font-medium rounded-full"
          :class="getInvoiceTypeColor(data.invoiceType)"
        >
          {{ getInvoiceTypeLabel(data.invoiceType) }}
        </span>
      </template>
    </Column>

    <!-- Para Birimi -->
    <Column field="currency.code" header="Para Birimi" style="min-width: 80px">
      <template #body="{ data }">
        {{
          currenciesWithComputed.find((c) => c.id === data.currencyId)
            ?.formattedCode || "-"
        }}
      </template>
    </Column>

    <!-- Kur -->
    <Column field="exchangeRate" header="Kur" style="min-width: 80px">
      <template #body="{ data }">
        {{ formatCurrency(data.exchangeRate, "", 4) }}
      </template>
    </Column>

    <!-- Durum -->
    <Column field="status" header="Durum" style="min-width: 100px">
      <template #body="{ data }">
        <span
          class="px-2 py-1 text-xs font-medium rounded-full"
          :class="getStatusColor(data.status)"
        >
          {{ getStatusLabel(data.status) }}
        </span>
      </template>
    </Column>

    <!-- Ödeme Durumu -->
    <Column field="isPaid" header="Ödeme" style="min-width: 80px">
      <template #body="{ data }">
        <span
          class="px-2 py-1 text-xs font-medium rounded-full"
          :class="
            data.isPaid
              ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300'
              : 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-300'
          "
        >
          {{ data.isPaid ? "Ödendi" : "Ödenmedi" }}
        </span>
      </template>
    </Column>

    <!-- Toplam Tutar -->
    <Column header="Toplam" style="min-width: 120px">
      <template #body="{ data }">
        {{ calculateInvoiceTotal(data) }}
      </template>
    </Column>

    <!-- Not -->
    <Column field="note" header="Not" style="min-width: 150px">
      <template #body="{ data }">
        <span
          class="text-sm text-gray-600 dark:text-gray-400"
          :title="data.note"
        >
          {{
            data.note
              ? data.note.length > 30
                ? data.note.substring(0, 30) + "..."
                : data.note
              : "-"
          }}
        </span>
      </template>
    </Column>    <!-- İşlem Butonları -->
    <Column header="İşlemler" style="min-width: 200px">
      <template #body="{ data }">
        <div class="flex gap-2">
          <!-- Edit Button -->
          <router-link
            :to="{
              name:
                data.invoiceType === 2
                  ? 'update-import-purchase'
                  : 'update-local-purchase',
              params: { id: data.id },
            }"
            class="p-2 text-blue-600 hover:text-blue-800 hover:bg-blue-100 dark:hover:bg-blue-900 rounded-lg transition-colors"
            title="Düzenle"
          >
            <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
              <path
                d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z"
              />
            </svg>
          </router-link>

          <!-- Delete Button -->
          <button
            @click="confirmDelete(data)"
            class="p-2 text-red-600 hover:text-red-800 hover:bg-red-100 dark:hover:bg-red-900 rounded-lg transition-colors"
            title="Sil"
          >
            <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
              <path
                fill-rule="evenodd"
                d="M9 2a1 1 0 000 2h2a1 1 0 100-2H9zM4 5a2 2 0 012-2h8a2 2 0 012 2v6a2 2 0 01-2 2H6a2 2 0 01-2-2V5zm3 4a1 1 0 102 0v-1a1 1 0 10-2 0v1zm4 0a1 1 0 102 0v-1a1 1 0 10-2 0v1z"
                clip-rule="evenodd"
              />
            </svg>
          </button>

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
              @click.prevent="viewExpenses(data)"
              class="dropdown-item"
            >
              Masraflar
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
              @click.prevent="duplicateInvoice(data)"
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
import ConfirmDialog from "primevue/confirmdialog";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import { usePurchaseStore } from "@/stores/purchase.store";
import { useAccountStore } from "@/stores/account.store";
import { useCurrencyStore } from "@/stores/currency.store";
import { usePartnerStore } from "@/stores/partner.store";
import { storeToRefs } from "pinia";
import TableDropdownButton from "@/components/UI/Buttons/TableDropdownButton.vue";
import LedgerModal from "@/components/UI/Modal/LedgerModal.vue";
import LedgerDetailsModal from "@/components/Views/Purchase/LedgerDetailsModal.vue";

// Import enum helpers
import {
  getInvoiceTypeLabel,
  getInvoiceTypeColor,
  getStatusLabel,
  getStatusColor,
} from "@/services/constants/purchase-enum";

const router = useRouter();
const confirm = useConfirm();

// Modal states
const showLedgerModal = ref(false);
const selectedLedgerId = ref(null);

const purchaseStore = usePurchaseStore();
const currencyStore = useCurrencyStore();
const partnerStore = usePartnerStore();
const accountStore = useAccountStore();

// Fetch initial data
await purchaseStore.fetchPurchases();
await accountStore.fetchAccounts();
await currencyStore.fetchCurrencies();
await partnerStore.fetchPartners();
// Reactive references
const { purchases, loading } = storeToRefs(purchaseStore);
const { accountTypeOptions } = storeToRefs(accountStore);
const { currencies } = storeToRefs(currencyStore);
const { partners } = storeToRefs(partnerStore);

// Filter state initialization
const initFilters = () => {
  filters.value = {
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    invoiceNo: {
      operator: FilterOperator.AND,
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }],
    },
    invoiceDate: {
      operator: FilterOperator.AND,
      constraints: [{ value: null, matchMode: FilterMatchMode.DATE_IS }],
    },
    "partner.name": {
      operator: FilterOperator.AND,
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }],
    },
    purchaseInvoiceType: {
      operator: FilterOperator.AND,
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }],
    },
    isPaid: {
      operator: FilterOperator.AND,
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }],
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

// Purchase invoice deletion
const confirmDelete = (purchase) => {
  confirm.require({
    message: `"${purchase.invoiceNo}" numaralı faturayı silmek istediğinize emin misiniz?`,
    header: "Fatura Silme Onayı",
    icon: "pi pi-exclamation-triangle",
    acceptClass: "p-button-danger",
    accept: async () => {
      await purchaseStore.deletePurchase(purchase.id);
    },
  });
};

// Helper functions
const formatDate = (date) => {
  if (!date) return "-";
  return new Date(date).toLocaleDateString("tr-TR");
};

const formatCurrency = (amount, currency = "€", decimals = 2) => {
  if (amount === null || amount === undefined) return "-";
  return (
    new Intl.NumberFormat("de-DE", {
      minimumFractionDigits: decimals,
      maximumFractionDigits: decimals,
    }).format(amount) + (currency ? ` ${currency}` : "")
  );
};

const calculateInvoiceTotal = (invoice) => {
  if (
    !invoice.purchaseInvoiceLines ||
    invoice.purchaseInvoiceLines.length === 0
  ) {
    return formatCurrency(0);
  }

  const total = invoice.purchaseInvoiceLines.reduce((sum, line) => {
    return sum + (line.totalAmount || 0);
  }, 0);

  return formatCurrency(total);
};

// Detail view functions
const viewDetails = async (invoice) => {
  try {
    await purchaseStore.fetchPurchase(invoice.id);
    // TODO: Show details in a modal or navigate to details page
    console.log("Invoice details:", purchaseStore.purchase);
  } catch (error) {
    console.error("Error fetching invoice details:", error);
  }
};

const viewLines = async (invoice) => {
  try {
    const lines = await purchaseStore.fetchPurchaseLines(invoice.id);
    console.log("Invoice lines:", lines);
    // TODO: Show lines in a modal
  } catch (error) {
    console.error("Error fetching invoice lines:", error);
  }
};

const viewExpenses = async (invoice) => {
  try {
    const expenses = await purchaseStore.fetchPurchaseExpenses(invoice.id);
    console.log("Invoice expenses:", expenses);
    // TODO: Show expenses in a modal
  } catch (error) {
    console.error("Error fetching invoice expenses:", error);
  }
};

// Show ledger details modal
const viewLedgerDetails = (invoice) => {
  if (invoice.ledger?.id) {
    selectedLedgerId.value = invoice.ledger.id;
    showLedgerModal.value = true;
  } else {
    console.warn("Ledger information not found for invoice:", invoice.id);
  }
};

// Currencies with computed properties
const currenciesWithComputed = computed(() => {
  return currencies.value.map((currency) => ({
    ...currency,
    formattedCode: currency.code.toUpperCase(),
  }));
});

// Partner names with computed properties
const partnerNames = computed(() => {
  return partners.value.reduce((acc, partner) => {
    acc[partner.id] = partner.name;
    return acc;
  }, {});
});

const duplicateInvoice = (invoice) => {
  // TODO: Implement invoice duplication
  console.log("Duplicating invoice:", invoice.id);
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
