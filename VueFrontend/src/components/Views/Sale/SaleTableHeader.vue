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
      <th scope="col" class="px-3 py-3 min-w-[150px]">Açıklama</th>
      
      <th
        scope="col"
        class="px-3 py-3 min-w-[80px] border-r-2 border-gray-300 dark:border-gray-600"
      >
        KDV
      </th>

      <!-- Price → Amount Flow Section -->
      <th scope="col" class="px-3 py-3 min-w-[100px]">Birim Fiyat</th>
      <th scope="col" class="px-3 py-3 min-w-[80px]">İskonto %</th>
      <th scope="col" class="px-3 py-3 min-w-[100px]">Toplam Fiyat</th>
      <th scope="col" class="px-3 py-3 min-w-[100px]">Tutar</th>
      <th scope="col" class="px-3 py-3 min-w-[100px]">İskonto Tutarı</th>
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
// This component renders the table header for sale invoice lines
// It's designed to be reusable across different sale views
</script>
