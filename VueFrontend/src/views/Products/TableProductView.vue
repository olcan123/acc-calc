<template>
  <!-- Header Section -->
  <div class="flex justify-between items-center mb-4">
    <!-- Yazıyı büyüt + y ekseninde hizala -->
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Ürün Tablosu
    </h2>

    <!-- Buton boyutunu koru, ama içerik dikey ortalansın -->
    <button
      @click="router.push({ name: 'create-product' })"
      type="button"
      class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
    >
      Yeni Ürün Oluştur
    </button>
  </div>
  <!-- DataTable -->
  <DataTable 
    :value="filteredProducts" 
    v-model:filters="filters"
    responsiveLayout="scroll" 
    dataKey="id"
    paginator
    :rows="10"
    filterDisplay="menu"
    sortMode="multiple"
    :globalFilterFields="['name', 'description', 'unitOfMeasure.name']"
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
          <h3 class="mt-2 text-sm font-medium text-gray-900 dark:text-gray-100">Ürün bulunamadı</h3>
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
          <p class="mt-2 text-sm">Ürünler yükleniyor. Lütfen bekleyin...</p>
        </div>
      </div>
    </template>    <Column header="ID" style="width: 60px">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>
    <Column field="name" header="Ürün Adı" sortable style="min-width: 200px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Ürün adı ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="description" header="Açıklama" sortable style="min-width: 200px">
      <template #body="slotProps">
        {{ slotProps.data.description || '-' }}
      </template>
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Açıklama ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="unitOfMeasure.name" header="Birim" sortable style="min-width: 120px">
      <template #body="slotProps">
        {{ slotProps.data.unitOfMeasure?.name || '-' }}
      </template>
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Birim ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="vat.rate" header="KDV Oranı" sortable style="min-width: 120px" dataType="numeric">
      <template #body="slotProps">
        {{ slotProps.data.vat?.rate ? `%${slotProps.data.vat.rate}` : '-' }}
      </template>
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="number"
          step="0.01"
          min="0"
          placeholder="Min KDV oranı..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>    <!-- İşlemler -->
    <Column header="İşlemler" style="min-width: 180px">
      <template #body="{ data }">
        <div class="flex gap-2">
          <!-- View Button -->
          <button
            @click="viewProduct(data.id)"
            class="p-2 text-gray-600 hover:text-gray-800 hover:bg-gray-100 dark:hover:bg-gray-900 rounded-lg transition-colors"
            title="Görüntüle"
          >
            <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
              <path d="M10 12a2 2 0 100-4 2 2 0 000 4z"/>
              <path fill-rule="evenodd" d="M.458 10C1.732 5.943 5.522 3 10 3s8.268 2.943 9.542 7c-1.274 4.057-5.064 7-9.542 7S1.732 14.057.458 10zM14 10a4 4 0 11-8 0 4 4 0 018 0z" clip-rule="evenodd"/>
            </svg>
          </button>
          
          <!-- Edit Button -->
          <button
            @click="updateProduct(data.id)"
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
            @click="confirmDeleteProduct(data.id)"
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
import { ref, computed, onMounted } from 'vue';
import { FilterMatchMode, FilterOperator } from "@primevue/core";
import { DataTable, Column, ConfirmDialog } from "primevue";
import { useRouter } from "vue-router";
import { storeToRefs } from "pinia";
import { useProductStore } from '@/stores/product.store';
import { useConfirm } from "primevue/useconfirm";

// Router & Store
const router = useRouter();
const productStore = useProductStore();
const { products, loading } = storeToRefs(productStore);
const { fetchProducts, deleteProduct } = productStore;

// PrimeVue Confirm service
const confirm = useConfirm();

// Filter state initialization
const initFilters = () => {
  filters.value = {
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    name: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    description: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    'unitOfMeasure.name': { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    'vat.rate': { 
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

const searchTerm = ref('');

const filteredProducts = computed(() => {
  if (!searchTerm.value) return products.value;
  
  const searchLower = searchTerm.value.toLowerCase();
  return products.value.filter(product => 
    product.name?.toLowerCase().includes(searchLower) || 
    product.description?.toLowerCase().includes(searchLower)
  );
});

// Fetch products
onMounted(async () => {
  await fetchProducts();
});

// Silme işlemi onaylı
const confirmDeleteProduct = (productId) => {
  confirm.require({
    message: "Bu ürünü silmek istediğinize emin misiniz?",
    header: "Silme Onayı",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    acceptClass: "p-button-danger",
    rejectClass: "p-button-secondary",
    accept: async () => {
      await deleteProduct(productId);
    },
    reject: () => {
      console.log("Silme işlemi iptal edildi.");
    },
  });
};

// Düzenleme
const updateProduct = (id) => {
  router.push({ name: "update-product", params: { id } });
};

// Görüntüleme
const viewProduct = (id) => {
  router.push({ name: "detail-product", params: { id } });
};
</script>
