<template>
  <div class="flex justify-between items-center mb-4">
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Satış Faturaları
    </h2>
    <div class="flex gap-2">
      <button
        @click="router.push({ name: 'create-local-sale' })"
        type="button"
        class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
      >
        Yeni Lokal Fatura
      </button>
      <button
        @click="router.push({ name: 'create-export-sale' })"
        type="button"
        class="flex items-center gap-2 text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800"
      >
        Yeni İhracat Faturası
      </button>
    </div>
  </div>

  <ConfirmDialog />

  <DataTable
    :value="sales"
    dataKey="id"
    responsiveLayout="scroll"
    :loading="loading"
  >
    <!-- Sıra Numarası -->
    <Column header="#" style="width: 60px">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>

    <!-- Fatura No -->
    <Column field="invoiceNo" header="Fatura No" style="min-width: 120px" />

    <!-- Fatura Tarihi -->
    <Column field="invoiceDate" header="Fatura Tarihi" style="min-width: 120px">
      <template #body="{ data }">
        {{ formatDate(data.invoiceDate) }}
      </template>
    </Column>

    <!-- Partner (Müşteri) -->
    <Column field="partner.name" header="Müşteri" style="min-width: 150px">
      <template #body="{ data }">
        {{ partnerNames[data.partnerId] || "-" }}
      </template>
    </Column>

    <!-- Fatura Tipi -->
    <Column field="saleInvoiceType" header="Tip" style="min-width: 120px">
      <template #body="{ data }">
        <span
          class="px-2 py-1 text-xs font-medium rounded-full"
          :class="getInvoiceTypeColor(data.saleInvoiceType)"
        >
          {{ getInvoiceTypeLabel(data.saleInvoiceType) }}
        </span>
      </template>
    </Column>

    <!-- Ödeme Durumu -->
    <Column field="isPaid" header="Ödeme" style="min-width: 80px">
      <template #body="{ data }">
        <span
          class="px-2 py-1 text-xs font-medium rounded-full"
          :class="data.isPaid ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'"
        >
          {{ data.isPaid ? "Ödendi" : "Ödenmedi" }}
        </span>
      </template>
    </Column>

    <!-- Toptan Satış -->
    <Column field="isWholeSale" header="Toptan" style="min-width: 80px">
      <template #body="{ data }">
        <span
          class="px-2 py-1 text-xs font-medium rounded-full"
          :class="data.isWholeSale ? 'bg-blue-100 text-blue-800' : 'bg-gray-100 text-gray-800'"
        >
          {{ data.isWholeSale ? "Evet" : "Hayır" }}
        </span>
      </template>
    </Column>

    <!-- Nakit Ödeme -->
    <Column field="cashPaymentAmount" header="Nakit Ödeme" style="min-width: 120px">
      <template #body="{ data }">
        {{ formatCurrency(data.cashPaymentAmount || 0, "€") }}
      </template>
    </Column>

    <!-- Toplam Tutar -->
    <Column header="Toplam Tutar" style="min-width: 120px">
      <template #body="{ data }">
        {{ formatCurrency(calculateInvoiceTotal(data), "€") }}
      </template>
    </Column>    <!-- İşlemler -->
    <Column header="İşlemler" style="min-width: 200px">
      <template #body="{ data }">        <div class="flex gap-2">
          <!-- Düzenle Butonu -->
          <router-link
            :to="{
              name: data.saleInvoiceType === 2 ? 'update-export-sale' : 'update-local-sale',
              params: { id: data.id },
            }"
            class="text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:focus:ring-yellow-900"
          >
            Düzenle
          </router-link>

          <!-- Sil Butonu -->
          <button
            @click="handleDelete(data)"
            type="button"
            class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900"
          >
            Sil
          </button>

          <!-- Dropdown İşlemler -->
          <TableDropdownButton buttonText="İşlemler">
            <a
              href="#"
              @click.prevent="viewDetails(data)"
              class="dropdown-item"
            >
              Detayları Görüntüle
            </a>

            <a href="#" @click.prevent="viewLines(data)" class="dropdown-item">
              Fatura Satırları
            </a>

            <a
              href="#"
              @click.prevent="viewLedgerDetails(data)"
              class="dropdown-item"
            >
              Defter Kayıtları
            </a>

            <hr class="my-2 border-gray-200 dark:border-gray-600" />
            <a
              href="#"
              @click.prevent="duplicateSale(data)"
              class="dropdown-item"
            >
              Faturayı Kopyala
            </a>
          </TableDropdownButton>
        </div>
      </template>
    </Column>
  </DataTable>
  <!-- Ledger Modal -->
  <LedgerModal v-model="showLedgerModal">
    <LedgerDetailsModal v-if="selectedLedgerId" :ledger-id="selectedLedgerId" />
  </LedgerModal>
</template>

