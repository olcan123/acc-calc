<template>
  <div class="grid grid-cols-1 lg:grid-cols-3 gap-4 mb-6 pb-3 border-b-2 border-gray-300">
    <!-- Şirket bilgileri (Sol) -->
    <div>
      <h1 class="text-xl font-bold text-gray-800 mb-2">{{ company.name }}</h1>
      <div class="text-xs text-gray-600 space-y-1">
        <p>{{ company.address }}</p>
        <p>{{ company.city }} {{ company.postalCode }}</p>
        <p>Tel: {{ company.phone }}</p>
        <p>Email: {{ company.email }}</p>
        <p>Web: {{ company.website }}</p>
        <p>Vergi No: {{ company.taxNumber }}</p>
      </div>
    </div>

    <!-- Fatura başlığı (Orta) -->
    <div class="text-center">
      <h2 class="text-2xl font-bold text-blue-600 mb-3">FATURA - INVOICE</h2>
      <div class="text-xs space-y-1">
        <p><span class="font-semibold">Fatura No:</span> {{ invoice.invoiceNumber }}</p>
        <p><span class="font-semibold">Tarih:</span> {{ formatDate(invoice.invoiceDate) }}</p>
        <p><span class="font-semibold">Vade Tarihi:</span> {{ formatDate(invoice.dueDate) }}</p>
      </div>
    </div>

    <!-- Banka bilgileri (Sağ) -->
    <div>
      <h3 class="text-sm font-semibold text-gray-800 mb-2 border-b border-gray-200 pb-1">
        Banka Bilgileri
      </h3>
      <div class="space-y-2">
        <div v-for="bank in banks" :key="bank.id" class="text-xs">
          <p class="font-semibold">{{ bank.bankName }}</p>
          <p>{{ bank.branchName }}</p>
          <p>IBAN: {{ bank.iban }}</p>
          <p>Hesap No: {{ bank.accountNumber }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { formatDate } from '@/utils/invoiceUtils'

// Props
defineProps({
  company: {
    type: Object,
    required: true
  },
  invoice: {
    type: Object,
    required: true
  },
  banks: {
    type: Array,
    required: true
  }
})
</script>