<template>
  <!-- Header Section -->
  <div class="flex justify-between items-center mb-4">
    <h2 class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight">
      Ölçü Birimleri Tablosu
    </h2>

    <button
      @click="router.push({ name: 'create-unit-of-measure' })"
      type="button"
      class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
    >
      Yeni Ölçü Birimi Oluştur
    </button>
  </div>
  <!-- DataTable -->
  <DataTable 
    v-model:filters="filters"
    :value="unitOfMeasures" 
    responsiveLayout="scroll" 
    dataKey="id"
    paginator 
    :rows="10" 
    filterDisplay="menu"
    :loading="loading"
    sortMode="multiple"
    :globalFilterFields="['name', 'abbreviation']"
    :emptyMessage="unitOfMeasures?.length === 0 ? 'Henüz hiç ölçü birimi eklenmemiş.' : 'Arama kriterlerinize uygun ölçü birimi bulunamadı.'"
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
          <span class="text-gray-600 dark:text-gray-400">Ölçü birimleri yükleniyor...</span>
        </div>
      </div>
    </template>

    <template #empty>
      <div class="flex flex-col items-center justify-center p-8 text-gray-500 dark:text-gray-400">
        <svg class="w-16 h-16 mb-4 text-gray-300 dark:text-gray-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1" d="M9 5H7a2 2 0 00-2 2v10a2 2 0 002 2h8a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01"></path>
        </svg>
        <p class="text-lg font-medium mb-1">{{ unitOfMeasures?.length === 0 ? 'Henüz hiç ölçü birimi eklenmemiş' : 'Sonuç bulunamadı' }}</p>
        <p class="text-sm">{{ unitOfMeasures?.length === 0 ? 'İlk ölçü biriminizi oluşturmak için yukarıdaki butonu kullanın.' : 'Arama kriterlerinizi değiştirmeyi deneyin.' }}</p>
      </div>
    </template>

    <Column header="ID" style="min-width: 80px">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>
    <Column field="name" header="Ad" sortable style="min-width: 200px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Ad ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="abbreviation" header="Kısaltma" sortable style="min-width: 150px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Kısaltma ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>    <!-- İşlemler -->
    <Column header="İşlemler" style="min-width: 150px">
      <template #body="{ data }">
        <div class="flex gap-2">
          <!-- Edit Button -->
          <button
            @click="updateUnitOfMeasure(data.id)"
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
            @click="confirmDeleteUnitOfMeasure(data.id)"
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
  </DataTable>
  
  <!-- ConfirmDialog popup -->
  <ConfirmDialog />
</template>

<script setup>
import { DataTable, Column, ConfirmDialog } from "primevue";
import { FilterMatchMode, FilterOperator } from "@primevue/core/api";
import { useRouter } from "vue-router";
import { ref } from "vue";
import { storeToRefs } from "pinia";
import { useUnitOfMeasureStore } from "@/stores/unit-of-measure.store";
import { useConfirm } from "primevue/useconfirm";

// Router & Store
const router = useRouter();
const unitOfMeasureStore = useUnitOfMeasureStore();
const { unitOfMeasures, loading } = storeToRefs(unitOfMeasureStore);

// PrimeVue Confirm service
const confirm = useConfirm();

// Filters
const filters = ref();

const initFilters = () => {
  filters.value = {
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    name: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] },
    abbreviation: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] }
  };
};

initFilters();

const clearFilter = () => {
  initFilters();
};

// Fetch unit of measures
await unitOfMeasureStore.fetchUnitOfMeasures();

// Silme işlemi onaylı
const confirmDeleteUnitOfMeasure = (unitId) => {
  confirm.require({
    message: "Bu ölçü birimini silmek istediğinize emin misiniz?",
    header: "Silme Onayı",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    acceptClass: "p-button-danger",
    rejectClass: "p-button-secondary",
    accept: async () => {
      await unitOfMeasureStore.deleteUnitOfMeasure(unitId);
    },
    reject: () => {
      console.log("Silme işlemi iptal edildi.");
    },
  });
};

// Düzenleme
const updateUnitOfMeasure = (id) => {
  router.push({ name: "update-unit-of-measure", params: { id } });
};
</script>
