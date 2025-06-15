<template>  <div class="mb-6 invoice-items-container">
    <table class="w-full border-collapse border border-gray-300 print:table-fixed invoice-items-table"><thead>        <tr class="bg-gray-100">
          <th class="border border-gray-300 px-2 py-1 text-left text-sm font-semibold description-column" style="width: 250px; min-width: 300px; max-width: 300px;">Përshkrimi</th>
          <th class="border border-gray-300 px-2 py-1 text-center text-sm font-semibold">Sasia</th>
          <th class="border border-gray-300 px-2 py-1 text-center text-sm font-semibold">Njësia</th>
          <th class="border border-gray-300 px-2 py-1 text-right text-sm font-semibold">%</th>
          <th class="border border-gray-300 px-2 py-1 text-right text-sm font-semibold">Çmimi për Njësi</th>
          <th class="border border-gray-300 px-2 py-1 text-right text-sm font-semibold">Çmimi me TVSH</th>
          <th class="border border-gray-300 px-2 py-1 text-right text-sm font-semibold">Shuma</th>
          <th class="border border-gray-300 px-2 py-1 text-right text-sm font-semibold">TVSH</th>
          <th class="border border-gray-300 px-2 py-1 text-right text-sm font-semibold">Totali</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(item, index) in sale.saleInvoiceLines" :key="index" class="hover:bg-gray-50 print:page-break-inside-avoid">          <td class="border border-gray-300 px-2 py-1 text-sm break-words description-column" style="width: 300px; min-width: 300px; max-width: 300px; word-wrap: break-word; white-space: normal; vertical-align: top;">
            {{ getProductName(item.productId) }} {{ item.description }}
          </td>
          <td class="border border-gray-300 px-2 py-1 text-center text-xs">{{ formatNumber(item.quantity) }}</td>
           <td class="border border-gray-300 px-2 py-1 text-center text-xs">{{getUnitName(item.unitOfMeasureId) }}</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs">{{ getVatRate(item.vatId) }}%</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs">{{ formatCurrency(item.unitPrice) }}</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs">{{ formatCurrency(item.totalPrice) }}</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs">{{ formatCurrency(item.amount) }}</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs">{{ formatCurrency(item.vatTaxAmount) }}</td>
          <td class="border border-gray-300 px-2 py-1 text-right text-xs font-semibold">{{ formatCurrency(item.totalAmount) }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { useSaleStore } from '@/stores/sale.store'
import { useProductStore } from '@/stores/product.store'
import { useVatStore } from '@/stores/vat.store'
import { useUnitOfMeasureStore } from '@/stores/unit-of-measure.store'
import { storeToRefs } from 'pinia'
import { formatCurrency, formatNumber } from '@/utils/invoiceUtils'

// Store
const saleStore = useSaleStore()
const productStore = useProductStore()
const vatStore = useVatStore()
const unitOfMeasureStore = useUnitOfMeasureStore()

// Reactive references
const { sale } = storeToRefs(saleStore)
const { products } = storeToRefs(productStore)
const { vats } = storeToRefs(vatStore)
const { unitOfMeasures } = storeToRefs(unitOfMeasureStore)

const getProductName = (productId) => {
  const product = products.value.find(p => p.id === productId)
  return product ? `${product.barcodes[0].code} ${product.name}` : 'Bilinmiyor'
}

const getVatRate = (vatId) => {
  const vat = vats.value.find(v => v.id === vatId)
  return vat ? vat.rate : 0
}

const getUnitName = (unitId) => {
  const unit = unitOfMeasures.value.find(u => u.id === unitId)
  return unit ? unit.name : 'Bilinmiyor'
}


</script>

<style scoped>
@media print {
  .mb-6 {
    margin-bottom: 8px !important;
    page-break-inside: auto !important;
    break-inside: auto !important;
  }
  
  /* Make VAT % column (4th column) smaller in print mode */
  .invoice-items-table th:nth-child(4),
  .invoice-items-table td:nth-child(4) {
    width: 5% !important;
    min-width: 5% !important;
    max-width: 5% !important;
  }
  
  table {
    page-break-inside: auto !important;
    break-inside: auto !important;
    table-layout: fixed !important;
  }
  
  thead {
    display: table-header-group !important;
  }
  
  tbody {
    display: table-row-group !important;
  }
  
  tr {
    page-break-inside: avoid !important;
    break-inside: avoid !important;
  }
}
</style>