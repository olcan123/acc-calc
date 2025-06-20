<template>
    <!-- Header Section -->
    <div class="flex justify-between items-center mb-4">
        <h2 class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight">
            Kategori Tablosu
        </h2>

        <button @click="router.push({ name: 'create-category' })" type="button"
            class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
            Yeni Kategori Oluştur
        </button>
    </div>    <!-- DataTable -->
    <DataTable 
      :value="categories" 
      v-model:filters="filters"
      responsiveLayout="scroll" 
      dataKey="id"
      paginator
      :rows="10"
      filterDisplay="menu"
      sortMode="multiple"
      :globalFilterFields="['name']"
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
            <h3 class="mt-2 text-sm font-medium text-gray-900 dark:text-gray-100">Kategori bulunamadı</h3>
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
            <p class="mt-2 text-sm">Kategoriler yükleniyor. Lütfen bekleyin...</p>
          </div>
        </div>
      </template>

        <Column header="ID" style="width: 60px">
            <template #body="{ index }">
                {{ index + 1 }}
            </template>
        </Column>
        <Column field="name" header="Kategori Adı" sortable style="min-width: 200px">
          <template #filter="{ filterModel }">
            <input
              v-model="filterModel.value"
              type="text"
              placeholder="Kategori adı ara..."
              class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
            />
          </template>
        </Column>        <!-- İşlemler -->
        <Column header="İşlemler" style="min-width: 150px">
            <template #body="{ data }">
                <div class="flex gap-2">
                    <EditButton 
                        @click="updateCategory(data.id)" 
                        title="Düzenle" 
                    />
                    <DeleteButton 
                        @click="confirmDeleteCategory(data.id)" 
                        title="Sil" 
                    />
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
import { useCategoryStore } from "@/stores/category.store";
import { useConfirm } from "primevue/useconfirm";
import EditButton from "@/components/UI/Buttons/EditButton.vue";
import DeleteButton from "@/components/UI/Buttons/DeleteButton.vue";

// Router & Store
const router = useRouter();
const categoryStore = useCategoryStore();
const { categories, loading } = storeToRefs(categoryStore);

// PrimeVue Confirm service
const confirm = useConfirm();

// Filters
const filters = ref();

const initFilters = () => {
    filters.value = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
        name: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] }
    };
};

initFilters();

const clearFilter = () => {
    initFilters();
};

// Fetch categories
await categoryStore.fetchCategories();

// Silme işlemi onaylı
const confirmDeleteCategory = (categoryId) => {
    confirm.require({
        message: "Bu kategoriyi silmek istediğinize emin misiniz?",
        header: "Silme Onayı",
        acceptLabel: "Evet",
        rejectLabel: "Hayır",
        acceptClass: "p-button-danger",
        rejectClass: "p-button-secondary",
        accept: async () => {
            await categoryStore.deleteCategory(categoryId);
        },
        reject: () => {
            console.log("Silme işlemi iptal edildi.");
        },
    });
};

// Düzenleme
const updateCategory = (id) => {
    router.push({ name: "update-category", params: { id } });
};
</script>
