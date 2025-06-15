<template>
  <!-- Print buttons -->
  <InvoicePrintButtons @print="handlePrint" @goBack="handleGoBack" />

  <!-- Ana fatura konteyneri -->
  <div
    class="max-w-4xl mx-auto p-6 bg-white shadow-lg print:shadow-none print:max-w-none print:p-0"
  >    <!-- Invoice content wrapper -->
    <div class="invoice-content">
      <!-- Header -->
      <InvoiceHeader />

      <!-- Customer Info -->
      <InvoiceCustomerInfo />

      <!-- Items Table -->
      <InvoiceItemsTable />

      <!-- Summary -->
      <InvoiceSummary />
      
      <!-- Signatures - Son sayfada görünür -->
      <InvoiceSignatures />
    </div>

    <!-- Footer - Static at bottom -->
    <!-- <div class="invoice-footer">
      <InvoiceFooter />
    </div> -->
  </div>
</template>

<script setup>
import { storeToRefs } from "pinia";
import { useRouter } from "vue-router";
import { useCompanyStore } from "@/stores/company.store";
import { useSaleStore } from "@/stores/sale.store";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { useBankStore } from "@/stores/bank.store";
import { useBankAccountCompanyStore } from "@/stores/bank-account-company.store";
import { usePartnerStore } from "@/stores/partner.store";
import { useProductStore } from "@/stores/product.store";
import { useUnitOfMeasureStore } from "@/stores/unit-of-measure.store";
import { useVatStore } from "@/stores/vat.store";
import {
  InvoiceHeader,
  InvoiceCustomerInfo,
  InvoiceItemsTable,
  InvoiceSummary,
  InvoiceFooter,
  InvoicePrintButtons,
  InvoiceSignatures,
} from "@/components/Views/Invoice";

const router = useRouter();
const saleStore = useSaleStore();
const companyStore = useCompanyStore();
const warehouseStore = useWarehouseStore();
const bankStore = useBankStore();
const bankAccountCompanyStore = useBankAccountCompanyStore();
const partnerStore = usePartnerStore();
const productStore = useProductStore();
const unitOfMeasureStore = useUnitOfMeasureStore();
const vatStore = useVatStore();
// Get reactive references from stores

const { sale } = storeToRefs(saleStore);
const { companies } = storeToRefs(companyStore);

// Event handlers
let isPrinting = false;

const handlePrint = () => {
  // Prevent multiple print calls
  if (isPrinting) return;

  isPrinting = true;

  // Hide document title during print
  const originalTitle = document.title;
  document.title = "";

  // Trigger print
  window.print();

  // Restore title and reset guard
  setTimeout(() => {
    document.title = originalTitle;
    isPrinting = false;
  }, 2000);
};

const handleGoBack = () => {
  // Navigate back functionality handled by the button component
};

const invoiceId = router.currentRoute.value.params.id;
await saleStore.fetchSale(invoiceId);
await companyStore.fetchCompanies();
await bankStore.fetchBanks();
await warehouseStore.fetchIncludeWarehouse(
  sale.value.saleInvoiceLines[0].warehouseId
);
await partnerStore.getPartnerDetails(sale.value.partnerId);
await bankAccountCompanyStore.fetchBankAccounts(companies.value[0].id);
await productStore.fetchProducts();
await unitOfMeasureStore.fetchUnitOfMeasures();
await vatStore.fetchVats();
</script>

<style scoped>
/* Normal ekran görünümü */
.invoice-content {
  min-height: 500px;
}

.invoice-footer {
  margin-top: 40px;
  border-top: 2px solid #e5e7eb;
  padding-top: 20px;
}

