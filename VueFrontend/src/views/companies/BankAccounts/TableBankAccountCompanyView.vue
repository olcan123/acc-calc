<template>
  <!-- Header Section -->
  <div class="flex justify-between items-center mb-4">
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Şirkete Ait Banka Hesapları
    </h2>

    <button
      @click="
        router.push({
          name: 'create-bank-account-company',
          params: { companyId },
        })
      "
      type="button"
      class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
    >
      Yeni Banka Hesabı
    </button>
  </div>

  <!-- ConfirmDialog -->
  <ConfirmDialog />
  <!-- DataTable -->
  <DataTable
    v-model:filters="filters"
    :value="bankAccounts"
    dataKey="id"
    responsiveLayout="scroll"
    paginator 
    :rows="10" 
    filterDisplay="menu"
    :loading="loading"
    sortMode="multiple"
    :globalFilterFields="['bankAccount.branchName', 'bankAccount.accountNumber', 'bankAccount.iban', 'bankAccount.swiftCode']"
    :emptyMessage="bankAccounts?.length === 0 ? 'Henüz hiç banka hesabı eklenmemiş.' : 'Arama kriterlerinize uygun banka hesabı bulunamadı.'"
  >
    <template #header>
      <div class="flex flex-col md:flex-row md:justify-between md:items-center gap-4 p-4 bg-gray-50 dark:bg-gray-800 rounded-lg">
        <div class="flex items-center gap-2">
          <button
            @click="clearFilter()"
            class="flex items-center gap-2 px-3 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-gray-300 dark:hover:bg-gray-600"
          >
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path>
            </svg>
            Filtreleri Temizle
          </button>
        </div>
        <div class="relative">
          <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
            <svg class="w-4 h-4 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path>
            </svg>
          </div>
          <input
            v-model="filters['global'].value"
            type="text"
            placeholder="Genel arama..."
            class="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-md leading-5 bg-white placeholder-gray-500 focus:outline-none focus:placeholder-gray-400 focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
          />
        </div>
      </div>
    </template>

    <template #loading>
      <div class="flex items-center justify-center p-8">
        <div class="flex items-center space-x-2">
          <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-blue-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
          <span class="text-gray-600 dark:text-gray-400">Banka hesapları yükleniyor...</span>
        </div>
      </div>
    </template>

    <template #empty>
      <div class="flex flex-col items-center justify-center p-8 text-gray-500 dark:text-gray-400">
        <svg class="w-16 h-16 mb-4 text-gray-300 dark:text-gray-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1" d="M3 10h18M7 15h1m4 0h1m-7 4h12a3 3 0 003-3V8a3 3 0 00-3-3H6a3 3 0 00-3 3v8a3 3 0 003 3z"></path>
        </svg>
        <p class="text-lg font-medium mb-1">{{ bankAccounts?.length === 0 ? 'Henüz hiç banka hesabı eklenmemiş' : 'Sonuç bulunamadı' }}</p>
        <p class="text-sm">{{ bankAccounts?.length === 0 ? 'İlk banka hesabınızı oluşturmak için yukarıdaki butonu kullanın.' : 'Arama kriterlerinizi değiştirmeyi deneyin.' }}</p>
      </div>
    </template>

    <Column header="#" style="width: 60px">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>

    <Column header="Banka" sortable style="min-width: 150px">
      <template #body="{ data }">
        {{
          banks.find((bank) => bank.id === data.bankAccount.bankId)?.name || "—"
        }}
      </template>
      <template #filter="{ filterModel }">
        <select
          v-model="filterModel.value"
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
        >
          <option value="">Tüm Bankalar</option>
          <option v-for="bank in banks" :key="bank.id" :value="bank.name">{{ bank.name }}</option>
        </select>
      </template>
    </Column>
    <Column field="bankAccount.branchName" header="Şube" sortable style="min-width: 150px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Şube ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="bankAccount.accountNumber" header="Hesap No" sortable style="min-width: 200px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Hesap no ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="bankAccount.iban" header="IBAN" sortable style="min-width: 200px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="IBAN ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="bankAccount.swiftCode" header="SWIFT Kodu" sortable style="min-width: 150px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="SWIFT ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column header="Para Birimi" sortable style="min-width: 120px">
      <template #body="{ data }">
        {{ currencies.find((currency) => currency.id === data.bankAccount.currencyId)?.code || "—" }}
      </template>
      <template #filter="{ filterModel }">
        <select
          v-model="filterModel.value"
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
        >
          <option value="">Tüm Para Birimleri</option>
          <option v-for="currency in currencies" :key="currency.id" :value="currency.code">{{ currency.code }}</option>
        </select>
      </template>
    </Column>    <Column header="İşlemler" style="min-width: 150px">
      <template #body="{ data }">
        <div class="flex gap-2">
          <EditButton 
            @click="router.push({
              name: 'update-bank-account-company',
              params: { companyId, bankAccountId: data.bankAccountId },
            })"
          />
          <DeleteButton @click="confirmDelete({ companyId, bankAccountId: data.bankAccountId })" />
        </div>
      </template>
    </Column>
  </DataTable>
</template>

<script setup>
import { useRoute, useRouter } from "vue-router";
import { ref } from "vue";
import { storeToRefs } from "pinia";
import { useConfirm } from "primevue/useconfirm";
import EditButton from "@/components/UI/Buttons/EditButton.vue";
import DeleteButton from "@/components/UI/Buttons/DeleteButton.vue";
import { FilterMatchMode, FilterOperator } from "@primevue/core/api";
import ConfirmDialog from "primevue/confirmdialog";
import { useBankAccountCompanyStore } from "@/stores/bank-account-company.store";
import { useCurrencyStore } from "@/stores/currency.store";
import { useBankStore } from "@/stores/bank.store";
import DataTable from "primevue/datatable";
import Column from "primevue/column";

const route = useRoute();
const router = useRouter();
const confirm = useConfirm();
const { companyId } = route.params;

// Store
const currencyStore = useCurrencyStore();
const bankAccountStore = useBankAccountCompanyStore();
const bankStore = useBankStore();

const { bankAccounts, loading } = storeToRefs(bankAccountStore);
const { banks } = storeToRefs(bankStore);
const { currencies } = storeToRefs(currencyStore);

// Filters
const filters = ref();

const initFilters = () => {
  filters.value = {
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    'bankAccount.branchName': { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] },
    'bankAccount.accountNumber': { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] },
    'bankAccount.iban': { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] },
    'bankAccount.swiftCode': { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] }
  };
};

initFilters();

const clearFilter = () => {
  initFilters();
};

const confirmDelete = ({ companyId, bankAccountId }) => {
  confirm.require({
    message: "Bu banka hesabını silmek istediğinize emin misiniz?",
    header: "Onay",
    icon: "pi pi-exclamation-triangle",
    acceptClass: "p-button-danger",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    accept: async () => {
      await bankAccountStore.deleteBankAccount(companyId, bankAccountId);
    },
  });
};

await currencyStore.fetchCurrenciesOtions();
await bankStore.fetchBanks();
await bankAccountStore.fetchBankAccounts(companyId);
</script>
