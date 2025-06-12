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
    v-model:filters="filters"
    :value="contactWarehouse.contactWarehouses"
    v-model:expandedRows="expandedRows"
    @rowExpand="onRowExpand"
    @rowCollapse="onRowCollapse"
    responsiveLayout="scroll"
    dataKey="contact.id"
    paginator 
    :rows="10" 
    filterDisplay="menu"
    :loading="loading"
    sortMode="multiple"
    :globalFilterFields="['contact.name']"
    :emptyMessage="contactWarehouse.contactWarehouses?.length === 0 ? 'Bu depoya ait iletişim bilgisi bulunamadı.' : 'Arama kriterlerinize uygun iletişim bilgisi bulunamadı.'"
  >
    <template #header>
      <div class="flex flex-col md:flex-row md:justify-between md:items-center gap-4 p-4 bg-gray-50 dark:bg-gray-800 rounded-lg">
        <div class="flex items-center gap-2">
          <button
            @click="clearFilter()"
            class="flex items-center gap-2 px-3 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-gray-300 dark:hover:bg-gray-600"
          >
            <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path>
            </svg>
            Filtreleri Temizle
          </button>
        </div>
        <div class="relative">
          <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
            <svg class="w-4 h-4 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path>
            </svg>
          </div>
          <input
            v-model="filters['global'].value"
            type="text"
            placeholder="Genel arama..."
            class="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-md leading-5 bg-white placeholder-gray-500 focus:outline-none focus:placeholder-gray-400 focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
          />
        </div>
      </div>
    </template>

    <template #loading>
      <div class="flex items-center justify-center p-8">
        <div class="flex items-center space-x-2">
          <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-blue-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
            <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
            <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
          <span class="text-gray-600 dark:text-gray-400">İletişim bilgileri yükleniyor...</span>
        </div>
      </div>
    </template>

    <!-- Empty State -->
    <template #empty> 
      <div class="flex flex-col items-center justify-center p-8 text-gray-500 dark:text-gray-400">
        <svg class="w-16 h-16 mb-4 text-gray-300 dark:text-gray-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1" d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z"></path>
        </svg>
        <p class="text-lg font-medium mb-1">{{ contactWarehouse.contactWarehouses?.length === 0 ? 'Bu depoya ait iletişim bilgisi bulunamadı' : 'Sonuç bulunamadı' }}</p>
        <p class="text-sm">{{ contactWarehouse.contactWarehouses?.length === 0 ? 'İlk iletişim bilgisini oluşturmak için yukarıdaki butonu kullanın.' : 'Arama kriterlerinizi değiştirmeyi deneyin.' }}</p>
      </div> 
    </template>

    <!-- Main Columns -->
    <Column expander style="width: 5rem" />
    <Column header="ID" style="min-width: 80px">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>
    <Column field="contact.name" header="İletişim Adı" sortable style="min-width: 200px">
      <template #filter="{ filterModel }">
        <input
          v-model="filterModel.value"
          type="text"
          placeholder="İletişim adı ara..."
          class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
        />
      </template>
    </Column>    <!-- Actions Column -->
    <Column header="İşlemler" style="min-width: 150px">
      <template #body="slotProps">
        <div class="flex gap-2">
          <EditLinkButton 
            :to="{
              name: 'warehouse-contacts-update',
              params: { warehouseId, contactId: slotProps.data.contact.id },
            }"
          />
          <DeleteButton @click="confirmDeleteContact(warehouseId, slotProps.data.contact.id)" />
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
import { FilterMatchMode, FilterOperator } from "@primevue/core/api";
import { useRouter, useRoute } from "vue-router";
import { useForm } from "vee-validate";
import { storeToRefs } from "pinia";
import { useContactWarehouseStore } from "@/stores/contact-warehouse.store";
import { useConfirm } from "primevue/useconfirm";
import EditLinkButton from "@/components/UI/Buttons/EditLinkButton.vue";
import DeleteButton from "@/components/UI/Buttons/DeleteButton.vue";
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
const { contactWarehouse, loading } = storeToRefs(contactWarehouseStore);
await contactWarehouseStore.fetchContactsByWarehouseId(warehouseId);

// SECTION: Confirm Dialog
const confirm = useConfirm();

// Filters
const filters = ref();

const initFilters = () => {
  filters.value = {
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
    'contact.name': { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] }
  };
};

initFilters();

const clearFilter = () => {
  initFilters();
};

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
