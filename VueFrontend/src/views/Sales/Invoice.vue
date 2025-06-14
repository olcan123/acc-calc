<template>
  <!-- Print buttons -->
  <InvoicePrintButtons @print="handlePrint" @goBack="handleGoBack" />

  <!-- Ana fatura konteyneri -->
  <div class="max-w-4xl mx-auto p-6 bg-white shadow-lg print:shadow-none print:max-w-none print:p-0">
    <!-- Header -->
    <InvoiceHeader 
      :company="company" 
      :invoice="invoice" 
      :banks="banks" 
    />
    
    <!-- Customer Info -->
    <InvoiceCustomerInfo :client="client" />
    
    <!-- Items Table -->
    <InvoiceItemsTable :items="items" />
    
    <!-- Summary -->
    <InvoiceSummary :summary="summary" />
    
    <!-- Notes -->
    <InvoiceNotes :notes="notes" />

    <!-- Footer -->
    <InvoiceFooter />
  </div>
</template>

<script setup>
import { computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useInvoiceStore } from '@/stores/invoice.store'
import {
  InvoiceHeader,
  InvoiceCustomerInfo,
  InvoiceItemsTable,
  InvoiceSummary,
  InvoiceNotes,
  InvoiceFooter,
  InvoicePrintButtons
} from '@/components/Views/Invoice'

const router = useRouter()
const invoiceStore = useInvoiceStore()

// Get data from store
const company = computed(() => invoiceStore.companyInfo)
const client = computed(() => invoiceStore.customerInfo)
const invoice = computed(() => invoiceStore.invoiceData)
const banks = computed(() => invoiceStore.companyBankInfo)
const items = computed(() => invoiceStore.invoiceLines)
const notes = computed(() => invoiceStore.invoiceData.notes)
const summary = computed(() => invoiceStore.summary)

// Event handlers
const handlePrint = () => {
  // Additional print logic can be added here if needed
  console.log('Printing invoice...')
}

const handleGoBack = () => {
  // Additional navigation logic can be added here if needed
  console.log('Navigating back...')
}

// Load invoice data on mount
onMounted(async () => {
  const invoiceId = router.currentRoute.value.params.id
  if (invoiceId) {
    // Load invoice data from API via store
    await invoiceStore.loadInvoice(invoiceId)
  }
})
</script>

<style scoped>
/* Yazdırma stilleri */
@media print {
  body {
    font-family: Arial, sans-serif;
    font-size: 11px;
  }
  
  .print\:hidden {
    display: none !important;
  }
  
  .print\:shadow-none {
    box-shadow: none !important;
  }
  
  .print\:max-w-none {
    max-width: none !important;
  }
  
  .print\:p-0 {
    padding: 0 !important;
  }
  
  /* Yazdırma modunda grid yapısını koru */
  .grid {
    display: grid !important;
  }
  
  .lg\:grid-cols-3 {
    grid-template-columns: repeat(3, minmax(0, 1fr)) !important;
  }
  
  /* Yazdırma modunda font boyutlarını optimize et */
  h1 {
    font-size: 16px !important;
  }
  
  h2 {
    font-size: 18px !important;
  }
  
  h3 {
    font-size: 12px !important;
  }
  
  .text-xs {
    font-size: 9px !important;
  }
  
  /* Yazdırma modunda spacing'i azalt */
  .gap-4 {
    gap: 8px !important;
  }
  
  .mb-6 {
    margin-bottom: 12px !important;
  }
  
  .p-4 {
    padding: 8px !important;
  }
  
  .px-2 {
    padding-left: 4px !important;
    padding-right: 4px !important;
  }
  
  .py-1 {
    padding-top: 2px !important;
    padding-bottom: 2px !important;
  }
  
  @page {
    size: A4;
    margin: 1cm;
  }
}
</style>