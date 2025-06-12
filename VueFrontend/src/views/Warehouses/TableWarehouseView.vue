<template>
  <!-- Header -->
  <div class="flex justify-between items-center mb-4">
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Depo Adres Tablosu
    </h2>

    <button
      @click="router.push({ name: 'create-warehouse' })"
      type="button"
      class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
    >
      + Yeni Depo Ekle
    </button>
  </div>
  <!-- DataTable -->
  <DataTable
    v-model:filters="filters"
    :value="flattenedWarehouses"
    responsiveLayout="scroll"
    dataKey="id"
    paginator 
    :rows="10" 
    filterDisplay="menu"
    :loading="loading"
    sortMode="multiple"
    :globalFilterFields="['warehouseName', 'street', 'city', 'country', 'zipCode']"
    :emptyMessage="flattenedWarehouses?.length === 0 ? 'Henüz hiç depo eklenmemiş.' : 'Arama kriterlerinize uygun depo bulunamadı.'"
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
          <span class="text-gray-600 dark:text-gray-400">Depolar yükleniyor...</span>
        </div>
      </div>
    </template>

    <template #empty>
      <div class="flex flex-col items-center justify-center p-8 text-gray-500 dark:text-gray-400">
        <svg class="w-16 h-16 mb-4 text-gray-300 dark:text-gray-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4"></path>
        </svg>
        <p class="text-lg font-medium mb-1">{{ flattenedWarehouses?.length === 0 ? 'Henüz hiç depo eklenmemiş' : 'Sonuç bulunamadı' }}</p>
        <p class="text-sm">{{ flattenedWarehouses?.length === 0 ? 'İlk deponuzu oluşturmak için yukarıdaki butonu kullanın.' : 'Arama kriterlerinizi değiştirmeyi deneyin.' }}</p>
      </div>
    </template>

    <Column field="warehouseName" header="Depo Adı" sortable style="min-width: 200px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Depo adı ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="street" header="Sokak" sortable style="min-width: 200px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Sokak ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="city" header="Şehir" sortable style="min-width: 150px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Şehir ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="country" header="Ülke" sortable style="min-width: 120px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Ülke ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="zipCode" header="Posta Kodu" sortable style="min-width: 120px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Posta kodu ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>    <Column header="İşlemler" style="min-width: 150px">
      <template #body="{ data }">
        <div class="flex gap-2">
          <!-- Edit Button -->
          <EditButton @click="updateWarehouse(data.id)" />

          <!-- Delete Button -->
          <DeleteButton @click="confirmDeleteWarehouse(data.id)" />

          <!-- Contact Link Button -->
          <ContactButton 
            :to="{
              name: 'table-contact-warehouse',
              params: { warehouseId: data.id },
            }"
          />
        </div>
      </template>
    </Column>
  </DataTable>

  <!-- ConfirmDialog -->
  <ConfirmDialog />
</template>

<script setup>
import { DataTable, Column } from "primevue";
import { FilterMatchMode, FilterOperator } from "@primevue/core/api";
import { useRouter } from "vue-router";
import { ref } from "vue";
import { storeToRefs } from "pinia";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { useConfirm } from "primevue/useconfirm";
import ConfirmDialog from "primevue/confirmdialog";
import EditButton from "@/components/UI/Buttons/EditButton.vue";
import DeleteButton from "@/components/UI/Buttons/DeleteButton.vue";
import ContactButton from "@/components/UI/Buttons/ContactButton.vue";

// Router & Store
const router = useRouter();
const warehouseStore = useWarehouseStore();
const { flattenedWarehouses, loading } = storeToRefs(warehouseStore);

// Confirm service
const confirm = useConfirm();

// Filters
const filters = ref();

const initFilters = () => {
  filters.value = {
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    warehouseName: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] },
    street: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] },
    city: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] },
    country: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] },
    zipCode: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] }
  };
};

initFilters();

const clearFilter = () => {
  initFilters();
};

// Fetch warehouses
await warehouseStore.fetchIncludeWarehouses();

// Silme işlemi onaylı
const confirmDeleteWarehouse = (warehouseId) => {
  confirm.require({
    message: "Bu depoyu silmek istediğinize emin misiniz?",
    header: "Silme Onayı",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    acceptClass: "p-button-danger",
    rejectClass: "p-button-secondary",
    accept: async () => {
      await warehouseStore.deleteWarehouse(warehouseId);
    },
    reject: () => {
      console.log("Silme işlemi iptal edildi.");
    },
  });
};

// Düzenleme
const updateWarehouse = (warehouseId) => {
  router.push({ name: "update-warehouse", params: { id: warehouseId } });
};
</script>

<style scoped></style>
