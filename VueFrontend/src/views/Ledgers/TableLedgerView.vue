<template>
  <div class="flex justify-between items-center mb-4">
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Muhasebe Fişleri
    </h2>
    <div class="flex gap-2">
      <button
        @click="router.push({ name: 'create-ledger' })"
        type="button"
        class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
      >
        <span>➕</span>
        Yeni Muhasebe Fişi
      </button>
    </div>
  </div>
  <ConfirmDialog />

  <DataTable
    v-model:expandedRows="expandedRows"
    :value="ledgers"
    dataKey="id"
    responsiveLayout="scroll"
    :loading="loading"
    @rowExpand="onRowExpand"
    @rowCollapse="onRowCollapse"
  >
    <template #header>
      <div class="flex flex-wrap justify-end gap-2">
        <button
          @click="expandAll"
          type="button"
          class="flex items-center gap-2 text-gray-700 bg-gray-100 hover:bg-gray-200 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-4 py-2 focus:outline-none dark:bg-gray-600 dark:hover:bg-gray-700 dark:text-white dark:focus:ring-gray-800"
        >
          <span>📖</span>
          Tümünü Genişlet
        </button>
        <button
          @click="collapseAll"
          type="button"
          class="flex items-center gap-2 text-gray-700 bg-gray-100 hover:bg-gray-200 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-4 py-2 focus:outline-none dark:bg-gray-600 dark:hover:bg-gray-700 dark:text-white dark:focus:ring-gray-800"
        >
          <span>📕</span>
          Tümünü Daralt
        </button>
      </div>
    </template>

    <!-- Expander Column -->
    <Column expander style="width: 5rem" />

    <!-- Sıra Numarası -->
    <Column header="#" style="width: 60px">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>

    <!-- Referans No -->
    <Column field="referenceNo" header="Referans No" style="min-width: 120px" />

    <!-- Belge Tarihi -->
    <Column field="documentDate" header="Belge Tarihi" style="min-width: 120px">
      <template #body="{ data }">
        {{ formatDate(data.documentDate) }}
      </template>
    </Column>

    <!-- Belge Türü -->
    <Column field="documentType" header="Belge Türü" style="min-width: 120px">
      <template #body="{ data }">
        <span
          class="px-2 py-1 text-xs font-medium rounded-full"
          :class="getDocumentTypeColor(data.documentType)"
        >
          {{ getDocumentTypeLabel(data.documentType) }}
        </span>
      </template>
    </Column>

    <!-- Açıklama -->
    <Column field="description" header="Açıklama" style="min-width: 200px" />

    <!-- Toplam Borç -->
    <Column header="Toplam Borç" style="min-width: 120px">
      <template #body="{ data }">
        <span class="text-red-600 font-medium">
          {{ formatCurrency(calculateTotalDebit(data.ledgerEntries)) }}
        </span>
      </template>
    </Column>

    <!-- Toplam Alacak -->
    <Column header="Toplam Alacak" style="min-width: 120px">
      <template #body="{ data }">
        <span class="text-green-600 font-medium">
          {{ formatCurrency(calculateTotalCredit(data.ledgerEntries)) }}
        </span>
      </template>
    </Column>

    <!-- Denge Durumu -->
    <Column header="Durum" style="min-width: 100px">
      <template #body="{ data }">
        <span
          class="px-2 py-1 text-xs font-medium rounded-full"
          :class="getBalanceStatusColor(data.ledgerEntries)"
        >
          {{ getBalanceStatusLabel(data.ledgerEntries) }}
        </span>
      </template>
    </Column>

    <!-- Status -->
    <Column field="status" header="Aktif/Pasif" style="min-width: 100px">
      <template #body="{ data }">
        <span
          class="px-2 py-1 text-xs font-medium rounded-full"
          :class="getStatusColor(data.status)"
        >
          {{ getStatusLabel(data.status) }}
        </span>
      </template>
    </Column>

    <!-- Actions -->
    <Column header="İşlemler" style="min-width: 150px">
      <template #body="{ data }">
        <div class="flex gap-2">
          <!-- Edit Button -->
          <button
            @click="editLedger(data.id)"
            class="p-2 text-blue-600 hover:text-blue-800 hover:bg-blue-100 dark:hover:bg-blue-900 rounded-lg transition-colors"
            title="Düzenle"
          >
            <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
              <path
                d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z"
              />
            </svg>
          </button>

          <!-- Delete Button -->
          <button
            @click="confirmDelete(data.id)"
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
        </div>
      </template>
    </Column>

    <!-- Expansion Template for Ledger Entries -->
    <template #expansion="slotProps">
      <div class="p-6 bg-gray-50 dark:bg-gray-900">
        <h5 class="text-lg font-semibold text-gray-800 dark:text-white mb-4">
          {{ slotProps.data.referenceNo }} - Muhasebe Kayıtları
        </h5>

        <div
          v-if="
            slotProps.data.ledgerEntries &&
            slotProps.data.ledgerEntries.length > 0
          "
        >
          <DataTable :value="slotProps.data.ledgerEntries" class="mt-4">
            <!-- Line Number -->
            <Column header="Sıra" style="min-width: 60px">
              <template #body="{ data }">
                {{ data.lineNo }}
              </template>
            </Column>

            <!-- Account -->
            <Column header="Hesap" style="min-width: 200px">
              <template #body="{ data }">
                <span class="text-sm text-gray-700 dark:text-gray-300">
                  {{ getAccountName(data.accountId) }}
                </span>
              </template>
            </Column>

            <!-- Partner -->
            <Column header="Partner" style="min-width: 150px">
              <template #body="{ data }">
                <span class="text-sm text-gray-700 dark:text-gray-300">
                  {{ getPartnerName(data.partnerId) }}
                </span>
              </template>
            </Column>

            <!-- Description -->
            <Column
              field="description"
              header="Açıklama"
              style="min-width: 200px"
            >
              <template #body="{ data }">
                <span class="text-sm text-gray-700 dark:text-gray-300">
                  {{ data.description || "-" }}
                </span>
              </template>
            </Column>

            <!-- Debit -->
            <Column header="Borç" style="min-width: 120px">
              <template #body="{ data }">
                <span class="text-red-600 font-medium">
                  {{ data.debit > 0 ? formatCurrency(data.debit) : "-" }}
                </span>
              </template>
            </Column>

            <!-- Credit -->
            <Column header="Alacak" style="min-width: 120px">
              <template #body="{ data }">
                <span class="text-green-600 font-medium">
                  {{ data.credit > 0 ? formatCurrency(data.credit) : "-" }}
                </span>
              </template>
            </Column>
          </DataTable>
        </div>

        <div v-else class="text-center py-4">
          <span class="text-gray-500 dark:text-gray-400"
            >Bu fiş için kayıt bulunamadı</span
          >
        </div>
      </div>
    </template>
  </DataTable>
