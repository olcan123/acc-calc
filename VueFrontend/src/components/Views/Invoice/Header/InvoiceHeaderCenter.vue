<template>  <div class="text-center">
    <h2 class="text-3xl font-bold text-blue-600 mb-3">FATURË - INVOICE</h2>
    <div class="text-sm">
      <p>
        <span class="font-semibold">Nr. Faturës:</span> {{ sale.invoiceNo || "Nr. i Faturës nuk është futur" }} |
        <span class="font-semibold">Data:</span> {{ formatDate(sale.invoiceDate) }} |
        <span class="font-semibold">Data e Afatit:</span> {{ formatDate(dueDate) }} |
        <span class="font-semibold">Depo:</span> {{ warehouse.name }}
      </p>
    </div>
  </div>
</template>

<script setup>
import { computed } from "vue";
import { formatDate } from "@/utils/invoiceUtils";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { useSaleStore } from "@/stores/sale.store";
import { storeToRefs } from "pinia";

const warehouseStore = useWarehouseStore();
const saleStore = useSaleStore();
const { warehouse } = storeToRefs(warehouseStore);
const { sale } = storeToRefs(saleStore);

// Computed property to calculate due date (invoice date + 30 days)
const dueDate = computed(() => {
  if (!sale.value?.invoiceDate) return null;
  
  const invoiceDate = new Date(sale.value.invoiceDate);
  const dueDate = new Date(invoiceDate);
  dueDate.setDate(dueDate.getDate() + 30);
  
  return dueDate.toISOString().split('T')[0];
});
</script>
