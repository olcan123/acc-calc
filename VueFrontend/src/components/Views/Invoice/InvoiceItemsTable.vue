<template>
  <div class="mb-6">
    <table class="w-full border-collapse border border-gray-300">
      <thead>
        <tr class="bg-gray-100">
          <th class="border border-gray-300 px-2 py-1 text-left text-xs font-semibold">Açıklama</th>
          <th class="border border-gray-300 px-2 py-1 text-center text-xs font-semibold">Miktar</th>
          <th class="border border-gray-300 px-2 py-1 text-center text-xs font-semibold">Birim</th>
          <th class="border border-gray-300 px-2 py-1 text-right text-xs font-semibold">Birim Fiyat</th>
          <th class="border border-gray-300 px-2 py-1 text-right text-xs font-semibold">İndirim</th>
          <th class="border border-gray-300 px-2 py-1 text-right text-xs font-semibold">Tutar</th>
          <th class="border border-gray-300 px-2 py-1 text-right text-xs font-semibold">KDV</th>
          <th class="border border-gray-300 px-2 py-1 text-right text-xs font-semibold">Toplam</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, index) in items" :key="index" class="hover:bg-gray-50">
          <td class="border border-gray-300 px-2 py-1 text-xs">{{ item.productName }}</td>
          <td class="border border-gray-300 px-2 py-1 text-center text-xs">{{ formatNumber(item.quantity) }}</td>
          <td class="border border-gray-300 px-2 py-1 text-center text-xs">{{ item.unitName }}</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs">{{ formatCurrency(item.unitPrice) }}</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs">{{ item.discountRate }}%</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs">{{ formatCurrency(item.amount) }}</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs">{{ formatCurrency(item.vatTaxAmount) }}</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs font-semibold">{{ formatCurrency(item.totalAmount) }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { formatCurrency, formatNumber } from '@/utils/invoiceUtils'

// Props
defineProps({
  items: {
    type: Array,
    required: true
  }
})
</script>