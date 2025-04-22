<template>
  <div class="p-6">
    <div class="flex items-center justify-between mb-4">
      <h1 class="text-3xl font-bold text-gray-900 dark:text-white">Şirketler</h1>
      <router-link
        :to="{ name: 'create-company' }"
        class="px-4 py-2 text-sm text-white bg-blue-700 hover:bg-blue-800 rounded-lg"
      >
        Yeni Şirket Ekle
      </router-link>
    </div>

    <!-- Main DataTable -->
    <DataTable
      :value="companies.data"
      v-model:expandedRows="expandedRows"
      dataKey="id"
      paginator
      :rows="5"
      stripedRows
      responsiveLayout="scroll"
      class="rounded-lg shadow border border-surface-300"
    >
      <!-- Expand Column -->
      <Column expander style="width: 5rem" />
      <!-- Basic Info -->
      <Column field="id" header="ID" />
      <Column field="name" header="Şirket Adı" />
      <Column field="tradeName" header="Ticari Unvan" />
      <Column field="uidNumber" header="UID No" />
      <Column field="vatNumber" header="KDV No" />
      <Column field="period" header="Dönem" />

      <!-- Action Column -->
      <Column header="İşlem" headerClass="bg-surface-100 text-color-emphasis text-xs font-semibold text-center">
        <template #body="{ data }">
          <div class="flex justify-center">
            <router-link
              :to="{ name: 'update-company', params: { id: data.id } }"
              class="px-4 py-2 text-xs font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 rounded-lg"
            >
              Düzenle
            </router-link>

            <button
              type="button"
              @click="companyStore.deleteCompany(data.id)"
              class="px-4 py-2 text-xs font-medium text-white bg-red-600 hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500 rounded-lg"
            >
              Sil
            </button>
          </div>
        </template>
      </Column>

      <!-- Row Expansion Template -->
      <template #expansion="slotProps">
        <div class="p-4 bg-gray-50 rounded-md space-y-6 text-sm text-gray-800">
          <!-- Bank Accounts Section -->
          <div v-if="slotProps.data.bankAccounts?.length">
            <div class="flex justify-between items-center mb-2">
              <h3 class="text-blue-700 font-semibold">🏦 Banka Hesapları</h3>
              <router-link
                :to="{ name: 'table-bank-account', params: { id: slotProps.data.id } }"
                class="text-xs text-white bg-green-600 hover:bg-green-700 px-3 py-1 rounded"
              >
                Hesapları Görüntüle
              </router-link>
            </div>
            <DataTable :value="slotProps.data.bankAccounts" class="text-sm border border-blue-200 rounded">
              <Column header="Banka">
                <template #body="{ data }">
                  {{ getBankBankName(data.bankId) }}
                </template>
              </Column>
              <Column field="branchName" header="Şube" />
              <Column field="iban" header="IBAN" />
              <Column field="accountNumber" header="Hesap No" />
              <Column field="currency" header="Para Birimi" />
            </DataTable>
          </div>

          <!-- Depolar ve Adresleri Listele -->
          <div v-if="slotProps.data.warehouses?.length">
            <div class="flex justify-between items-center mb-2">
              <h3 class="text-green-700 font-semibold">🏬 Depolar ve Adresleri</h3>
              <router-link
                class="text-xs text-white bg-green-600 hover:bg-green-700 px-3 py-1 rounded"
                  :to="{ name: 'table-warehouse', params: { id: slotProps.data.id } }"
              >
                Depoları Görüntüle
              </router-link>
            </div>

            <DataTable
              :value="flatWarehousesWithAddresses(slotProps.data.warehouses)"
              class="text-sm border border-green-200 rounded"
            >
              <Column field="warehouseName" header="Depo Adı" />
              <Column field="type" header="Adres Tipi" />
              <Column field="street" header="Sokak" />
              <Column field="city" header="Şehir" />
              <Column field="country" header="Ülke" />
              <Column field="zipCode" header="Posta Kodu" />
            </DataTable>
          </div>
        </div>
      </template>
    </DataTable>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { storeToRefs } from "pinia";
import { useCompanyStore } from "@/stores/company.store";
import { useBankStore } from "@/stores/bank.store";
import { DataTable, Column } from "primevue";
import router from "@/router";

const expandedRows = ref([]);

const companyStore = useCompanyStore();
const bankStore = useBankStore();
const { companies } = storeToRefs(companyStore);
const { getSelectBanks } = storeToRefs(bankStore);
await companyStore.fetchCompanies();
await bankStore.fetchBanks();

const getBankBankName = (bankId) => {
  const bank = getSelectBanks.value.find((bank) => bank.value === bankId);
  // return bank ? bank.value : 'Banka Bulunamadı'
  return bank ? bank.label : "Banka Bulunamadı";
};

// Depolar ve Adresleri Listele
function flatWarehousesWithAddresses(warehouses) {
  return warehouses.flatMap((warehouse) => {
    return (warehouse.addresses || []).map((address) => ({
      warehouseName: warehouse.name,
      ...address,
    }));
  });
}
</script>