/* Yazdırma stilleri */
@media print {  @page {
    size: A4;
    margin: 0.15cm; /* Further reduced margins for more space */
    /* Hide browser default headers and footers */
    @top-left { content: none !important; }
    @top-center { content: none !important; }
    @top-right { content: none !important; }
    @bottom-left { content: none !important; }
    @bottom-center { content: none !important; }
    @bottom-right { content: none !important; }
  }

  * {
    -webkit-print-color-adjust: exact !important;
    print-color-adjust: exact !important;
  }
  html, body {
    margin: 0 !important;
    padding: 0 !important;
    font-family: Arial, sans-serif !important;
    font-size: 11px !important; /* Slightly reduced for more content */
    background: white !important;
    width: 100% !important;
  }
  /* Print utility classes */
  .print\:hidden { display: none !important; }
  .print\:shadow-none { box-shadow: none !important; }
  .print\:max-w-none { max-width: none !important; width: 100% !important; }
  .print\:p-0 { padding: 0 !important; }  /* Make invoice wider and full page */
  .max-w-4xl {
    max-width: none !important;
    width: 100% !important;
    margin: 0 !important;
    padding: 0.1cm !important; /* Minimal padding for maximum width */
    height: 100vh !important;
    position: relative !important;
  }  /* Footer positioning - always at bottom */
  .invoice-footer {
    position: fixed !important;
    bottom: 0.05cm !important; /* Minimal bottom margin */
    left: 0.15cm !important; /* Reduced margins */
    right: 0.15cm !important; /* Reduced margins */
    width: calc(100% - 0.3cm) !important; /* Updated calculation */
    background: white !important;
    border-top: 1px solid #333 !important;
    padding: 3px 0 !important; /* Reduced padding */
    z-index: 999 !important;
    height: 40px !important; /* Reduced height */
    page-break-inside: avoid !important;
  }  /* Content area with space for footer */
  .invoice-content {
    padding-bottom: 50px !important; /* Reduced space for footer */
    min-height: calc(100vh - 50px) !important;
    page-break-after: auto !important;
    width: 100% !important;
    max-width: none !important;
  }
    /* Items table specific handling */
  .invoice-content .mb-6,
  .invoice-items-container {
    margin-bottom: 8px !important; /* Reduced margin */
    display: block !important;
    page-break-inside: auto !important;
    width: 100% !important;
    max-width: none !important;
  }
  
  /* Invoice items table specific styling */
  .invoice-items-table {
    page-break-inside: auto !important;
    margin-bottom: 15px !important; /* Reduced margin */
    width: 100% !important;
    max-width: none !important;
    table-layout: fixed !important;
  }
  
  /* Table container page break handling */
  .page-break-avoid {
    page-break-inside: auto !important;
    break-inside: auto !important;
    display: block !important;
    visibility: visible !important;
  }
  
  /* Ensure table can break across pages properly */
  table {
    page-break-inside: auto !important;
    break-inside: auto !important;
  }
  
  /* Table header should repeat on each page */
  thead {
    display: table-header-group !important;
  }
  
  /* Table rows should avoid breaking in the middle but allow page breaks between rows */
  tbody tr {
    page-break-inside: avoid !important;
    break-inside: avoid !important;
    page-break-after: auto !important;
  }
  
  /* Prevent orphaned rows near page bottom */
  tbody tr:nth-last-child(-n+3) {
    page-break-before: auto !important;
  }  /* Typography and spacing */
  h1 { font-size: 18px !important; } /* Increased from 16px */
  h2 { font-size: 20px !important; } /* Increased from 18px */
  h3 { font-size: 14px !important; } /* Increased from 12px */
  .text-xs { font-size: 11px !important; } /* Increased from 9px */
  .text-sm { font-size: 13px !important; } /* Added for better readability */
  .text-base { font-size: 14px !important; } /* Added base text size */
  .text-lg { font-size: 16px !important; } /* Added large text size */
  .gap-4 { gap: 6px !important; } /* Reduced from 8px */
  .mb-6 { margin-bottom: 8px !important; } /* Reduced from 12px */
  .p-4 { padding: 6px !important; } /* Reduced from 8px */
  .px-2 { padding-left: 3px !important; padding-right: 3px !important; } /* Reduced from 4px */
  .py-1 { padding-top: 1px !important; padding-bottom: 1px !important; } /* Reduced from 2px *//* Layout and table styles */
  .grid { display: grid !important; }
  .lg\:grid-cols-2 { 
    grid-template-columns: repeat(2, minmax(0, 1fr)) !important; 
    display: grid !important;
  }
  .lg\:grid-cols-3 { grid-template-columns: repeat(3, minmax(0, 1fr)) !important; }
  /* Header specific print styles */
  .grid.grid-cols-1.lg\:grid-cols-2,
  .print-header-grid,
  .invoice-header-layout,
  .print-side-by-side {
    grid-template-columns: 1fr 1fr !important;
    display: grid !important;
    gap: 1rem !important;
    width: 100% !important;
    break-inside: avoid !important;
    page-break-inside: avoid !important;
  }
  
  /* Force header components to be side by side */
  .grid.grid-cols-1.lg\:grid-cols-2 > *,
  .print-header-grid > *,
  .invoice-header-layout > *,
  .print-side-by-side > * {
    width: 100% !important;
    display: block !important;
    box-sizing: border-box !important;
    break-inside: avoid !important;
  }
    /* Specific wrapper classes for header components */
  .invoice-header-left,
  .invoice-header-right,
  .print-left-section,
  .print-right-section {
    width: 50% !important;
    max-width: 50% !important;
    display: block !important;
    box-sizing: border-box !important;
    break-inside: avoid !important;
    page-break-inside: avoid !important;
    float: left !important;
  }
  
  .print-right-section {
    float: right !important;
  }
  
  /* Override responsive grid classes in print mode */
  .grid-cols-1 {
    grid-template-columns: 1fr 1fr !important;
  }
  
  /* Flexbox fallback for maximum compatibility */
  .print-side-by-side {
    display: flex !important;
    flex-direction: row !important;
    justify-content: space-between !important;
    align-items: flex-start !important;
  }
  
  /* Ensure header titles are consistent */
  .invoice-header-left h1,
  .invoice-header-right h1 {
    font-size: 14px !important;
    font-weight: bold !important;
    margin-bottom: 8px !important;
  }  /* Table and page break styles */
  table { 
    width: 100% !important; 
    border-collapse: collapse !important;
    page-break-inside: auto !important;
    font-size: 11px !important; /* Added table-specific font size */
  }
  
  /* Table cell font sizes */
  table th {
    font-size: 12px !important; /* Header cells slightly larger */
    font-weight: bold !important;
  }
  
  table td {
    font-size: 11px !important; /* Body cells */
  }
  
  .description-column { 
    width: 400px !important; /* Increased from 350px to utilize more space */
    min-width: 400px !important; 
    max-width: 400px !important; 
  }
  
  /* Advanced page break control for invoice items */
  .invoice-content > div:has(table) {
    page-break-inside: auto !important;
    margin-bottom: 20px !important;
  }
  
  /* Ensure table header repeats on new pages */
  table thead {
    display: table-header-group !important;
  }
  
  table tbody {
    display: table-row-group !important;
  }
  
  /* Row-level page break control */
  tbody tr {
    page-break-inside: avoid !important;
    break-inside: avoid !important;
  }
  
  /* Signatures and summary should prefer to stay on same page */
  .invoice-content > div:last-child,
  .invoice-content > div:nth-last-child(2) {
    page-break-before: auto !important;
    page-break-inside: avoid !important;
  }
}
</style>
