<template>
  <div class="flex justify-end mb-6">
    <div class="w-64">
      <table class="w-full border-collapse border border-gray-300">
        <tbody>          <tr>
            <td
              class="border border-gray-300 px-2 py-1 text-sm font-semibold bg-gray-100"
            >
              Nëntotali:
            </td>
            <td class="border border-gray-300 px-2 py-1 text-right text-sm">
              {{ formatCurrency(grandAmount) }}
            </td>
          </tr>
          <tr>
            <td
              class="border border-gray-300 px-2 py-1 text-sm font-semibold bg-gray-100"
            >
              Totali TVSH:
            </td>
            <td class="border border-gray-300 px-2 py-1 text-right text-sm">
              {{ formatCurrency(grandVatAmount) }}
            </td>
          </tr>
          <tr class="bg-blue-50">
            <td class="border border-gray-300 px-2 py-1 text-sm font-bold">
              TOTALI I PËRGJITHSHËM:
            </td>
            <td
              class="border border-gray-300 px-2 py-1 text-right text-sm font-bold text-blue-600"
            >
              {{ formatCurrency(grandTotal) }}
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { useSaleStore } from "@/stores/sale.store";
import { storeToRefs } from "pinia";
import { formatCurrency } from "@/utils/invoiceUtils";

// Store
const saleStore = useSaleStore();

const { sale } = storeToRefs(saleStore);

// Yeni ve daha verimli kod:
const { grandAmount, grandVatAmount, grandTotal } =
  sale.value.saleInvoiceLines.reduce(
    (acc, line) => {
      acc.grandAmount += line.amount || 0; // Eğer amount tanımsız ise 0 ekler
      acc.grandVatAmount += line.vatTaxAmount || 0; // Eğer vatTaxAmount tanımsız ise 0 ekler
      acc.grandTotal += line.totalAmount || 0; // Eğer totalAmount tanımsız ise 0 ekler
      return acc;
    },
    { grandAmount: 0, grandVatAmount: 0, grandTotal: 0 }
  );


</script>
