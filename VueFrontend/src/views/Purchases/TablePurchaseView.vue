<template>
  <div class="flex justify-between items-center mb-4">
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Satın Alma Faturaları
    </h2>

    <div class="flex gap-2">
      <button
        @click="router.push({ name: 'CreateLocalPurchase' })"
        type="button"
        class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
      >
        Yeni Lokal Fatura
      </button>
      <button
        @click="router.push({ name: 'CreateImportPurchase' })"
        type="button"
        class="flex items-center gap-2 text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800"
      >
        Yeni İthalat Faturası
      </button>
    </div>
  </div>

  <ConfirmDialog />

  <DataTable :value="purchases" dataKey="id" responsiveLayout="scroll" :loading="loading">
    <!-- Sıra Numarası -->
    <Column header="#" style="width: 60px">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>

    <!-- Fatura No -->
    <Column field="invoiceNo" header="Fatura No" style="min-width: 120px" />

    <!-- Fatura Tarihi -->
    <Column field="invoiceDate" header="Fatura Tarihi" style="min-width: 120px">
      <template #body="{ data }">
        {{ formatDate(data.invoiceDate) }}
      </template>
    </Column>

    <!-- Partner -->
    <Column field="partner.name" header="Tedarikçi" style="min-width: 150px">
      <template #body="{ data }">
        {{ data.partner?.name || data.partner?.tradeName || "-" }}
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
        {{ data.currency?.code || "-" }}
      </template>
    </Column>

    <!-- Kur -->
    <Column field="exchangeRate" header="Kur" style="min-width: 80px">
      <template #body="{ data }">
        {{ formatCurrency(data.exchangeRate, '', 4) }}
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
          :class="data.isPaid ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300' : 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-300'"
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
        <span class="text-sm text-gray-600 dark:text-gray-400" :title="data.note">
          {{ data.note ? (data.note.length > 30 ? data.note.substring(0, 30) + '...' : data.note) : '-' }}
        </span>
      </template>
    </Column>

    <!-- İşlem Butonları -->
    <Column header="İşlemler" style="width: 200px">
      <template #body="{ data }">
        <div class="flex gap-2">
          <!-- Düzenle Butonu -->
          <router-link
            :to="{ 
              name: data.invoiceType === 2 ? 'UpdateImportPurchase' : 'UpdateLocalPurchase', 
              params: { id: data.id } 
            }"
            type="button"
            class="text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:focus:ring-yellow-900"
          >
            Düzenle
          </router-link>

          <!-- Sil Butonu -->
          <button
            @click="confirmDelete(data)"
            type="button"
            class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900"
          >
            Sil
          </button>

          <!-- Dropdown İşlemler -->
          <TableDropdownButton buttonText="İşlemler">
            <a
              href="#"
              @click.prevent="viewDetails(data)"
              class="dropdown-item"
            >
              Detayları Görüntüle
            </a>

            <a
              href="#"
              @click.prevent="viewLines(data)"
              class="dropdown-item"
            >
              Fatura Satırları
            </a>

            <a
              href="#"
              @click.prevent="viewExpenses(data)"
              class="dropdown-item"
            >
              Masraflar
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
</template>

<script setup>
import { useRouter } from "vue-router";
import { useConfirm } from "primevue/useconfirm";
import ConfirmDialog from "primevue/confirmdialog";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import { usePurchaseStore } from "@/stores/purchase.store";
import { storeToRefs } from "pinia";
import TableDropdownButton from "@/components/UI/Buttons/TableDropdownButton.vue";

// Import enum helpers
import {
  getInvoiceTypeLabel,
  getInvoiceTypeColor,
  getStatusLabel,
  getStatusColor,
} from "@/services/constants/purchase-enum";

const router = useRouter();
const confirm = useConfirm();

const purchaseStore = usePurchaseStore();
await purchaseStore.fetchPurchases();
const { purchases, loading } = storeToRefs(purchaseStore);

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

const formatCurrency = (amount, currency = "₺", decimals = 2) => {
  if (amount === null || amount === undefined) return "-";
  return new Intl.NumberFormat("tr-TR", {
    minimumFractionDigits: decimals,
    maximumFractionDigits: decimals,
  }).format(amount) + (currency ? ` ${currency}` : "");
};

const calculateInvoiceTotal = (invoice) => {
  if (!invoice.purchaseInvoiceLines || invoice.purchaseInvoiceLines.length === 0) {
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