</template>

<script setup>
import { onMounted, computed, ref } from "vue";
import { useRouter } from "vue-router";
import { useConfirm } from "primevue/useconfirm";
import { storeToRefs } from "pinia";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import ConfirmDialog from "primevue/confirmdialog";
import { useLedgerStore } from "@/stores/ledger.store";
import { useAccountStore } from "@/stores/account.store";
import { usePartnerStore } from "@/stores/partner.store";

// Router
const router = useRouter();

// Stores
const ledgerStore = useLedgerStore();
const accountStore = useAccountStore();
const partnerStore = usePartnerStore();

// PrimeVue confirm dialog
const confirm = useConfirm();

// Store refs
const { ledgers, loading } = storeToRefs(ledgerStore);
const { accounts } = storeToRefs(accountStore);
const { partners } = storeToRefs(partnerStore);

// Expansion state
const expandedRows = ref({});

// Methods
const formatDate = (dateString) => {
  if (!dateString) return "-";
  return new Date(dateString).toLocaleDateString("tr-TR");
};

const formatCurrency = (amount) => {
  return new Intl.NumberFormat("tr-TR", {
    style: "currency",
    currency: "TRY",
  }).format(amount || 0);
};

// Document type helpers
const documentTypeLabels = {
  1: "Alım Faturası",
  2: "Satış Faturası",
  3: "Ödeme",
  4: "Tahsilat",
  5: "Yevmiye",
};

