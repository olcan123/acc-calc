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
  <DataTable :value="filteredProducts" responsiveLayout="scroll" dataKey="id">
    <Column header="ID">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>
    <Column field="name" header="Ürün Adı" />
    <Column field="description" header="Açıklama">
      <template #body="slotProps">
        {{ slotProps.data.description || '-' }}
      </template>
    </Column>
    <Column field="unitOfMeasure.name" header="Birim">
      <template #body="slotProps">
        {{ slotProps.data.unitOfMeasure?.name || '-' }}
      </template>
    </Column>
    <Column field="vat.rate" header="KDV Oranı">
      <template #body="slotProps">
        {{ slotProps.data.vat?.rate ? `%${slotProps.data.vat.rate}` : '-' }}
      </template>
    </Column>

    <!-- İşlemler -->
    <Column header="İşlemler" style="width: 180px">
      <template #body="slotProps">
        <div class="flex gap-2">
          <!-- Görüntüle Butonu -->
          <button
            @click="viewProduct(slotProps.data.id)"
            type="button"
            class="text-white bg-blue-600 hover:bg-blue-700 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:focus:ring-blue-900"
          >
            Görüntüle
          </button>
          
          <!-- Düzenle Butonu -->
          <button
            @click="updateProduct(slotProps.data.id)"
            type="button"
            class="text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:focus:ring-yellow-900"
          >
            Düzenle
          </button>

          <!-- Sil Butonu -->
          <button
            @click="confirmDeleteProduct(slotProps.data.id)"
            type="button"
            class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900"
          >
            Sil
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
import { ref, computed, onMounted } from 'vue';
import { useRouter } from "vue-router";
import { storeToRefs } from "pinia";
import { useProductStore } from '@/stores/product.store';
import { useConfirm } from "primevue/useconfirm";

// Router & Store
const router = useRouter();
const productStore = useProductStore();
const { products } = storeToRefs(productStore);
const { fetchProducts, deleteProduct } = productStore;

// PrimeVue Confirm service
const confirm = useConfirm();

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
