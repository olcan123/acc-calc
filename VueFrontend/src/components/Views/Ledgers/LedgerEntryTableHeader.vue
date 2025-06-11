<template>
  <thead
    class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400"
  >
    <tr>
      <!-- Sıra No -->
      <th scope="col" class="px-3 py-3 min-w-[60px]">
        Sıra
      </th>
      
      <!-- Hesap -->
      <th scope="col" class="px-3 py-3 min-w-[200px]">
        Hesap
      </th>
      
      <!-- Partner -->
      <th scope="col" class="px-3 py-3 min-w-[150px]">
        Partner
      </th>
      
      <!-- Açıklama -->
      <th scope="col" class="px-3 py-3 min-w-[200px]">
        Açıklama
      </th>
      
      <!-- Borç -->
      <th scope="col" class="px-3 py-3 min-w-[120px] bg-red-50 dark:bg-red-900">
        <div class="flex items-center gap-1">
          <span class="text-red-600 dark:text-red-400">📈</span>
          Borç
        </div>
      </th>
      
      <!-- Alacak -->
      <th scope="col" class="px-3 py-3 min-w-[120px] bg-green-50 dark:bg-green-900">
        <div class="flex items-center gap-1">
          <span class="text-green-600 dark:text-green-400">📉</span>
          Alacak
        </div>
      </th>
      
      <!-- Actions -->
      <th scope="col" class="px-3 py-3 min-w-[80px]">
        İşlemler
      </th>
    </tr>
  </thead>
</template>

<script setup>
// Bu component sadece tablo header'ı için, logic gerektirmiyor
</script>
