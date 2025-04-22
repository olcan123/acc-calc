<template>
  <div class="p-6">
    <div class="flex items-center justify-between mb-4">
      <h1 class="text-3xl font-bold text-gray-900 dark:text-white">Depolar ve Adresleri</h1>

      <router-link
        :to="{ name: 'create-warehouse', params: { id: company.data.id } }"
        class="px-4 py-2 text-sm text-white bg-green-700 hover:bg-green-800 rounded-lg"
      >
        Yeni Depo Ekle
      </router-link>
    </div>

    <!-- DataTable -->
    <DataTable
      :value="flatWarehousesWithAddresses(company.data.warehouses)"
      paginator
      :rows="5"
      stripedRows
      responsiveLayout="scroll"
      class="rounded-lg shadow border border-surface-300"
    >
      <Column field="warehouseName" header="Depo Adı" />
      <Column field="type" header="Adres Tipi" />
      <Column field="street" header="Sokak" />
      <Column field="city" header="Şehir" />
      <Column field="country" header="Ülke" />
      <Column field="zipCode" header="Posta Kodu" />

      <!-- İşlem Sütunu (İsteğe bağlı) -->
      <Column header="İşlem" headerClass="bg-surface-100 text-xs font-semibold text-center">
        <template #body="{ data, index }">
          <div class="flex justify-center gap-2">
            <router-link
              :to="{ name: 'update-warehouse', params: { warehouseId: company.data.warehouses[index].id } }"
              class="px-4 py-2 text-xs font-medium text-white bg-blue-600 hover:bg-blue-700 rounded-lg"
            >
              Düzenle
            </router-link>
            <button
              type="button"
              @click="warehouseStore.deleteWarehouse(company.data.warehouses[index].id)"
              class="px-4 py-2 text-xs font-medium text-white bg-red-600 hover:bg-red-700 rounded-lg"
            >
              Sil
            </button>
          </div>
        </template>
      </Column>
    </DataTable>
  </div>
</template>

<script setup>
import { storeToRefs } from "pinia";
import { useRoute } from "vue-router";
import { useCompanyStore } from "@/stores/company.store";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { DataTable, Column } from "primevue";

const { id } = useRoute().params;
const companyStore = useCompanyStore();
const warehouseStore = useWarehouseStore();

await companyStore.fetchCompanyWithInclude(id);

const { company } = storeToRefs(companyStore);

// Depolar ve Adresleri Listele
function flatWarehousesWithAddresses(warehouses) {
  return warehouses.flatMap((warehouse) => {
    return (warehouse.addresses || []).map((address) => ({
      warehouseName: warehouse.name,
      ...address,
    }));
  });
}
</script>

<style lang="scss" scoped></style>
