<template>
  <tr
    class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600"
    v-for="(field, index) in fields"
    :key="`ledger-entry-${field.key}-${index}`"
  >
    <!-- Sıra No -->
    <td class="px-3 py-3 min-w-[60px]">
      <div class="flex items-center justify-center">
        <span class="text-sm font-medium text-gray-700 dark:text-gray-300">
          {{ index + 1 }}
        </span>
      </div>
    </td>
    <!-- Hesap Selection -->
    <td class="px-3 py-3 min-w-[200px]">
      <TableFieldSelect
        :fieldName="`ledgerEntries[${index}].accountId`"
        :options="optionAccounts || []"
        placeholder="Hesap seçin"
      />
    </td>

    <!-- Partner Selection -->
    <td class="px-3 py-3 min-w-[150px]">
      <TableFieldSelect
        :fieldName="`ledgerEntries[${index}].partnerId`"
        :options="optionPartners || []"
        placeholder="Partner seçin"
      />
    </td>

    <!-- Açıklama -->
    <td class="px-3 py-3 min-w-[200px]">
      <TableFieldTextInput
        :fieldName="`ledgerEntries[${index}].description`"
        placeholder="Açıklama girin"
      />
    </td>

    <!-- Borç Amount -->
    <td class="px-3 py-3 min-w-[120px] bg-red-50 dark:bg-red-900">
      <TableFieldNumberInput
        :fieldName="`ledgerEntries[${index}].debit`"
        placeholder="0.00"
        step="0.01"
        :class="'text-red-600 font-medium'"
      />
    </td>

    <!-- Alacak Amount -->
    <td class="px-3 py-3 min-w-[120px] bg-green-50 dark:bg-green-900">
      <TableFieldNumberInput
        :fieldName="`ledgerEntries[${index}].credit`"
        placeholder="0.00"
        step="0.01"
        :class="'text-green-600 font-medium'"
      />
    </td>

    <!-- Actions -->
    <td class="px-3 py-3 min-w-[80px]">
      <div class="flex items-center gap-2">
        <!-- Remove Row Button -->
        <button
          type="button"
          @click="removeLine(index)"
          :disabled="fields.length <= 1"
          class="p-1 text-red-600 hover:text-red-800 disabled:text-gray-400 disabled:cursor-not-allowed transition-colors"
          title="Satırı Sil"
        >
          <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
            <path
              fill-rule="evenodd"
              d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
              clip-rule="evenodd"
            />
          </svg>
        </button>
      </div>
    </td>
  </tr>
</template>

<script setup>
import { useFieldArray } from "vee-validate";
import { storeToRefs } from "pinia";
import TableFieldSelect from "@/components/TableForm/TableFieldSelect.vue";
import TableFieldNumberInput from "@/components/TableForm/TableFieldNumberInput.vue";
import TableFieldTextInput from "@/components/TableForm/TableFieldTextInput.vue";

// Stores
import { useAccountStore } from "@/stores/account.store";
import { usePartnerStore } from "@/stores/partner.store";

// Store instances
const accountStore = useAccountStore();
const partnerStore = usePartnerStore();

// Store refs with reactivity
const { optionAccounts } = storeToRefs(accountStore);
const { optionPartners } = storeToRefs(partnerStore);

// Field array for ledger entries
const { fields, remove } = useFieldArray("ledgerEntries");

// Methods
const onAccountChange = (index, accountId) => {
  console.log(`Account changed for entry ${index}:`, accountId);
};

const onPartnerChange = (index, partnerId) => {
  console.log(`Partner changed for entry ${index}:`, partnerId);
};

const handleDebitChange = (index, value) => {
  console.log(`Debit changed for entry ${index}:`, value);
  // VeeValidate will handle the form state
};

const handleCreditChange = (index, value) => {
  console.log(`Credit changed for entry ${index}:`, value);
  // VeeValidate will handle the form state
};

const removeLine = (index) => {
  if (fields.value.length > 1) {
    remove(index);
  }
};
</script>
