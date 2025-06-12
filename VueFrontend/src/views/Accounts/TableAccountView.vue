<template>  <!-- Header Section -->
  <div class="flex justify-between items-center mb-4">
    <!-- Yazıyı büyüt + y ekseninde hizala -->
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Hesap Planı
    </h2>

    <div class="flex gap-2">
      <button
        @click="router.push({ name: 'create-parent-account' })"
        type="button"
        class="flex items-center gap-2 text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800"
      >
        Yeni Üst Hesap Oluştur
      </button>
      
      <button
        @click="router.push({ name: 'create-account' })"
        type="button"
        class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
      >
        Yeni Hesap Oluştur
      </button>
    </div>
  </div>  <!-- DataTable -->
  <DataTable
    :value="accounts"
    v-model:filters="filters"
    responsiveLayout="scroll"
    dataKey="id"
    paginator
    :rows="10"
    filterDisplay="menu"
    sortMode="multiple"
    :globalFilterFields="['code', 'name', 'parentAccount.name']"
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
          <h3 class="mt-2 text-sm font-medium text-gray-900 dark:text-gray-100">Hesap bulunamadı</h3>
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
          <p class="mt-2 text-sm">Hesaplar yükleniyor. Lütfen bekleyin...</p>
        </div>
      </div>
    </template>    <Column header="Sıra" style="width: 60px">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>
    <Column field="code" header="Hesap Kodu" sortable style="min-width: 120px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Kod ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="name" header="Hesap Adı" sortable style="min-width: 200px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Ad ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="parentAccount.name" header="Üst Hesap" sortable style="min-width: 150px">
      <template #body="slotProps">
        {{ slotProps.data.parentAccount ? slotProps.data.parentAccount.name : '-' }}
      </template>
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="Üst hesap ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>
    <Column field="accountType" header="Hesap Türü" sortable style="min-width: 120px">
      <template #body="slotProps">
        {{ getAccountTypeName(slotProps.data.accountType) }}
      </template>
      <template #filter="{ filterModel }">
        <select
          v-model="filterModel.value"
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
        >
          <option value="">Tümü</option>
          <option value="1">Varlık</option>
          <option value="2">Borç</option>
          <option value="3">Özsermaye</option>
          <option value="4">Gelir</option>
          <option value="5">Gider</option>
        </select>
      </template>
    </Column>
    <Column field="normalBalance" header="Normal Bakiye" sortable style="min-width: 120px">      <template #body="slotProps">
        {{ getNormalBalanceName(slotProps.data.normalBalance) }}
      </template>
      <template #filter="{ filterModel }">
        <select
          v-model="filterModel.value"
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
        >
          <option value="">Tümü</option>
          <option value="1">Borç</option>
          <option value="2">Alacak</option>
        </select>
      </template>
    </Column>
    <Column field="isActive" header="Aktif" sortable style="min-width: 100px">
      <template #body="slotProps">
        <span
          :class="
            'px-2 py-1 text-xs font-medium rounded-full',
            slotProps.data.isActive
              ? 'bg-green-100 text-green-800'
              : 'bg-red-100 text-red-800'
          "
        >
          {{ slotProps.data.isActive ? "Evet" : "Hayır" }}
        </span>
      </template>
      <template #filter="{ filterModel }">
        <select
          v-model="filterModel.value"
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
        >
          <option value="">Tümü</option>
          <option value="true">Aktif</option>
          <option value="false">Pasif</option>
        </select>
      </template>
    </Column>
    <Column field="isPostable" header="Kayıt Yapılabilir" sortable style="min-width: 150px">      <template #body="slotProps">
        <span
          :class="
            'px-2 py-1 text-xs font-medium rounded-full',
            slotProps.data.isPostable
              ? 'bg-green-100 text-green-800'
              : 'bg-red-100 text-red-800'
          "
        >
          {{ slotProps.data.isPostable ? "Evet" : "Hayır" }}
        </span>
      </template>
      <template #filter="{ filterModel }">
        <select
          v-model="filterModel.value"
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
        >
          <option value="">Tümü</option>
          <option value="true">Evet</option>
          <option value="false">Hayır</option>
        </select>
      </template>
    </Column>    <!-- İşlemler -->
    <Column header="İşlemler" style="min-width: 150px">
      <template #body="{ data }">
        <div class="flex gap-2">
          <!-- Edit Button -->
          <button
            @click="updateAccount(data.id)"
            class="p-2 text-blue-600 hover:text-blue-800 hover:bg-blue-100 dark:hover:bg-blue-900 rounded-lg transition-colors"
            title="Düzenle"
          >
            <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
              <path
                d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z"
              />
            </svg>
          </button>

          <!-- Delete Button -->
          <button
            @click="confirmDeleteAccount(data.id)"
            class="p-2 text-red-600 hover:text-red-800 hover:bg-red-100 dark:hover:bg-red-900 rounded-lg transition-colors"
            title="Sil"
          >
            <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
              <path
                fill-rule="evenodd"
                d="M9 2a1 1 0 000 2h2a1 1 0 100-2H9zM4 5a2 2 0 012-2h8a2 2 0 012 2v6a2 2 0 01-2 2H6a2 2 0 01-2-2V5zm3 4a1 1 0 102 0v-1a1 1 0 10-2 0v1zm4 0a1 1 0 102 0v-1a1 1 0 10-2 0v1z"
                clip-rule="evenodd"
              />
            </svg>
          </button>
        </div>
      </template>
    </Column>
  </DataTable>
  
  <!-- ConfirmDialog popup -->
  <ConfirmDialog />
