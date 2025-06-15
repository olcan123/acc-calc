<template>
  <div class="mb-6 pb-3 border-b-2 border-gray-300">
    <!-- Fatura başlığı (Üstte) -->
    <div class="text-center mb-4">
      <InvoiceHeaderCenter />
    </div>
    
    <!-- Şirket ve Banka bilgileri (Yan yana) -->
    <InvoiceHeaderSideSection />
  </div>
</template>

<script setup>
import InvoiceHeaderCenter from "./InvoiceHeaderCenter.vue";
import InvoiceHeaderSideSection from "./InvoiceHeaderSideSection.vue";
</script>
