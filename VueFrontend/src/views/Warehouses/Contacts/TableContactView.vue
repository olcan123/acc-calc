<template>
  <!-- SECTION: Header -->
  <div class="flex justify-between items-center mb-4">
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      <span class="font-bold">{{ contactWarehouse.name }}</span> Ait Depo
      İletişim Tablosu
    </h2>
    <router-link
      :to="{ name: 'warehouse-contacts-create', params: { warehouseId } }"
      type="button"
      class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
    >
      + Yeni İletişim Bilgisi Ekle
    </router-link>
  </div>

  <!-- SECTION: Main DataTable -->
  <DataTable
    :value="contactWarehouse.contactWarehouses"
    v-model:expandedRows="expandedRows"
    @rowExpand="onRowExpand"
    @rowCollapse="onRowCollapse"
    responsiveLayout="scroll"
    dataKey="contact.id"
  >
    <!-- Empty State -->
    <template #empty> Bu depoya ait iletişim bilgisi bulunamadı. </template>

    <!-- Main Columns -->
    <Column expander style="width: 5rem" />
    <Column header="ID">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>
    <Column field="contact.name" header="İletişim Adı" />

    <!-- Actions Column -->
    <Column header="İşlemler" style="width: 180px">
      <template #body="slotProps">
        <div class="flex gap-2">
          <router-link
            :to="{
              name: 'warehouse-contacts-update',
              params: { warehouseId, contactId: slotProps.data.contact.id },
            }"
            class="text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:focus:ring-yellow-900"
          >
            Düzenle
          </router-link>
          <button
            @click="
              confirmDeleteContact(warehouseId, slotProps.data.contact.id)
            "
            type="button"
            class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900"
          >
            Sil
          </button>
        </div>
      </template>
    </Column>

    <!-- SECTION: Expansion Table -->
    <template #expansion="slotProps">
      <DataTable
        :value="slotProps.data.contact.contactDetails"
        responsiveLayout="scroll"
        dataKey="id"
      >
        <h1 class="text-2xl font-bold text-gray-900 dark:text-white mb-4">
          {{ slotProps.data.contact.name }} Ait Detaylar
        </h1>

        <!-- Empty Details -->
        <template #empty>
          <div class="text-center text-gray-400 text-lg py-6">
            Bu iletişime ait detay bilgisi bulunamadı.
          </div>
        </template>

        <!-- Detail Columns -->
        <Column header="ID">
          <template #body="{ index }">
            {{ index + 1 }}
          </template>
        </Column>
        <Column field="name" header="Tip" />
        <Column field="value" header="Değer" />
        <Column field="description" header="Açıklama" />
        <Column header="Aktif">
          <template #body="{ data }">
            {{ data.isActive ? "Evet" : "Hayır" }}
          </template>
        </Column>
        <Column header="Primary">
          <template #body="{ data }">
            {{ data.isPrimary ? "Evet" : "Hayır" }}
          </template>
        </Column>

        <!-- Detail Actions -->
        <Column header="İşlemler">
          <template #body="{ data }">
            <div class="flex gap-2">
              <button
                @click="confirmDeleteContactDetail(data.id)"
                type="button"
                class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900"
              >
                Sil
              </button>
              <button
                @click="openEditModal(data)"
                type="button"
                class="text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:focus:ring-yellow-900"
              >
                Güncelle
              </button>
            </div>
          </template>
        </Column>
      </DataTable>
    </template>
  </DataTable>

  <!-- SECTION: Confirm Dialog -->
  <ConfirmDialog />

  <!-- SECTION: Modal -->
  <BaseModal v-model="isModalOpen" :staticBackdrop="true">
    <template #header>
      <h3 class="text-xl font-semibold text-gray-900 dark:text-white">
        İletişim Detayı Güncelle
      </h3>
    </template>

    <template #default>
      <UpdateContactDetailModal />
    </template>

    <template #footer>
      <button
        class="text-white bg-blue-700 hover:bg-blue-800 rounded-lg text-sm px-5 py-2.5"
        @click="handleUpdate"
      >
        Güncelle
      </button>
      <button
        class="px-5 py-2.5 text-sm text-gray-800 bg-white border border-gray-300 hover:bg-gray-100 dark:bg-gray-800 dark:text-gray-300 dark:border-gray-600 dark:hover:bg-gray-700"
        @click="isModalOpen = false"
      >
        İptal
      </button>
    </template>
  </BaseModal>
</template>

<script setup>
// SECTION: Imports
import { ref } from "vue";
import { DataTable, Column } from "primevue";
import { useRouter, useRoute } from "vue-router";
import { useForm } from "vee-validate";
import { storeToRefs } from "pinia";
import { useContactWarehouseStore } from "@/stores/contact-warehouse.store";
import { useConfirm } from "primevue/useconfirm";
import ConfirmDialog from "primevue/confirmdialog";
import BaseModal from "@/components/UI/Modal/BaseModal.vue";
import UpdateContactDetailModal from "@/components/Views/UpdateContactDetailModal.vue";

// SECTION: State
const expandedRows = ref({});
const isModalOpen = ref(false);

// Optional: Row expand/collapse handlers
const onRowExpand = (event) => {};
const onRowCollapse = (event) => {};

// SECTION: Router
const router = useRouter();
const { warehouseId } = useRoute().params;

// SECTION: Store
const contactWarehouseStore = useContactWarehouseStore();
const { contactWarehouse } = storeToRefs(contactWarehouseStore);
await contactWarehouseStore.fetchContactsByWarehouseId(warehouseId);

// SECTION: Confirm Dialog
const confirm = useConfirm();

// TODO: Confirm Delete Contact
const confirmDeleteContact = (warehouseId, contactId) => {
  confirm.require({
    message: "Bu iletişim bilgisini silmek istediğinize emin misiniz?",
    header: "Silme Onayı",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    acceptClass: "p-button-danger",
    rejectClass: "p-button-secondary",
    accept: async () => {
      await contactWarehouseStore.deleteContact(warehouseId, contactId);
    },
    reject: () => {
      console.log("Silme iptal edildi.");
    },
  });
};

// TODO: Confirm Delete Contact Detail
const confirmDeleteContactDetail = (detailId) => {
  confirm.require({
    message: "Bu iletişim detayını silmek istediğinize emin misiniz?",
    header: "Silme Onayı",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    acceptClass: "p-button-danger",
    rejectClass: "p-button-secondary",
    accept: async () => {
      await contactWarehouseStore.deleteContactDetail(detailId);
    },
    reject: () => {
      console.log("Silme iptal edildi");
    },
  });
};

const { handleSubmit, setValues } = useForm({
  initialValues: {
    name: "",
    value: "",
    description: "",
    isActive: false,
    isPrimary: false,
  },
});

// TODO: Handle Update
const handleUpdate = handleSubmit(async (values) => {
  await contactWarehouseStore.updateContactDetail(values);
  isModalOpen.value = false;
});

// TODO: Open Edit Modal
const openEditModal = (detail) => {
  isModalOpen.value = true;
  setValues(detail);
};

</script>

<style scoped></style>
