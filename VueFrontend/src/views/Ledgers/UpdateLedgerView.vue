<template>  <!-- Loading State -->
  <div v-if="loading || !formValues.ledger?.id" class="flex justify-center items-center h-64">
    <div class="animate-spin rounded-full h-32 w-32 border-b-2 border-blue-500"></div>
  </div>

  <!-- Form Content -->
  <div v-else>
    <!-- Page Header -->
    <div
      class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4"
    >
      <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
        Muhasebe Fişi Düzenle - {{ formValues.ledger?.referenceNo }}
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
        {{ loading ? "Güncelleniyor..." : "Güncelle" }}
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
        </div>

        <!-- Entries Table -->
        <div class="overflow-x-auto">
          <table
            class="w-full text-sm text-left text-gray-500 dark:text-gray-400"
          >            <LedgerEntryTableHeader />
            <tbody>
              <LedgerEntryTableRow />
            </tbody>
          </table>
        </div>

        <!-- Summary -->
        <LedgerTotalSummary />
      </div>
    </form>
  </div>
</template>

<script setup>
import { onMounted, computed, nextTick } from "vue";
import { useForm, useFieldArray } from "vee-validate";
import { useRouter, useRoute } from "vue-router";
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
const route = useRoute();

// Stores
const ledgerStore = useLedgerStore();
const accountStore = useAccountStore();
const partnerStore = usePartnerStore();

// Store refs
const { loading, ledger, ledgerEntries } = storeToRefs(ledgerStore);

// Get ledger ID from route
const ledgerId = computed(() => parseInt(route.params.id));

// Initial form values
const initialValues = {
  ledger: {
    id: 0,
    documentType: 5,
    documentDate: new Date().toISOString().split('T')[0],
    referenceNo: "",
    description: "",
    status: 1
  },
  ledgerEntries: []
};

// Form setup
const { handleSubmit, values: formValues, meta, submitCount, setValues } = useForm({
  initialValues,
});

// Field array for ledger entries
const { push: pushEntry } = useFieldArray("ledgerEntries");

// Methods
const addNewEntry = () => {
  const newEntry = {
    id: 0,
    ledgerId: ledgerId.value,
    partnerId: null,
    lineNo: formValues.ledgerEntries.length + 1,
    accountId: 0,
    description: "",
    debit: 0,
    credit: 0
  };
  pushEntry(newEntry);
};

const loadLedgerData = async () => {
  try {
    await ledgerStore.fetchLedger(ledgerId.value);
    
    // Wait for reactive refs to be updated
    await nextTick();
    
    // Set form values from store refs
    if (ledger.value) {
      const ledgerData = {
        ledger: {
          ...ledger.value,
          documentDate: ledger.value.documentDate?.split('T')[0] || new Date().toISOString().split('T')[0]
        },
        ledgerEntries: (ledgerEntries.value && ledgerEntries.value.length > 0) ? ledgerEntries.value : [
          {
            id: 0,
            ledgerId: ledgerId.value,
            partnerId: null,
            lineNo: 1,
            accountId: 0,
            description: "",
            debit: 0,
            credit: 0
          }
        ]
      };
      
      setValues(ledgerData);
    }
  } catch (error) {
    console.error("Error loading ledger:", error);
    router.push({ name: "table-ledger" });
  }
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

    await ledgerStore.updateLedger(payload);
  } catch (error) {
    console.error("Ledger update error:", error);
  }
});

// Lifecycle
onMounted(async () => {
  await Promise.all([
    accountStore.fetchAccounts(),
    partnerStore.fetchPartners(),
    loadLedgerData()
  ]);
});
</script>
