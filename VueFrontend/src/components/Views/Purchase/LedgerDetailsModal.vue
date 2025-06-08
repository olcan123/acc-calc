<template>
  <div v-if="loading" class="flex justify-center items-center p-8">
    <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
  </div>
  
  <div v-else-if="ledgerData" class="space-y-6">
    <!-- Defter Bilgileri -->
    <div class="bg-gray-50 dark:bg-gray-700 rounded-lg p-4">
      <h4 class="text-md font-semibold text-gray-900 dark:text-white mb-3">
        Defter Bilgileri
      </h4>
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
          <span class="ml-2 text-gray-900 dark:text-white">{{ ledgerData.referenceNo || '-' }}</span>
        </div>
        <div>
          <span class="font-medium text-gray-600 dark:text-gray-300">Durum:</span>
          <span class="ml-2" :class="getStatusClass(ledgerData.status)">
            {{ getStatusLabel(ledgerData.status) }}
          </span>
        </div>
        <div class="col-span-2">
          <span class="font-medium text-gray-600 dark:text-gray-300">Açıklama:</span>
          <span class="ml-2 text-gray-900 dark:text-white">{{ ledgerData.description || '-' }}</span>
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
            <div>
              <div class="font-medium text-gray-900 dark:text-white">
                {{ getAccountName(data.accountId) }}
              </div>
              <div class="text-xs text-gray-500 dark:text-gray-400">
                ID: {{ data.accountId }}
              </div>
            </div>
          </template>
        </Column>

        <!-- Partner -->
        <Column header="Partner" style="min-width: 120px">
          <template #body="{ data }">
            <span v-if="data.partnerId" class="text-gray-900 dark:text-white">
              {{ getPartnerName(data.partnerId) }}
            </span>
            <span v-else class="text-gray-500 dark:text-gray-400">-</span>
          </template>
        </Column>

        <!-- Açıklama -->
        <Column field="description" header="Açıklama" style="min-width: 200px">
          <template #body="{ data }">
            <span class="text-sm text-gray-900 dark:text-white">{{ data.description }}</span>
          </template>
        </Column>        <!-- Borç -->
        <Column header="Borç" style="min-width: 120px" class="text-right">
          <template #body="{ data }">
            <span class="font-medium" :class="data.debit > 0 ? 'text-green-600 dark:text-green-400' : 'text-gray-400'">
              {{ formatCurrency(data.debit) }}
            </span>
          </template>
        </Column>

        <!-- Alacak -->
        <Column header="Alacak" style="min-width: 120px" class="text-right">
          <template #body="{ data }">
            <span class="font-medium" :class="data.credit > 0 ? 'text-red-600 dark:text-red-400' : 'text-gray-400'">
              {{ formatCurrency(data.credit) }}
            </span>
          </template>
        </Column>

        <!-- Bakiye -->
        <Column header="Bakiye" style="min-width: 140px" class="text-right">
          <template #body="{ data }">
            <span class="font-semibold" :class="getRunningBalance(data.lineNo) >= 0 ? 'text-green-600 dark:text-green-400' : 'text-red-600 dark:text-red-400'">
              {{ formatCurrency(getRunningBalance(data.lineNo)) }}
            </span>
          </template>
        </Column>
      </DataTable>      <!-- Toplam Satırı -->
      <div class="mt-4 p-3 bg-gray-50 dark:bg-gray-700 rounded-lg">
        <div class="flex justify-between items-center text-sm font-medium">
          <span class="text-gray-900 dark:text-white">Toplam:</span>          <div class="flex gap-8">
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
                {{ formatCurrency(finalBalance) }}
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
import { usePurchaseStore } from '@/stores/purchase.store';
import { useAccountStore } from '@/stores/account.store';
import { usePartnerStore } from '@/stores/partner.store';

const props = defineProps({
  ledgerId: {
    type: Number,
    required: true
  }
});

const purchaseStore = usePurchaseStore();
const accountStore = useAccountStore();
const partnerStore = usePartnerStore();

const { purchases } = storeToRefs(purchaseStore);
const { accounts } = storeToRefs(accountStore);
const { partners } = storeToRefs(partnerStore);

const loading = ref(false);
const ledgerData = ref(null);

// Computed properties for totals
const totalDebit = computed(() => {
  if (!ledgerData.value?.ledgerEntries) return 0;
  return ledgerData.value.ledgerEntries.reduce((sum, entry) => sum + (entry.debit || 0), 0);
});

const totalCredit = computed(() => {
  if (!ledgerData.value?.ledgerEntries) return 0;
  return ledgerData.value.ledgerEntries.reduce((sum, entry) => sum + (entry.credit || 0), 0);
});

const balanceDifference = computed(() => {
  return Math.abs(totalDebit.value - totalCredit.value);
});

const finalBalance = computed(() => {
  return totalDebit.value - totalCredit.value;
});

// Helper functions
const formatDate = (date) => {
  if (!date) return '-';
  return new Date(date).toLocaleDateString('tr-TR');
};

const formatDateTime = (date) => {
  if (!date) return '-';
  return new Date(date).toLocaleString('tr-TR');
};

const formatCurrency = (amount, decimals = 2) => {
  if (amount === null || amount === undefined) return '-';
  return new Intl.NumberFormat('de-DE', {
    style: 'currency',
    currency: 'EUR',
    minimumFractionDigits: decimals,
    maximumFractionDigits: decimals,
  }).format(amount);
};

const getDocumentTypeLabel = (type) => {
  const types = {
    1: 'Satın Alma Faturası',
    2: 'Satış Faturası',
    3: 'Ödeme',
    4: 'Tahsilat'
  };
  return types[type] || 'Bilinmeyen';
};

const getStatusLabel = (status) => {
  const statuses = {
    1: 'Aktif',
    2: 'Pasif',
    3: 'İptal'
  };
  return statuses[status] || 'Bilinmeyen';
};

const getStatusClass = (status) => {
  const classes = {
    1: 'text-green-600 dark:text-green-400',
    2: 'text-yellow-600 dark:text-yellow-400',
    3: 'text-red-600 dark:text-red-400'
  };
  return classes[status] || 'text-gray-600 dark:text-gray-400';
};

const getAccountName = (accountId) => {
  const account = accounts.value.find(acc => acc.id === accountId);
  return account ? account.name : `Hesap ${accountId}`;
};

const getPartnerName = (partnerId) => {
  const partner = partners.value.find(p => p.id === partnerId);
  return partner ? partner.name : `Partner ${partnerId}`;
};

// Calculate running balance for each entry
const getRunningBalance = (lineNo) => {
  if (!ledgerData.value?.ledgerEntries) return 0;
  
  // Sort entries by line number and calculate running balance up to the current line
  const sortedEntries = [...ledgerData.value.ledgerEntries].sort((a, b) => a.lineNo - b.lineNo);
  let runningBalance = 0;
  
  for (const entry of sortedEntries) {
    runningBalance += (entry.debit || 0) - (entry.credit || 0);
    
    if (entry.lineNo === lineNo) {
      break;
    }
  }
  
  return runningBalance;
};

// Fetch ledger data
const fetchLedgerData = async () => {
  loading.value = true;
  try {
    // Fetch the purchases if not already loaded
    if (!purchases.value || purchases.value.length === 0) {
      await purchaseStore.fetchPurchases();
    }
    
    // Find the purchase that contains this ledger
    const purchase = purchases.value.find(p => p.ledger?.id === props.ledgerId);
    
    if (purchase?.ledger) {
      ledgerData.value = purchase.ledger;
    } else {
      // If not found, the ledger data might not be loaded with the purchase list
      // Try to fetch the specific purchase details
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
