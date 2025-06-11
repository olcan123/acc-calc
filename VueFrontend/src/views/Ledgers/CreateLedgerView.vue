<template>
  <!-- Page Header -->
  <div
    class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4"
  >
    <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
      Yeni Muhasebe Fişi Oluştur
    </h2>

    <button
      type="submit"
      form="ledgerForm"
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
  <form @submit="onSubmit" id="ledgerForm" class="space-y-6">
    <!-- Ledger Info Card -->
    <LedgerInfoCard />

    <!-- Ledger Entries Section -->
    <div class="bg-white dark:bg-gray-800 rounded-lg shadow">
      <div class="p-6 border-b border-gray-200 dark:border-gray-700">
        <div class="flex justify-between items-center">
          <h3 class="text-lg font-medium text-gray-800 dark:text-white">
            Muhasebe Kayıtları
          </h3>
          <div class="flex gap-2">
            <button
              type="button"
              @click="addNewEntry"
              class="flex items-center gap-2 text-white bg-green-600 hover:bg-green-700 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-4 py-2 dark:bg-green-500 dark:hover:bg-green-600 dark:focus:ring-green-800"
            >
              <span>➕</span>
              Kayıt Ekle
            </button>
          </div>
        </div>
      </div>      <!-- Entries Table -->
      <div class="overflow-x-auto" v-if="dataReady">
        <table
          class="w-full text-sm text-left text-gray-500 dark:text-gray-400"
        >          <LedgerEntryTableHeader />
          <tbody>
            <LedgerEntryTableRow />
          </tbody>
        </table>
      </div>
      
      <!-- Loading state -->
      <div v-else class="flex justify-center items-center py-8">
        <div class="text-gray-500 dark:text-gray-400">
          <span class="animate-spin inline-block mr-2">⏳</span>
          Veriler yükleniyor...
        </div>
      </div>

      <!-- Summary -->
      <LedgerTotalSummary />
    </div>
  </form>
</template>

<script setup>
import { onMounted, computed } from "vue";
import { useForm, useFieldArray } from "vee-validate";
import { useRouter } from "vue-router";
import { storeToRefs } from "pinia";

// Components
import {
  LedgerInfoCard,
  LedgerEntryTableHeader,
  LedgerEntryTableRow,
  LedgerTotalSummary,
} from "@/components/Views/Ledgers";

// Stores
import { useLedgerStore } from "@/stores/ledger.store";
import { useAccountStore } from "@/stores/account.store";
import { usePartnerStore } from "@/stores/partner.store";

// Router
const router = useRouter();

// Stores
const ledgerStore = useLedgerStore();
const accountStore = useAccountStore();
const partnerStore = usePartnerStore();

// Store refs
const { loading } = storeToRefs(ledgerStore);
const { accounts, loading: accountLoading } = storeToRefs(accountStore);
const { partners, loading: partnerLoading } = storeToRefs(partnerStore);

// Initial form values
const initialValues = {
  ledger: {
    id: 0,
    documentType: 5, // Default to Journal
    documentDate: new Date().toISOString().split('T')[0],
    referenceNo: "",
    description: "",
    status: 1
  },
  ledgerEntries: [
    {
      id: 0,
      ledgerId: 0,
      partnerId: null,
      lineNo: 1,
      accountId: 0,
      description: "",
      debit: 0,
      credit: 0
    }
  ]
};

// Form setup
const { handleSubmit, values: formValues, meta, submitCount } = useForm({
  initialValues,
});

// Field array for ledger entries
const { push: pushEntry } = useFieldArray("ledgerEntries");

// Computed values
const dataReady = computed(() => {
  const hasAccounts = accounts.value && accounts.value.length > 0;
  const hasPartners = partners.value && partners.value.length > 0;
  return hasAccounts && hasPartners && !accountLoading.value && !partnerLoading.value;
});

// Methods
const addNewEntry = () => {
  const newEntry = {
    id: 0,
    ledgerId: 0,
    partnerId: null,
    lineNo: formValues.ledgerEntries.length + 1,
    accountId: 0,
    description: "",
    debit: 0,
    credit: 0
  };
  pushEntry(newEntry);
};

// Form submit handler
const onSubmit = handleSubmit(async (values) => {
  try {
    // Validate balance
    const totalDebit = values.ledgerEntries.reduce((sum, entry) => sum + (parseFloat(entry.debit) || 0), 0);
    const totalCredit = values.ledgerEntries.reduce((sum, entry) => sum + (parseFloat(entry.credit) || 0), 0);
    
    if (Math.abs(totalDebit - totalCredit) >= 0.01) {
      alert("Muhasebe fişi dengesiz! Borç ve alacak tutarları eşit olmalıdır.");
      return;
    }

    // Prepare data for API
    const payload = {
      ledger: values.ledger,
      ledgerEntries: values.ledgerEntries
    };

    await ledgerStore.addLedger(payload);
  } catch (error) {
    console.error("Ledger create error:", error);
  }
});

// Lifecycle
onMounted(async () => {
  try {
    await Promise.all([
      accountStore.fetchAccounts(),
      partnerStore.fetchPartners()
    ]);
  } catch (error) {
    console.error("Error loading data:", error);
    // Optionally show a toast or error message
  }
});
</script>
