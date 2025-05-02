<template>
  <!-- Header -->
  <div class="flex justify-between items-center mb-4">
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Depo Adres Tablosu
    </h2>

    <button
      @click="router.push({ name: 'create-warehouse' })"
      type="button"
      class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
    >
      + Yeni Depo Ekle
    </button>
  </div>

  <!-- DataTable -->
  <DataTable
    :value="flattenedWarehouses"
    responsiveLayout="scroll"
    dataKey="id"
  >
    <Column field="warehouseName" header="Depo Adı" />
    <Column field="street" header="Sokak" />
    <Column field="city" header="Şehir" />
    <Column field="country" header="Ülke" />
    <Column field="zipCode" header="Posta Kodu" />

    <Column header="İşlemler" style="width: 180px">
      <template #body="slotProps">
        <div class="flex gap-2">
          <button
            @click="updateWarehouse(slotProps.data.id)"
            type="button"
            class="text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:focus:ring-yellow-900"
          >
            Düzenle
          </button>

          <button
            @click="confirmDeleteWarehouse(slotProps.data.id)"
            type="button"
            class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900"
          >
            Sil
          </button>

          <router-link
            :to="{
              name: 'table-contact-warehouse',
              params: { warehouseId: slotProps.data.id },
            }"
            class="text-white bg-blue-600 hover:bg-blue-700 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-900"
          >
            iletişim</router-link
          >
        </div>
      </template>
    </Column>
  </DataTable>

  <!-- ConfirmDialog -->
  <ConfirmDialog />
</template>

<script setup>
import { DataTable, Column } from "primevue";
import { useRouter } from "vue-router";
import { storeToRefs } from "pinia";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { useConfirm } from "primevue/useconfirm";
import ConfirmDialog from "primevue/confirmdialog";

// Router & Store
const router = useRouter();
const warehouseStore = useWarehouseStore();
const { flattenedWarehouses } = storeToRefs(warehouseStore);

// Confirm service
const confirm = useConfirm();

// Fetch warehouses
await warehouseStore.fetchIncludeWarehouses();

// Silme işlemi onaylı
const confirmDeleteWarehouse = (warehouseId) => {
  confirm.require({
    message: "Bu depoyu silmek istediğinize emin misiniz?",
    header: "Silme Onayı",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    acceptClass: "p-button-danger",
    rejectClass: "p-button-secondary",
    accept: async () => {
      await warehouseStore.deleteWarehouse(warehouseId);
    },
    reject: () => {
      console.log("Silme işlemi iptal edildi.");
    },
  });
};

// Düzenleme
const updateWarehouse = (warehouseId) => {
  router.push({ name: "update-warehouse", params: { id: warehouseId } });
};
</script>

<style scoped></style>
