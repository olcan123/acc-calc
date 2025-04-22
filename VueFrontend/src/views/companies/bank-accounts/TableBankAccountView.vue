<template>
  <div class="p-6">
    <div class="flex items-center justify-between mb-4">
      <h1 class="text-3xl font-bold text-gray-900 dark:text-white">Banka Hesapları</h1>
      <router-link
        :to="{ name: 'create-bank-account', params: { id: company.data.id } }"
        class="px-4 py-2 text-sm text-white bg-blue-700 hover:bg-blue-800 rounded-lg"
      >
        Yeni Banka Hesabi Ekle
      </router-link>
    </div>

    <!-- Main DataTable -->
    <DataTable
      :value="company.data.bankAccounts"
      dataKey="id"
      paginator
      :rows="5"
      stripedRows
      responsiveLayout="scroll"
      class="rounded-lg shadow border border-surface-300"
    >
      <!-- Expand Column -->
      <!-- Basic Info -->
      <Column field="id" header="ID" />
      <Column header="Banka">
        <template #body="{ data }">
          {{ getBankBankName(data.bankId) }}
        </template>
      </Column>
      <Column field="branchName" header="Şube" />
      <Column field="iban" header="IBAN" />
      <Column field="accountNumber" header="Hesap No" />
      <Column field="currency" header="Para Birimi" />

      <!-- Action Column -->
      <Column header="İşlem" headerClass="bg-surface-100 text-color-emphasis text-xs font-semibold text-center">
        <template #body="{ data }">
          <div class="flex justify-center">
            <router-link
              :to="{ name: 'update-bank-account', params: { bankAccountId: data.id } }"
              class="px-4 py-2 text-xs font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 rounded-lg"
            >
              Düzenle
            </router-link>

            <button
              type="button"
              @click="bankAccountStore.deleteBankAccount(data.id)"
              class="px-4 py-2 text-xs font-medium text-white bg-red-600 hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500 rounded-lg"
            >
              Sil
            </button>
          </div>
        </template>
      </Column>
    </DataTable>
  </div>
</template>

<script setup>
import { storeToRefs } from "pinia";
import { useRoute } from "vue-router";
import { useCompanyStore } from "@/stores/company.store";
import { useBankStore } from "@/stores/bank.store";
import { useBankAccountStore } from "@/stores/bank-account.store";
import { DataTable, Column } from "primevue";

const { id } = useRoute().params;

const companyStore = useCompanyStore();
const bankStore = useBankStore();
const bankAccountStore = useBankAccountStore();
const { company } = storeToRefs(companyStore);
const { getSelectBanks } = storeToRefs(bankStore);
await companyStore.fetchCompanyWithInclude(id);
await bankStore.fetchBanks();

const getBankBankName = (bankId) => {
  const bank = getSelectBanks.value.find((bank) => bank.value === bankId);
  return bank ? bank.label : "Banka Bulunamadı";
};
</script>
