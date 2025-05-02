<template>
  <!-- Header Section -->
  <div class="flex justify-between items-center mb-4">
    <!-- Yazıyı büyüt + y ekseninde hizala -->
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Şirket Tablosu
    </h2>

    <!-- Buton boyutunu koru, ama içerik dikey ortalansın -->
    <button
      :disabled="companies.length > 0"
      @click="router.push({ name: 'create-company' })"
      type="button"
      class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
    >
      <span v-if="companies.length > 0">🚫</span>
      Yeni Şirket Oluştur
    </button>
  </div>

  <!-- DataTable -->
  <DataTable :value="companies" responsiveLayout="scroll" dataKey="id">
    <Column header="ID">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>
    <Column field="name" header="Name" />
    <Column field="tradeName" header="Trade Name" />
    <Column field="uidNumber" header="UID Number" />
    <Column field="vatNumber" header="VAT Number" />
    <Column field="period" header="Period" />

    <!-- İşlemler -->
    <Column header="İşlemler" style="width: 180px">
      <template #body="slotProps">
        <div class="flex gap-2">
          <!-- Düzenle Butonu -->
          <button
            @click="updateCompany(slotProps.data.id)"
            type="button"
            class="text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:focus:ring-yellow-900"
          >
            Düzenle
          </button>

          <!-- Sil Butonu -->
          <button
            @click="confirmDeleteCompany(slotProps.data.id)"
            type="button"
            class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900"
          >
            Sil
          </button>
          <router-link :to="{ name:'table-warehouse' }"
         class="text-white bg-blue-600 hover:bg-blue-700 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-900"
          >Depolar</router-link>
        </div>
      </template>
    </Column>
  </DataTable>
  <!-- ConfirmDialog popup -->
  <ConfirmDialog />
</template>

<script setup>
import { DataTable, Column, ConfirmDialog } from "primevue";
import { useRouter } from "vue-router";
import { storeToRefs } from "pinia";
import { useCompanyStore } from "@/stores/company.store";
import { useConfirm } from "primevue/useconfirm";

// Router & Store
const router = useRouter();
const companyStore = useCompanyStore();
const { companies } = storeToRefs(companyStore);

// PrimeVue Confirm service
const confirm = useConfirm();

// Fetch companies
await companyStore.fetchCompanies();

// Silme işlemi onaylı
const confirmDeleteCompany = (companyId) => {
  confirm.require({
    message: "Bu şirketi silmek istediğinize emin misiniz?",
    header: "Silme Onayı",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    acceptClass: "p-button-danger",
    rejectClass: "p-button-secondary",
    accept: async () => {
      await companyStore.deleteCompany(companyId);
    },
    reject: () => {
      console.log("Silme işlemi iptal edildi.");
    },
  });
};

// Düzenleme
const updateCompany = (id) => {
  router.push({ name: "update-company", params: { id } });
};
</script>
