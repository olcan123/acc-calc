<template>
  <thead
    class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400"  >    <tr>
      <!-- Unit Info Section -->
      <th scope="col" class="px-1 py-2 w-1/8 min-w-[100px] text-xs">Barkod</th>
      <th scope="col" class="px-1 py-2 w-1/5 min-w-[150px] text-xs">Ürün</th>
      <th scope="col" class="px-1 py-2 w-40 text-xs">Hesap</th>
      <th scope="col" class="px-1 py-2 w-24 text-xs">Depo</th>
      <th scope="col" class="px-1 py-2 min-w-[60px] text-xs">Miktar</th>
      <th scope="col" class="px-1 py-2 min-w-[60px] text-xs">Birim</th>
      <th scope="col" class="px-1 py-2 min-w-[100px] text-xs">Açıklama</th>
      
      <th
        scope="col"
        class="px-1 py-2 w-24 text-xs border-r-2 border-gray-300 dark:border-gray-600"
      >
        KDV
      </th>      <!-- Price → Amount Flow Section -->
      <th scope="col" class="px-1 py-2 min-w-[70px] text-xs">B.Fiyat</th>
      <!-- //TODO -  <th scope="col" class="px-1 py-2 min-w-[50px] text-xs">İsk%</th> -->
      <th scope="col" class="px-1 py-2 min-w-[70px] text-xs">T.Fiyat</th>
      <th scope="col" class="px-1 py-2 min-w-[70px] text-xs">Tutar</th>
      <!--//TODO -  <th scope="col" class="px-1 py-2 min-w-[60px] text-xs">İsk.Tut</th> -->
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
// This component renders the table header for sale invoice lines
// It's designed to be reusable across different sale views
</script>
