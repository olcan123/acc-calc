<template>
  <div class="flex justify-between items-center mb-4">
    <h2 class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight">
      <span class="font-bold">{{ contactPartner.name }}</span> Ait Partner İletişim Tablosu
    </h2>
    <router-link
      :to="{ name: 'partner-contacts-create', params: { partnerId } }"
      class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
    >
      + Yeni İletişim Bilgisi Ekle
    </router-link>
  </div>

  <DataTable
    :value="contactPartner.contactPartners"
    v-model:expandedRows="expandedRows"
    @rowExpand="onRowExpand"
    @rowCollapse="onRowCollapse"
    responsiveLayout="scroll"
    dataKey="contact.id"
  >
    <template #empty> Bu partnera ait iletişim bilgisi bulunamadı. </template>

    <Column expander style="width: 5rem" />
    <Column header="ID">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>
    <Column field="contact.name" header="İletişim Adı" />

    <Column header="İşlemler" style="width: 180px">
      <template #body="slotProps">
        <div class="flex gap-2">
          <router-link
            :to="{
              name: 'partner-contacts-update',
              params: { partnerId, contactId: slotProps.data.contact.id },
            }"
            class="text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:focus:ring-yellow-900"
          >
            Düzenle
          </router-link>
          <button
            @click="confirmDeleteContact(partnerId, slotProps.data.contact.id)"
            type="button"
            class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900"
          >
            Sil
          </button>
        </div>
      </template>
    </Column>

    <template #expansion="slotProps">
      <DataTable
        :value="slotProps.data.contact.contactDetails"
        responsiveLayout="scroll"
        dataKey="id"
      >
        <h1 class="text-2xl font-bold text-gray-900 dark:text-white mb-4">
          {{ slotProps.data.contact.name }} Ait Detaylar
        </h1>

        <template #empty>
          <div class="text-center text-gray-400 text-lg py-6">
            Bu iletişime ait detay bilgisi bulunamadı.
          </div>
        </template>

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
        <Column header="İşlemler">
          <template #body="{ data }">
            <div class="flex gap-2">
              <button @click="confirmDeleteContactDetail(data.id)"
                class="text-white bg-red-600 hover:bg-red-700 rounded-lg text-sm px-3 py-1.5">
                Sil
              </button>
              <button @click="openEditModal(data)"
                class="text-white bg-yellow-400 hover:bg-yellow-500 rounded-lg text-sm px-3 py-1.5">
                Güncelle
              </button>
            </div>
          </template>
        </Column>
      </DataTable>
    </template>
  </DataTable>

  <ConfirmDialog />
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
      <button class="text-white bg-blue-700 hover:bg-blue-800 rounded-lg text-sm px-5 py-2.5"
        @click="handleUpdate">Güncelle</button>
      <button class="px-5 py-2.5 text-sm text-gray-800 bg-white border hover:bg-gray-100"
        @click="isModalOpen = false">İptal</button>
    </template>
  </BaseModal>
</template>

<script setup>
import { ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useForm } from "vee-validate";
import { storeToRefs } from "pinia";
import { DataTable, Column } from "primevue";
import { useContactPartnerStore } from "@/stores/contact-partner.store";
import { useConfirm } from "primevue/useconfirm";
import ConfirmDialog from "primevue/confirmdialog";
import BaseModal from "@/components/UI/Modal/BaseModal.vue";
import UpdateContactDetailModal from "@/components/Views/UpdateContactDetailModal.vue";

const expandedRows = ref({});
const isModalOpen = ref(false);
const onRowExpand = () => {};
const onRowCollapse = () => {};

const router = useRouter();
const { partnerId } = useRoute().params;

const contactPartnerStore = useContactPartnerStore();
const { contactPartner } = storeToRefs(contactPartnerStore);
await contactPartnerStore.fetchContactsByPartnerId(partnerId);

const confirm = useConfirm();

const confirmDeleteContact = (partnerId, contactId) => {
  confirm.require({
    message: "Bu iletişim bilgisini silmek istediğinize emin misiniz?",
    header: "Silme Onayı",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    acceptClass: "p-button-danger",
    rejectClass: "p-button-secondary",
    accept: async () => {
      await contactPartnerStore.deleteContact(partnerId, contactId);
    },
  });
};

const confirmDeleteContactDetail = (detailId) => {
  confirm.require({
    message: "Bu iletişim detayını silmek istediğinize emin misiniz?",
    header: "Silme Onayı",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    acceptClass: "p-button-danger",
    rejectClass: "p-button-secondary",
    accept: async () => {
      await contactPartnerStore.deleteContactDetail(detailId);
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

const handleUpdate = handleSubmit(async (values) => {
  await contactPartnerStore.updateContactDetail(values);
  isModalOpen.value = false;
});

const openEditModal = (detail) => {
  isModalOpen.value = true;
  setValues(detail);
};
</script>