<script setup>
import { ref, onMounted, computed } from "vue";
import { useRouter } from "vue-router";
import { useConfirm } from "primevue/useconfirm";
import { storeToRefs } from "pinia";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import ConfirmDialog from "primevue/confirmdialog";
import { useSaleStore } from "@/stores/sale.store";
import { usePartnerStore } from "@/stores/partner.store";
import { useCurrencyStore } from "@/stores/currency.store";
import TableDropdownButton from "@/components/UI/Buttons/TableDropdownButton.vue";
import LedgerModal from "@/components/UI/Modal/LedgerModal.vue";
import LedgerDetailsModal from "@/components/Views/Sale/LedgerDetailsModal.vue";

const router = useRouter();
const confirm = useConfirm();

// Stores
const saleStore = useSaleStore();
const partnerStore = usePartnerStore();
const currencyStore = useCurrencyStore();

// Reactive state
const { sales, loading } = storeToRefs(saleStore);
const { partners } = storeToRefs(partnerStore);

// Modal state
const showLedgerModal = ref(false);
const selectedLedgerId = ref(null);

// Computed partner names mapping
const partnerNames = computed(() => {
  const mapping = {};
  partners.value.forEach((partner) => {
    mapping[partner.id] = partner.name;
  });
  return mapping;
});

// Lifecycle hooks
onMounted(async () => {
  await Promise.all([
    saleStore.fetchSales(),
    partnerStore.fetchPartners(),
    currencyStore.fetchCurrencies(),
  ]);
});

// Helper functions
const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString("tr-TR");
};

const formatCurrency = (value, currency = "€", precision = 2) => {
  return new Intl.NumberFormat("de-DE", {
    style: "currency",
    currency: "EUR",
    minimumFractionDigits: precision,
    maximumFractionDigits: precision,
  }).format(value || 0);
};

const getInvoiceTypeColor = (type) => {
  switch (type) {
    case 1: // LocalInvoice
      return "bg-blue-100 text-blue-800";
    case 2: // ExportInvoice
      return "bg-green-100 text-green-800";
    case 3: // DebitNote
      return "bg-yellow-100 text-yellow-800";
    case 4: // CreditNote
      return "bg-red-100 text-red-800";
    default:
      return "bg-gray-100 text-gray-800";
  }
};

const getInvoiceTypeLabel = (type) => {
  switch (type) {
    case 1:
      return "Lokal";
    case 2:
      return "İhracat";
    case 3:
      return "Borç Dekontu";
    case 4:
      return "Alacak Dekontu";
    default:
      return "Bilinmeyen";
  }
};

const calculateInvoiceTotal = (sale) => {
  if (!sale.saleInvoiceLines || sale.saleInvoiceLines.length === 0) {
    return 0;
  }
  return sale.saleInvoiceLines.reduce((total, line) => {
    return total + (line.totalAmount || 0);
  }, 0);
};

// Detail view functions
const viewDetails = async (sale) => {
  try {
    await saleStore.fetchSale(sale.id);
    // TODO: Show details in a modal or navigate to details page
    console.log("Sale details:", saleStore.sale);
  } catch (error) {
    console.error("Error fetching sale details:", error);
  }
};

const viewLines = async (sale) => {
  try {
    const lines = await saleStore.fetchSaleLines(sale.id);
    console.log("Sale lines:", lines);
    // TODO: Show lines in a modal
  } catch (error) {
    console.error("Error fetching sale lines:", error);
  }
};

// Show ledger details modal
const viewLedgerDetails = (sale) => {
  if (sale.ledger?.id) {
    selectedLedgerId.value = sale.ledger.id;
    showLedgerModal.value = true;
  } else {
    console.warn("Ledger information not found for sale:", sale.id);
  }
};

const duplicateSale = (sale) => {
  // TODO: Implement sale duplication
  console.log("Duplicating sale:", sale.id);
};

const handleEdit = (sale) => {
  const routeName = sale.saleInvoiceType === 2 ? 'update-export-sale' : 'update-local-sale';
  router.push({
    name: routeName,
    params: { id: sale.id },
  });
};

const handleDelete = (sale) => {
  console.log("Deleting sale:", sale);
  // confirm.require({
  //   message: `"${sale.invoiceNo}" numaralı satış faturasını silmek istediğinizden emin misiniz?`,
  //   header: "Silme Onayı",
  //   icon: "pi pi-exclamation-triangle",
  //   rejectProps: {
  //     label: "İptal",
  //     severity: "secondary",
  //     outlined: true,
  //   },
  //   acceptProps: {
  //     label: "Sil",
  //     severity: "danger",
  //   },
  //   accept: async () => {
  //     try {
  //       await saleStore.deleteSale(sale.id);
  //     } catch (error) {
  //       console.error("Delete error:", error);
  //     }
  //   },
  // });
};
</script>

<style scoped>
.dropdown-item {
  display: block;
  padding: 0.5rem 1rem;
  font-size: 0.875rem;
  color: #374151;
  transition: background 0.2s;
}

.dropdown-item:hover {
  background-color: #f3f4f6;
}

@media (prefers-color-scheme: dark) {
  .dropdown-item {
    color: #d1d5db;
  }

  .dropdown-item:hover {
    background-color: #374151;
  }
}
</style>
