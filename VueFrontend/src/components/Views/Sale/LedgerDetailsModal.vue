<template>
  <div v-if="loading" class="flex justify-center items-center p-8">
    <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
  </div>
  
  <div v-else-if="ledgerData" class="space-y-6">
    <!-- Defter Bilgileri -->
    <div class="bg-gray-50 dark:bg-gray-700 rounded-lg p-4">
      <div class="grid grid-cols-2 gap-4 text-sm">
        <div>
          <span class="font-medium text-gray-600 dark:text-gray-300">Belge Türü:</span>
          <span class="ml-2 text-gray-900 dark:text-white">{{ getDocumentTypeLabel(ledgerData.documentType) }}</span>
        </div>
        <div>
          <span class="font-medium text-gray-600 dark:text-gray-300">Belge Tarihi:</span>
          <span class="ml-2 text-gray-900 dark:text-white">{{ formatDate(ledgerData.documentDate) }}</span>
        </div>
        <div>
          <span class="font-medium text-gray-600 dark:text-gray-300">Referans No:</span>
          <span class="ml-2 text-gray-900 dark:text-white">{{ ledgerData.referenceNo || "-" }}</span>
        </div>
        <div>
          <span class="font-medium text-gray-600 dark:text-gray-300">Durum:</span>
          <span class="ml-2" :class="getStatusClass(ledgerData.status)">{{ getStatusLabel(ledgerData.status) }}</span>
        </div>
        <div class="col-span-2">
          <span class="font-medium text-gray-600 dark:text-gray-300">Açıklama:</span>
          <span class="ml-2 text-gray-900 dark:text-white">{{ ledgerData.description || "-" }}</span>
        </div>
      </div>
    </div>

    <!-- Defter Kayıtları Tablosu -->
    <div>
      <h4 class="text-md font-semibold text-gray-900 dark:text-white mb-3">
        Defter Kayıtları
      </h4>
      
      <DataTable
        :value="ledgerData.ledgerEntries"
        dataKey="id"
        responsiveLayout="scroll"
        class="p-datatable-sm"
        :paginator="false"
        stripedRows
      >
        <!-- Satır No -->
        <Column field="lineNo" header="Satır" style="width: 80px" />
          <!-- Hesap -->
        <Column header="Hesap" style="min-width: 150px">
          <template #body="{ data }">
            <div class="text-sm">
              <div class="font-mono text-gray-900 dark:text-white">{{ data.accountCode }}</div>
              <div class="text-gray-600 dark:text-gray-300">{{ getAccountNameWithStore(data.accountId) }}</div>
            </div>
          </template>
        </Column>

        <!-- Partner -->
        <Column header="Partner" style="min-width: 120px">
          <template #body="{ data }">
            <span class="text-sm text-gray-900 dark:text-white">
              {{ data.partnerId ? getPartnerNameWithStore(data.partnerId) : '-' }}
            </span>
          </template>
        </Column>

        <!-- Açıklama -->
        <Column field="description" header="Açıklama" style="min-width: 200px">
          <template #body="{ data }">
            <span class="text-sm text-gray-900 dark:text-white">{{ data.description || '-' }}</span>
          </template>
        </Column>

        <!-- Borç -->
        <Column header="Borç" style="min-width: 120px" class="text-right">
          <template #body="{ data }">
            <span v-if="data.debit && data.debit > 0" class="text-green-600 dark:text-green-400 font-medium">
              {{ formatCurrency(data.debit) }}
            </span>
            <span v-else class="text-gray-400">-</span>
          </template>
        </Column>

        <!-- Alacak -->
        <Column header="Alacak" style="min-width: 120px" class="text-right">
          <template #body="{ data }">
            <span v-if="data.credit && data.credit > 0" class="text-red-600 dark:text-red-400 font-medium">
              {{ formatCurrency(data.credit) }}
            </span>
            <span v-else class="text-gray-400">-</span>
          </template>
        </Column>

        <!-- Bakiye -->
        <Column header="Bakiye" style="min-width: 140px" class="text-right">
          <template #body="{ data }">
            <span class="font-semibold" :class="getRunningBalance(data.lineNo) >= 0 ? 'text-green-600 dark:text-green-400' : 'text-red-600 dark:text-red-400'">
              {{ formatCurrency(Math.abs(getRunningBalance(data.lineNo))) }}
            </span>
          </template>
        </Column>
      </DataTable>

      <!-- Toplam Satırı -->
      <div class="mt-4 p-3 bg-gray-50 dark:bg-gray-700 rounded-lg">
        <div class="flex justify-between items-center text-sm font-medium">
          <span class="text-gray-900 dark:text-white">Toplam:</span>
          <div class="flex gap-8">
            <div>
              <span class="text-gray-600 dark:text-gray-300 mr-2">Borç:</span>
              <span class="text-green-600 dark:text-green-400 font-semibold">
                {{ formatCurrency(totalDebit) }}
              </span>
            </div>
            <div>
              <span class="text-gray-600 dark:text-gray-300 mr-2">Alacak:</span>
              <span class="text-red-600 dark:text-red-400 font-semibold">
                {{ formatCurrency(totalCredit) }}
              </span>
            </div>
            <div>
              <span class="text-gray-600 dark:text-gray-300 mr-2">Net Bakiye:</span>
              <span :class="finalBalance >= 0 ? 'text-green-600 dark:text-green-400' : 'text-red-600 dark:text-red-400'" class="font-semibold">
                {{ formatCurrency(Math.abs(finalBalance)) }}
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>

  <div v-else class="text-center p-8 text-gray-500 dark:text-gray-400">
    Defter kayıtları bulunamadı.
  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import { useSaleStore } from '@/stores/sale.store';