const getDocumentTypeLabel = (type) => {
  return documentTypeLabels[type] || "Belirtilmemiş";
};

const getDocumentTypeColor = (type) => {
  const colors = {
    1: "bg-blue-100 text-blue-800",
    2: "bg-green-100 text-green-800",
    3: "bg-red-100 text-red-800",
    4: "bg-purple-100 text-purple-800",
    5: "bg-gray-100 text-gray-800",
  };
  return colors[type] || "bg-gray-100 text-gray-800";
};

// Status helpers
const getStatusLabel = (status) => {
  return status === 1 ? "Aktif" : "Pasif";
};

const getStatusColor = (status) => {
  return status === 1
    ? "bg-green-100 text-green-800"
    : "bg-red-100 text-red-800";
};

// Ledger balance helpers
const calculateTotalDebit = (entries) => {
  if (!entries) return 0;
  return entries.reduce(
    (sum, entry) => sum + (parseFloat(entry.debit) || 0),
    0
  );
};

const calculateTotalCredit = (entries) => {
  if (!entries) return 0;
  return entries.reduce(
    (sum, entry) => sum + (parseFloat(entry.credit) || 0),
    0
  );
};

const getBalanceStatusLabel = (entries) => {
  const debit = calculateTotalDebit(entries);
  const credit = calculateTotalCredit(entries);
  const isBalanced = Math.abs(debit - credit) < 0.01;
  return isBalanced ? "✓ Dengeli" : "⚠ Dengesiz";
};

const getBalanceStatusColor = (entries) => {
  const debit = calculateTotalDebit(entries);
  const credit = calculateTotalCredit(entries);
  const isBalanced = Math.abs(debit - credit) < 0.01;
  return isBalanced ? "bg-green-100 text-green-800" : "bg-red-100 text-red-800";
};

// Expansion methods
const onRowExpand = (event) => {
};

const onRowCollapse = (event) => {
};

const expandAll = () => {
  expandedRows.value = ledgers.value.reduce((acc, ledger) => {
    acc[ledger.id] = true;
    return acc;
  }, {});
};

const collapseAll = () => {
  expandedRows.value = {};
};

// Helper methods for ledger entries
const getAccountName = (accountId) => {
  if (!accountId || !accounts.value) return "-";
  const account = accounts.value.find((acc) => acc.id === accountId);
  return account ? `${account.code} - ${account.name}` : "Hesap bulunamadı";
};

const getPartnerName = (partnerId) => {
  if (!partnerId || !partners.value) return "-";
  const partner = partners.value.find((p) => p.id === partnerId);
  return partner ? partner.name : "Partner bulunamadı";
};
const editLedger = (id) => {
  router.push({ name: "update-ledger", params: { id } });
};

const confirmDelete = (id) => {
  confirm.require({
    message: "Bu muhasebe fişini silmek istediğinize emin misiniz?",
    header: "Silme Onayı",
    icon: "pi pi-exclamation-triangle",
    rejectClass: "p-button-secondary p-button-outlined",
    acceptClass: "p-button-danger",
    rejectLabel: "İptal",
    acceptLabel: "Sil",
    accept: () => {
      deleteLedger(id);
    },
  });
};

const deleteLedger = async (id) => {
  await ledgerStore.deleteLedger(id);
};

// Lifecycle
await Promise.all([
  ledgerStore.fetchLedgers(),
  accountStore.fetchAccounts(),
  partnerStore.fetchPartners(),
]);
</script>
