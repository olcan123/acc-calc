<template>
  <div class="flex justify-end mb-6">
    <div class="w-64">
      <table class="w-full border-collapse border border-gray-300">
        <tr>
          <td class="border border-gray-300 px-2 py-1 text-xs font-semibold bg-gray-100">Ara Toplam:</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs">{{ formatCurrency(summary.subtotal) }}</td>
        </tr>
        <tr>
          <td class="border border-gray-300 px-2 py-1 text-xs font-semibold bg-gray-100">Toplam İndirim:</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs">{{ formatCurrency(summary.totalDiscount) }}</td>
        </tr>
        <tr>
          <td class="border border-gray-300 px-2 py-1 text-xs font-semibold bg-gray-100">KDV Matrahı:</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs">{{ formatCurrency(summary.taxBase) }}</td>
        </tr>
        <tr>
          <td class="border border-gray-300 px-2 py-1 text-xs font-semibold bg-gray-100">Toplam KDV:</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs">{{ formatCurrency(summary.totalVat) }}</td>
        </tr>
        <tr class="bg-blue-50">
          <td class="border border-gray-300 px-2 py-1 text-xs font-bold">GENEL TOPLAM:</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs font-bold text-blue-600">{{ formatCurrency(summary.grandTotal) }}</td>
        </tr>
      </table>
    </div>
  </div>
</template>

<script setup>
import { formatCurrency } from '@/utils/invoiceUtils'

// Props
defineProps({
  summary: {
    type: Object,
    required: true
  }
})
</script>