import { useAccountStore } from '@/stores/account.store';
import { usePartnerStore } from '@/stores/partner.store';
import { useLedgerHelpers } from '@/composables/useLedgerHelpers';

const props = defineProps({
  ledgerId: {
    type: Number,
    required: true
  }
});

const saleStore = useSaleStore();
const accountStore = useAccountStore();
const partnerStore = usePartnerStore();

const { sales } = storeToRefs(saleStore);
const { accounts } = storeToRefs(accountStore);
const { partners } = storeToRefs(partnerStore);

const loading = ref(false);
const ledgerData = ref(null);

// Composable'dan helper fonksiyonları
const {
  formatDate,
  formatDateTime,
  formatCurrency,
  getDocumentTypeLabel,
  getStatusLabel,
  getStatusClass,
  getAccountName,
  getPartnerName,
  calculateRunningBalance,
  calculateTotalDebit,
  calculateTotalCredit,
  calculateFinalBalance,
} = useLedgerHelpers();

// Computed properties for totals
const totalDebit = computed(() => {
  return calculateTotalDebit(ledgerData.value?.ledgerEntries);
});

const totalCredit = computed(() => {
  return calculateTotalCredit(ledgerData.value?.ledgerEntries);
});

const finalBalance = computed(() => {
  return calculateFinalBalance(totalDebit.value, totalCredit.value);
});

// Helper functions that use stores
const getAccountNameWithStore = (accountId) => {
  return getAccountName(accountId, accounts.value);
};

const getPartnerNameWithStore = (partnerId) => {
  return getPartnerName(partnerId, partners.value);
};

const getRunningBalance = (lineNo) => {
  return calculateRunningBalance(lineNo, ledgerData.value?.ledgerEntries);
};

// Fetch ledger data
const fetchLedgerData = async () => {
  loading.value = true;
  try {
    // Fetch the sales if not already loaded
    if (!sales.value || sales.value.length === 0) {
      await saleStore.fetchSales();
    }
    
    // Find the sale that contains this ledger
    const sale = sales.value.find(s => s.ledger?.id === props.ledgerId);
    
    if (sale?.ledger) {
      ledgerData.value = sale.ledger;
    } else {
      // If not found, the ledger data might not be loaded with the sale list
      // Try to fetch the specific sale details
      ledgerData.value = null;
    }
  } catch (error) {
    console.error('Error fetching ledger data:', error);
    ledgerData.value = null;
  } finally {
    loading.value = false;
  }
};

// Watch for ledgerId changes
watch(() => props.ledgerId, () => {
  if (props.ledgerId) {
    fetchLedgerData();
  }
}, { immediate: true });

// Load initial data
onMounted(async () => {
  await accountStore.fetchAccounts();
  await partnerStore.fetchPartners();
  if (props.ledgerId) {
    await fetchLedgerData();
  }
});

</script>

<style scoped>
.p-datatable-sm :deep(.p-datatable-tbody > tr > td) {
  padding: 0.5rem;
}

.p-datatable-sm :deep(.p-datatable-thead > tr > th) {
  padding: 0.75rem 0.5rem;
  font-weight: 600;
}
</style>
