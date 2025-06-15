<template>
  <div
    class="grid grid-cols-1 lg:grid-cols-2 gap-6 print-header-grid invoice-header-layout print-side-by-side"
    style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem"
  >    <!-- Informacioni i Kompanisë (Majtas) -->
    <div class="invoice-header-left print-left-section">
      <div>
        <h1 class="text-3xl font-bold text-gray-800 mb-2">{{ companies[0]?.name }}</h1>
        <div class="text-base text-gray-600">
          <p>
            {{ warehouse.addressWarehouses?.[0]?.address?.street }},
            {{ warehouse.addressWarehouses?.[0]?.address?.city }},
            {{ warehouse.addressWarehouses?.[0]?.address?.country }},
            {{ warehouse.addressWarehouses?.[0]?.address?.zipCode }}
          </p>
          <p>
            {{
              warehouse.contactWarehouses?.[0]?.contact?.contactDetails?.[0]
                ?.name
            }}:
            {{
              warehouse.contactWarehouses?.[0]?.contact?.contactDetails?.[0]
                ?.value
            }}
            <span
              v-if="
                warehouse.contactWarehouses?.[0]?.contact?.contactDetails?.[1]
              "
            >
              |
              {{
                warehouse.contactWarehouses?.[0]?.contact?.contactDetails?.[1]
                  ?.name
              }}:
              {{
                warehouse.contactWarehouses?.[0]?.contact?.contactDetails?.[1]
                  ?.value
              }}
            </span>
          </p>
          <p>
            Nr. UID: {{ companies[0]?.uidNumber }}
            <span v-if="companies[0]?.vatNumber">
              | Nr. TVSH: {{ companies[0]?.vatNumber }}</span
            >
          </p>
        </div>
      </div>
    </div>
    <!-- Banka bilgileri (Sağ) -->
    <div class="invoice-header-right print-right-section">
      <div>        <h1 class="text-3xl font-bold text-gray-800 mb-2">Informacioni Bankar</h1>
        <div class="text-base text-gray-600">
          <div
            v-for="bank in bankAccounts.slice(0, 2)"
            :key="bank.bankAccountId"
          >
            <p>
              {{ filteredBank(bank.bankAccount.bankId) }}
            </p>
            <p>
              IBAN: {{ bank.bankAccount.iban }}
              <span v-if="bank.bankAccount.accountNumber">
                | Hesap No: {{ bank.bankAccount.accountNumber }}</span
              >
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { storeToRefs } from "pinia";
import { useCompanyStore } from "@/stores/company.store";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { useBankStore } from "@/stores/bank.store";
import { useBankAccountCompanyStore } from "@/stores/bank-account-company.store";

// Company and Warehouse stores
const companyStore = useCompanyStore();
const warehouseStore = useWarehouseStore();

// Bank stores
const bankAccountCompanyStore = useBankAccountCompanyStore();
const bankStore = useBankStore();

// Reactive references
const { companies } = storeToRefs(companyStore);
const { warehouse } = storeToRefs(warehouseStore);
const { banks } = storeToRefs(bankStore);
const { bankAccounts } = storeToRefs(bankAccountCompanyStore);

// Computed property to filter banks based on the provided bank IDs
const filteredBank = (bankId) => {
  return banks.value.find((bank) => bank.id === bankId)?.name || "Bilinmiyor";
};
</script>

<style scoped>
/* Ensure side-by-side layout in all modes */
.print-side-by-side {
  display: grid !important;
  grid-template-columns: 1fr 1fr !important;
  gap: 1rem !important;
  width: 100% !important;
}

.invoice-header-left,
.invoice-header-right,
.print-left-section,
.print-right-section {
  width: 100% !important;
  box-sizing: border-box !important;
}

/* Print-specific styles */
@media print {
  /* Font size adjustments for print */
  h1 {
    font-size: 16px !important;
    line-height: 1.2 !important;
    margin-bottom: 8px !important;
  }

  .text-sm {
    font-size: 11px !important;
    line-height: 1.3 !important;
  }

  .print-side-by-side {
    display: grid !important;
    grid-template-columns: 1fr 1fr !important;
    gap: 1rem !important;
    width: 100% !important;
    break-inside: avoid !important;
    page-break-inside: avoid !important;
  }

  .invoice-header-left,
  .invoice-header-right,
  .print-left-section,
  .print-right-section {
    width: 50% !important;
    max-width: 50% !important;
    min-width: 45% !important;
    display: block !important;
    box-sizing: border-box !important;
    break-inside: avoid !important;
    page-break-inside: avoid !important;
    float: left !important;
  }

  .print-right-section {
    float: right !important;
  }

  /* Force grid layout on all screen sizes in print */
  .grid-cols-1 {
    grid-template-columns: 1fr 1fr !important;
    display: grid !important;
  }

  .lg\:grid-cols-2 {
    grid-template-columns: 1fr 1fr !important;
    display: grid !important;
  }

  /* Additional fallback with flexbox */
  .print-side-by-side {
    display: flex !important;
    flex-direction: row !important;
    justify-content: space-between !important;
  }

  .print-left-section,
  .print-right-section {
    flex: 1 !important;
    margin-right: 1rem !important;
  }

  .print-right-section {
    margin-right: 0 !important;
    margin-left: 1rem !important;
  }
}
</style>
