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

  <DataTable :value="partners" dataKey="id" responsiveLayout="scroll">
    <!-- Sıra Numarası -->
    <Column header="#" style="width: 60px">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>

    <!-- Ana Bilgiler -->
    <Column field="name" header="Adı" />
    <Column field="tradeName" header="Ticari Ünvan" />

    <!-- Partner Tipi -->
    <Column field="partnerType" header="Partner Tipi">
      <template #body="{ data }">
        {{ getPartnerTypeLabel(data.partnerType) }}
      </template>
    </Column>

    <!-- İş Partner Tipi -->
    <Column field="businessPartnerType" header="İş Partner Tipi">
      <template #body="{ data }">
        {{ getBusinessPartnerTypeLabel(data.businessPartnerType) }}
      </template>
    </Column>

    <!-- Diğer Bilgiler -->
    <Column field="identityNumber" header="Kimlik No" />
    <Column field="vatNumber" header="Vergi No" />

    <!-- İşlem Butonları -->
    <Column header="İşlemler" style="width: 180px">
      <template #body="{ data }">
        <div class="flex gap-2">
          <!-- Düzenle Butonu -->
          <router-link
            :to="{ name: 'update-partner', params: { id: data.id } }"
            type="button"
            class="text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:focus:ring-yellow-900"
          >
            Düzenle
          </router-link>

          <!-- Sil Butonu -->
          <button
            @click="
              confirmDelete({
                id: data.id,
                addressId: data.addressPartners[0]?.addressId,
              })
            "
            type="button"
            class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900"
          >
            Sil
          </button>
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
import { useRouter } from "vue-router";
import { useConfirm } from "primevue/useconfirm";
import ConfirmDialog from "primevue/confirmdialog";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import { usePartnerStore } from "@/stores/partner.store";
import { storeToRefs } from "pinia";
import TableDropdownButton from "@/components/UI/Buttons/TableDropdownButton.vue";

// Ortak enum fonksiyonlarını import et
import {
  getPartnerTypeLabel,
  getBusinessPartnerTypeLabel,
} from "@/services/constants/partner-enum";

const router = useRouter();
const confirm = useConfirm();

const partnerStore = usePartnerStore();
await partnerStore.fetchPartners();
const { partners } = storeToRefs(partnerStore);

// Partner silme işlemi
const confirmDelete = (partner) => {
  confirm.require({
    message: "Bu partneri silmek istediğinize emin misiniz?",
    header: "Onay",
    icon: "pi pi-exclamation-triangle",
    acceptClass: "p-button-danger",
    accept: async () => {
      await partnerStore.deletePartner(partner.id, partner.addressId);
    },
  });
};
</script>
