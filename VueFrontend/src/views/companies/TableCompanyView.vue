<template>
  <!-- Header Section -->
  <div class="flex justify-between items-center mb-4">
    <!-- Yazıyı büyüt + y ekseninde hizala -->
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Şirket Tablosu
    </h2>

    <!-- Buton boyutunu koru, ama içerik dikey ortalansın -->
    <button
      :disabled="companies.length > 0"
      @click="router.push({ name: 'create-company' })"
      type="button"
      class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
    >
      <span v-if="companies.length > 0">🚫</span>
      Yeni Şirket Oluştur
    </button>
  </div>
  <!-- DataTable -->
  <DataTable 
    :value="companies" 
    v-model:filters="filters"
    responsiveLayout="scroll" 
    dataKey="id"
    paginator
    :rows="10"
    filterDisplay="menu"
    sortMode="multiple"
    :globalFilterFields="['name', 'tradeName', 'uidNumber', 'vatNumber']"
    :loading="loading"
  >
    <template #header>
      <div class="flex flex-wrap justify-between items-center gap-4">
        <!-- Clear Filter Button -->
        <button
          @click="clearFilter"
          type="button"
          class="flex items-center gap-2 text-gray-700 bg-gray-100 hover:bg-gray-200 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-4 py-2 focus:outline-none dark:bg-gray-600 dark:hover:bg-gray-700 dark:text-white dark:focus:ring-gray-800"
        >
          <span>🗑️</span>
          Filtreleri Temizle
        </button>

        <!-- Global Search -->
        <div class="flex items-center gap-2">
          <div class="relative">
            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <svg class="w-4 h-4 text-gray-500 dark:text-gray-400" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd"></path>
              </svg>
            </div>
            <input
              v-model="filters['global'].value"
              type="text"
              placeholder="Anahtar kelime ile ara..."
              class="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-md leading-5 bg-white placeholder-gray-500 focus:outline-none focus:placeholder-gray-400 focus:ring-1 focus:ring-blue-500 focus:border-blue-500 sm:text-sm dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
            />
          </div>
        </div>
      </div>
    </template>

    <template #empty>
      <div class="text-center py-8">
        <div class="text-gray-500 dark:text-gray-400">
          <svg class="mx-auto h-12 w-12 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
          </svg>
          <h3 class="mt-2 text-sm font-medium text-gray-900 dark:text-gray-100">Şirket bulunamadı</h3>
          <p class="mt-1 text-sm text-gray-500 dark:text-gray-400">Arama kriterlerinizi değiştirerek tekrar deneyin.</p>
        </div>
      </div>
    </template>

    <template #loading>
      <div class="text-center py-8">
        <div class="text-gray-500 dark:text-gray-400">
          <div class="animate-spin inline-block w-6 h-6 border-[3px] border-current border-t-transparent text-blue-600 rounded-full" role="status" aria-label="loading">
            <span class="sr-only">Yükleniyor...</span>
          </div>
          <p class="mt-2 text-sm">Şirketler yükleniyor. Lütfen bekleyin...</p>
        </div>
      </div>
    </template>    <Column header="ID" style="width: 60px">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>
    <Column field="name" header="Name" sortable style="min-width: 200px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Ad ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="tradeName" header="Trade Name" sortable style="min-width: 200px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Ticari ad ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="uidNumber" header="UID Number" sortable style="min-width: 120px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="UID ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="vatNumber" header="VAT Number" sortable style="min-width: 120px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="VAT ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="period" header="Period" sortable style="min-width: 100px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Dönem ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>    <!-- İşlemler -->
    <Column header="İşlemler" style="min-width: 200px">
      <template #body="{ data }">
        <div class="flex gap-2">
          <!-- Edit Button -->
          <EditButton @click="updateCompany(data.id)" />

          <!-- Delete Button -->
          <DeleteButton @click="confirmDeleteCompany(data.id)" />
          
          <!-- More Actions Dropdown -->
          <TableDropdownButton buttonText="İşlemler">
            <router-link
              :to="{ name: 'table-warehouse' }"
              class="dropdown-item"
            >
              <svg class="dropdown-item-icon" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M4 4a2 2 0 00-2 2v8a2 2 0 002 2h12a2 2 0 002-2V6a2 2 0 00-2-2H4zm0 2h12v8H4V6z" clip-rule="evenodd"/>
              </svg>
              Depolar
            </router-link>
            <router-link
              :to="{
                name: 'table-bank-account-company',
                params: { companyId: data.id },
              }"
              class="dropdown-item"
            >
              <svg class="dropdown-item-icon" fill="currentColor" viewBox="0 0 20 20">
                <path d="M4 4a2 2 0 00-2 2v1h16V6a2 2 0 00-2-2H4zM18 9H2v5a2 2 0 002 2h12a2 2 0 002-2V9zM4 13a1 1 0 011-1h1a1 1 0 110 2H5a1 1 0 01-1-1zm5-1a1 1 0 100 2h1a1 1 0 100-2H9z"/>
              </svg>
              Bankalar
            </router-link>
          </TableDropdownButton>
        </div>
      </template>
    </Column>
  </DataTable>
  <!-- ConfirmDialog popup -->
  <ConfirmDialog />
</template>

<script setup>
import { DataTable, Column, ConfirmDialog } from "primevue";
import { useRouter } from "vue-router";
import { storeToRefs } from "pinia";
import { useCompanyStore } from "@/stores/company.store";
import { useConfirm } from "primevue/useconfirm";
import TableDropdownButton from "@/components/UI/Buttons/TableDropdownButton.vue";
import EditButton from "@/components/UI/Buttons/EditButton.vue";
import DeleteButton from "@/components/UI/Buttons/DeleteButton.vue";
import { ref, onMounted } from "vue";
import { FilterMatchMode, FilterOperator } from "@primevue/core";

// Router & Store
const router = useRouter();
const companyStore = useCompanyStore();
const { companies, loading } = storeToRefs(companyStore);

// PrimeVue Confirm service
const confirm = useConfirm();

// Fetch companies
await companyStore.fetchCompanies();

// Silme işlemi onaylı
const confirmDeleteCompany = (companyId) => {
  confirm.require({
    message: "Bu şirketi silmek istediğinize emin misiniz?",
    header: "Silme Onayı",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    acceptClass: "p-button-danger",
    rejectClass: "p-button-secondary",
    accept: async () => {
      await companyStore.deleteCompany(companyId);
    },
    reject: () => {
      console.log("Silme işlemi iptal edildi.");
    },
  });
};

// Düzenleme
const updateCompany = (id) => {
  router.push({ name: "update-company", params: { id } });
};

// Filter state initialization
const initFilters = () => {
  filters.value = {
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    name: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    tradeName: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    uidNumber: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    vatNumber: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    period: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
  };
};

// Filter state
const filters = ref();
initFilters();

// Clear filters function
const clearFilter = () => {
  initFilters();
};
</script>
