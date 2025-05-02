<template>
  <!-- Header -->
  <div class="flex justify-between items-center mb-4">
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Banka Tablosu
    </h2>

    <button
      @click="router.push({ name: 'create-bank' })"
      type="button"
      class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
    >
      Yeni Banka Oluştur
    </button>
  </div>

  <!-- DataTable -->
  <DataTable :value="banks" responsiveLayout="scroll" dataKey="id">
    <Column header="ID">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>
    <Column field="name" header="Banka Adı" />

    <!-- İşlemler -->
    <Column header="İşlemler" style="width: 180px">
      <template #body="slotProps">
        <div class="flex gap-2">
          <button
            @click="updateBank(slotProps.data.id)"
            type="button"
            class="text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:focus:ring-yellow-900"
          >
            Düzenle
          </button>

          <button
            @click="confirmDeleteBank(slotProps.data.id)"
            type="button"
            class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900"
          >
            Sil
          </button>
        </div>
      </template>
    </Column>
  </DataTable>

  <!-- ConfirmDialog popup -->
  <ConfirmDialog />
</template>

<script setup>
import { DataTable, Column } from "primevue";
import ConfirmDialog from "primevue/confirmdialog";
import { useConfirm } from "primevue/useconfirm";
import { useRouter } from "vue-router";
import { storeToRefs } from "pinia";
import { useBankStore } from "@/stores/bank.store";

// Router & Store
const router = useRouter();
const confirm = useConfirm();
const bankStore = useBankStore();
const { banks } = storeToRefs(bankStore);

// Fetch bank list
await bankStore.fetchBanks();

// Silme işlemi
const confirmDeleteBank = (bankId) => {
  confirm.require({
    message: "Bu bankayı silmek istediğinize emin misiniz?",
    header: "Silme Onayı",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    acceptClass: "p-button-danger",
    rejectClass: "p-button-secondary",
    accept: async () => {
      await bankStore.deleteBank(bankId);
    },
    reject: () => {
      console.log("Silme işlemi iptal edildi.");
    },
  });
};

// Düzenleme sayfasına yönlendir
const updateBank = (id) => {
  router.push({ name: "update-bank", params: { id } });
};
</script>

<style scoped></style>
