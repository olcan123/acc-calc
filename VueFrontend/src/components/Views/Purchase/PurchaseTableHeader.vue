<template>
  <thead
    class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400"
  >
    <tr>
      <!-- Unit Info Section -->
      <th scope="col" class="px-3 py-3 min-w-[200px]">Ürün</th>
      <th scope="col" class="px-3 py-3 min-w-[200px]">Hesap No</th>
      <th scope="col" class="px-3 py-3 min-w-[120px]">Depo</th>
      <th scope="col" class="px-3 py-3 min-w-[80px]">Miktar</th>
      <th scope="col" class="px-3 py-3 min-w-[100px]">Birim</th>
      
      <!-- Import-only tax columns -->
      <th 
        v-if="isImportPurchase"
        scope="col" 
        class="px-3 py-3 min-w-[80px] bg-orange-50 dark:bg-orange-900"
      >
        ÖTV %
      </th>
      <th 
        v-if="isImportPurchase"
        scope="col" 
        class="px-3 py-3 min-w-[80px] bg-purple-50 dark:bg-purple-900"
      >
        Gümrük %
      </th>
      
      <th
        scope="col"
        class="px-3 py-3 min-w-[80px] border-r-2 border-gray-300 dark:border-gray-600"
      >
        KDV
      </th>

      <!-- Price → Amount Flow Section -->
      <th scope="col" class="px-3 py-3 min-w-[100px]">Birim Fiyat</th>
      <th scope="col" class="px-3 py-3 min-w-[80px]">İskonto %</th>
      <th scope="col" class="px-3 py-3 min-w-[100px]">Maliyet Fiyatı</th>
      <th scope="col" class="px-3 py-3 min-w-[100px]">Toplam Fiyat</th>
      <th scope="col" class="px-3 py-3 min-w-[100px]">Tutar</th>
      <th scope="col" class="px-3 py-3 min-w-[100px]">İskonto Tutarı</th>
      <th
        scope="col"
        class="px-3 py-3 min-w-[100px] bg-blue-50 dark:bg-blue-900"
      >
        Masraf Tutarı
      </th>
      
      <!-- Import-only tax amount columns -->
      <th 
        v-if="isImportPurchase"
        scope="col" 
        class="px-3 py-3 min-w-[100px] bg-orange-50 dark:bg-orange-900"
      >
        ÖTV Tutarı
      </th>
      <th 
        v-if="isImportPurchase"
        scope="col" 
        class="px-3 py-3 min-w-[100px] bg-purple-50 dark:bg-purple-900"
      >
        Gümrük Tutarı
      </th>
      
      <th scope="col" class="px-3 py-3 min-w-[100px]">Maliyet Tutarı</th>
      <th scope="col" class="px-3 py-3 min-w-[100px]">KDV Tutarı</th>
      <th
        scope="col"
        class="px-3 py-3 min-w-[120px] border-r-2 border-gray-300 dark:border-gray-600"
      >
        Toplam Tutar
      </th>

      <!-- Actions Section -->
      <th scope="col" class="px-3 py-3 min-w-[80px]">İşlem</th>
    </tr>
  </thead>
</template>

<script setup>
import { computed } from 'vue';
import { useRoute } from 'vue-router';

// Check if this is an import purchase using route
const route = useRoute();
const isImportPurchase = computed(() => {
  return route.path.includes("/import");
});

// This component renders the table header for purchase invoice lines
// It's designed to be reusable across different purchase views
</script>
