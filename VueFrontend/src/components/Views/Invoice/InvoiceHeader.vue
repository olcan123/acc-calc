<template>
  <div
    class="grid grid-cols-1 lg:grid-cols-3 gap-4 mb-6 pb-3 border-b-2 border-gray-300"
  >
    <!-- Şirket bilgileri (Sol) -->
    <InvoiceHeaderLeft />

    <!-- Fatura başlığı (Orta) -->
    <InvoiceHeaderCenter />

    <!-- Banka bilgileri (Sağ) -->
    <InvoiceHeaderRight />
  </div>
</template>

<script setup>
import InvoiceHeaderLeft from "./InvoiceHeaderLeft.vue";
import InvoiceHeaderCenter from "./InvoiceHeaderCenter.vue";
import InvoiceHeaderRight from "./InvoiceHeaderRight.vue";

// Props
defineProps({
  company: {
    type: Object,
    required: true,
  },
  invoice: {
    type: Object,
    required: true,
  },
  banks: {
    type: Array,
    required: true,
  },
});
</script>
