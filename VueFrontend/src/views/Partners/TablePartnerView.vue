<template>
  <div class="flex justify-between items-center mb-4">
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Partner Listesi
    </h2>

    <button
      @click="router.push({ name: 'create-partner' })"
      type="button"
      class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
    >
      Yeni Partner
    </button>
  </div>

  <ConfirmDialog />

  <DataTable 
    :value="partners" 
    v-model:filters="filters"
    dataKey="id" 
    responsiveLayout="scroll"
    paginator
    :rows="10"
    filterDisplay="menu"
    sortMode="multiple"
    :globalFilterFields="['name', 'tradeName', 'identityNumber', 'vatNumber']"
    :loading="loading"
  >
    <template #header>
      <div class="flex flex-wrap justify-between items-center gap-4">
        <!-- Clear Filter Button -->
        <button
          @click="clearFilter"
          type="button"
          class="flex items-center gap-2 text-gray-700 bg-gray-100 hover:bg-gray-200 focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-4 py-2 focus:outline-none dark:bg-gray-600 dark:hover:bg-gray-700 dark:text-white dark:focus:ring-gray-800"
        >
          <span>🗑️</span>
          Filtreleri Temizle
        </button>

        <!-- Global Search -->
        <div class="flex items-center gap-2">
          <div class="relative">
            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <svg class="w-4 h-4 text-gray-500 dark:text-gray-400" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd"></path>
              </svg>
            </div>
            <input
              v-model="filters['global'].value"
              type="text"
              placeholder="Anahtar kelime ile ara..."
              class="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-md leading-5 bg-white placeholder-gray-500 focus:outline-none focus:placeholder-gray-400 focus:ring-1 focus:ring-blue-500 focus:border-blue-500 sm:text-sm dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
            />
          </div>
        </div>
      </div>
    </template>

    <template #empty>
      <div class="text-center py-8">
        <div class="text-gray-500 dark:text-gray-400">
          <svg class="mx-auto h-12 w-12 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
          </svg>
          <h3 class="mt-2 text-sm font-medium text-gray-900 dark:text-gray-100">Partner bulunamadı</h3>
          <p class="mt-1 text-sm text-gray-500 dark:text-gray-400">Arama kriterlerinizi değiştirerek tekrar deneyin.</p>
        </div>
      </div>
    </template>

    <template #loading>
      <div class="text-center py-8">
        <div class="text-gray-500 dark:text-gray-400">
          <div class="animate-spin inline-block w-6 h-6 border-[3px] border-current border-t-transparent text-blue-600 rounded-full" role="status" aria-label="loading">
            <span class="sr-only">Yükleniyor...</span>
          </div>
          <p class="mt-2 text-sm">Partnerler yükleniyor. Lütfen bekleyin...</p>
        </div>
      </div>
    </template>    <!-- Sıra Numarası -->
    <Column header="#" style="width: 60px">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>

    <!-- Ana Bilgiler -->
    <Column field="name" header="Adı" sortable style="min-width: 200px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Ad ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="tradeName" header="Ticari Ünvan" sortable style="min-width: 200px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Ticari ünvan ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>

    <!-- Partner Tipi -->
    <Column field="partnerType" header="Partner Tipi" sortable style="min-width: 120px">
      <template #body="{ data }">
        {{ getPartnerTypeLabel(data.partnerType) }}
      </template>
      <template #filter="{ filterModel }">
        <select
          v-model="filterModel.value"
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
        >
          <option value="">Tümü</option>
          <option value="1">Şahıs</option>
          <option value="2">Şirket</option>
        </select>
      </template>
    </Column>

    <!-- İş Partner Tipi -->
    <Column field="businessPartnerType" header="İş Partner Tipi" sortable style="min-width: 140px">
      <template #body="{ data }">
        {{ getBusinessPartnerTypeLabel(data.businessPartnerType) }}
      </template>
      <template #filter="{ filterModel }">
        <select
          v-model="filterModel.value"
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
        >
          <option value="">Tümü</option>
          <option value="1">Müşteri</option>
          <option value="2">Tedarikçi</option>
          <option value="3">Her ikisi</option>
        </select>
      </template>
    </Column>

    <!-- Diğer Bilgiler -->
    <Column field="identityNumber" header="Kimlik No" sortable style="min-width: 120px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Kimlik no ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="vatNumber" header="Vergi No" sortable style="min-width: 120px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Vergi no ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>

    <!-- İşlem Butonları -->    <Column header="İşlemler" style="min-width: 200px">
      <template #body="{ data }">
        <div class="flex gap-2">
          <!-- Edit Button -->
          <EditButton @click="updatePartner(data.id)" />

          <!-- Delete Button -->
          <DeleteButton @click="confirmDeletePartner(data.id)" />
          
          <!-- More Actions Dropdown -->
          <TableDropdownButton buttonText="İşlemler">
            <router-link
              :to="{
                name: 'table-bank-account-partner',
                params: { partnerId: data.id },
              }"
              class="dropdown-item"
            >
              Banka
            </router-link>

            <router-link
              :to="{
                name: 'table-contact-partner',
                params: { partnerId: data.id },
              }"
              class="dropdown-item"
            >
              İletişim
            </router-link>
          </TableDropdownButton>
        </div>
      </template>
    </Column>
  </DataTable>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { FilterMatchMode, FilterOperator } from "@primevue/core";
import { useRouter } from "vue-router";
import { useConfirm } from "primevue/useconfirm";
import ConfirmDialog from "primevue/confirmdialog";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import { usePartnerStore } from "@/stores/partner.store";
import { storeToRefs } from "pinia";
import TableDropdownButton from "@/components/UI/Buttons/TableDropdownButton.vue";
import EditButton from "@/components/UI/Buttons/EditButton.vue";
import DeleteButton from "@/components/UI/Buttons/DeleteButton.vue";

// Ortak enum fonksiyonlarını import et
import {
  getPartnerTypeLabel,
  getBusinessPartnerTypeLabel,
} from "@/services/constants/partner-enum";

const router = useRouter();
const confirm = useConfirm();

const partnerStore = usePartnerStore();
const { partners, loading } = storeToRefs(partnerStore);

// Filter state initialization
const initFilters = () => {
  filters.value = {
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    name: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    tradeName: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    partnerType: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    businessPartnerType: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    identityNumber: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    vatNumber: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
  };
};

// Filter state
const filters = ref();
initFilters();

// Clear filters function
const clearFilter = () => {
  initFilters();
};

// Lifecycle
onMounted(async () => {
  await partnerStore.fetchPartners();
});

// Partner güncelleme işlemi
const updatePartner = (id) => {
  router.push({ name: "update-partner", params: { id } });
};

// Partner silme işlemi
const confirmDeletePartner = (partnerId) => {
  confirm.require({
    message: "Bu partneri silmek istediğinize emin misiniz?",
    header: "Onay",
    icon: "pi pi-exclamation-triangle",
    acceptClass: "p-button-danger",
    accept: async () => {
      await partnerStore.deletePartner(partnerId);
    },
  });
};
</script>
