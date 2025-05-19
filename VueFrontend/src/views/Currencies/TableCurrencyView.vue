<template>
  <!-- Header Section -->
  <div class="flex justify-between items-center mb-4">
    <!-- Yazıyı büyüt + y ekseninde hizala -->
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Para Birimi Tablosu
    </h2>

    <!-- Buton boyutunu koru, ama içerik dikey ortalansın -->
    <button
      @click="router.push({ name: 'create-currency' })"
      type="button"
      class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
    >
      <span class="material-icons-outlined text-base">add</span>
      Yeni Para Birimi Oluştur
    </button>
  </div>

  <!-- ConfirmDialog -->
  <ConfirmDialog />

    <!-- DataTable -->
    <DataTable :value="currencies" responsiveLayout="scroll" dataKey="id">
        <Column header="ID">
            <template #body="{ index }">
                {{ index + 1 }}
            </template>
        </Column>
        <Column field="code" header="Kod" />
        <Column field="name" header="İsim" />

        <Column header="İşlemler" style="width: 180px">
            <template #body="{ data }">
                <div class="flex gap-2">
                    <button @click="router.push({ name: 'update-currency', params: { id: data.id } })" type="button"
                        class="text-white bg-yellow-500 hover:bg-yellow-600 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-4 py-2 dark:bg-yellow-400 dark:hover:bg-yellow-500 dark:focus:ring-yellow-600">
                        Düzenle
                    </button>
                    <button @click="confirmDelete(data.id)" type="button"
                        class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-4 py-2 dark:bg-red-500 dark:hover:bg-red-600 dark:focus:ring-red-800">
                        Sil
                    </button>
                </div>
            </template>
        </Column>
    </DataTable>
</template>

<script setup>
import { onMounted } from "vue";
import { useRouter } from "vue-router";
import { storeToRefs } from "pinia";
import { useConfirm } from "primevue/useconfirm";
import { useCurrencyStore } from "@/stores/currency.store";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import ConfirmDialog from "primevue/confirmdialog";

const router = useRouter();
const confirm = useConfirm();

// Store
const currencyStore = useCurrencyStore();
const { currencies } = storeToRefs(currencyStore);

// Delete confirmation
const confirmDelete = (id) => {
  confirm.require({
    message: "Bu para birimini silmek istediğinize emin misiniz?",
    header: "Silme Onayı",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    acceptClass: "p-button-danger",
    rejectClass: "p-button-secondary",
    icon: "pi pi-exclamation-triangle",
    accept: async () => {
      await currencyStore.deleteCurrency(id);
    },
    reject: () => {
      console.log("Silme işlemi iptal edildi.");
    },
  });
};

// Fetch currencies on component mount
  await currencyStore.fetchCurrencies();
</script>