</template>

<script setup>
import { ref, onMounted } from "vue";
import { FilterMatchMode, FilterOperator } from "@primevue/core";
import { DataTable, Column, ConfirmDialog } from "primevue";
import { useRouter } from "vue-router";
import { storeToRefs } from "pinia";
import { useAccountStore } from "@/stores/account.store";
import { useConfirm } from "primevue/useconfirm";

// Router & Store
const router = useRouter();
const accountStore = useAccountStore();
const { accounts, loading, accountTypeOptions, normalBalanceOptions } = storeToRefs(accountStore);

// Filter state initialization
const initFilters = () => {
  filters.value = {
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    code: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    name: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    'parentAccount.name': { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    accountType: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    normalBalance: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    isActive: { 
      operator: FilterOperator.AND, 
      constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] 
    },
    isPostable: { 
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

// PrimeVue Confirm service
const confirm = useConfirm();

// Fetch accounts
onMounted(async () => {
  await accountStore.fetchAccounts();
});

// Silme işlemi onaylı
const confirmDeleteAccount = (id) => {
  confirm.require({
    message: "Bu hesabı silmek istediğinizden emin misiniz?",
    header: "Silme İşlemi",
    icon: "pi pi-exclamation-triangle",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    accept: () => {
      accountStore.deleteAccount(id);
    },
  });
};

// Düzenleme sayfasına yönlendirme
const updateAccount = (id) => {
  const account = accounts.value.find(acc => acc.id === id);
  
  // Hesap türüne göre farklı güncelleme sayfalarına yönlendir
  if (account && !account.isPostable) {
    // Üst hesap (isPostable=false)
    router.push({ name: "update-parent-account", params: { id } });
  } else {
    // Normal hesap (isPostable=true)
    router.push({ name: "update-account", params: { id } });
  }
};

// Hesap türü adını alma
const getAccountTypeName = (typeId) => {
  const found = accountTypeOptions.value.find((type) => type.value === typeId);
  return found ? found.label : "-";
};

// Normal bakiye adını alma
const getNormalBalanceName = (balanceId) => {
  if (balanceId === null) return "-";
  const found = normalBalanceOptions.value.find((balance) => balance.value === balanceId);
  return found ? found.label : "-";
};
</script>

<style scoped></style>
