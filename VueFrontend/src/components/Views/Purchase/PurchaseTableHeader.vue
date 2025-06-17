<template>
  <thead
    class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400"
    style="font-size: 10px"
  >
    <tr>
      <!-- Unit Info Section -->
      <th scope="col" class="px-1 py-1 w-1/5 min-w-[120px]">Barkod</th>
      <th scope="col" class="px-1 py-1 w-1/5 min-w-[200px]">Ürün</th>
      <th scope="col" class="px-1 py-1 w-40">Hesap</th>
      <th scope="col" class="px-1 py-1 w-24">Depo</th>
      <th scope="col" class="px-1 py-1 min-w-[60px]">Miktar</th>
      <th scope="col" class="px-1 py-1 min-w-[60px]">Birim</th>

      <!-- Import-only tax columns -->
      <th
        v-if="isImportPurchase"
        scope="col"
        class="px-1 py-2 min-w-[50px] text-xs bg-orange-50 dark:bg-orange-900"
      >
        ÖTV%
      </th>
      <th
        v-if="isImportPurchase"
        scope="col"
        class="px-1 py-2 min-w-[55px] text-xs bg-purple-50 dark:bg-purple-900"
      >
        Gümrük%
      </th>
      <th
        scope="col"
        class="px-1 py-2 w-24 text-xs border-r-2 border-gray-300 dark:border-gray-600"
      >
        KDV
      </th>

      <!-- Price → Amount Flow Section -->
      <th scope="col" class="px-1 py-2 min-w-[70px] text-xs">B.Fiyat</th>
      <th scope="col" class="px-1 py-2 min-w-[50px] text-xs">İsk%</th>
      <th scope="col" class="px-1 py-2 min-w-[70px] text-xs">M.Fiyat</th>
      <th scope="col" class="px-1 py-2 min-w-[70px] text-xs">T.Fiyat</th>
      <th scope="col" class="px-1 py-2 min-w-[70px] text-xs">Tutar</th>
      <th scope="col" class="px-1 py-2 min-w-[60px] text-xs">İsk.Tut</th>
      <th
        scope="col"
        class="px-1 py-2 min-w-[60px] text-xs bg-blue-50 dark:bg-blue-900"
      >
        Masraf
      </th>

      <!-- Import-only tax amount columns -->
      <th
        v-if="isImportPurchase"
        scope="col"
        class="px-1 py-2 min-w-[60px] text-xs bg-orange-50 dark:bg-orange-900"
      >
        ÖTV
      </th>
      <th
        v-if="isImportPurchase"
        scope="col"
        class="px-1 py-2 min-w-[60px] text-xs bg-purple-50 dark:bg-purple-900"
      >
        Gümrük
      </th>

      <th scope="col" class="px-1 py-2 min-w-[70px] text-xs">M.Tutar</th>
      <th scope="col" class="px-1 py-2 min-w-[60px] text-xs">KDV Tut</th>
      <th
        scope="col"
        class="px-1 py-2 min-w-[80px] text-xs border-r-2 border-gray-300 dark:border-gray-600"
      >
        Toplam
      </th>

      <!-- Actions Section -->
      <th scope="col" class="px-1 py-2 min-w-[50px] text-xs">İşlem</th>
    </tr>
  </thead>
</template>

<script setup>
import { computed } from "vue";
import { useRoute } from "vue-router";

// Check if this is an import purchase using route
const route = useRoute();
const isImportPurchase = computed(() => {
  return route.path.includes("/import");
});

// This component renders the table header for purchase invoice lines
// It's designed to be reusable across different purchase views
</script>
