<template>
  <BaseModalPersistent v-model="modalStore.isInvoiceModalOpen" size="lg">
    <template #header>
      <h3 class="text-lg font-semibold text-gray-900 dark:text-white">
        Satış Faturası Bilgileri
      </h3>
    </template>
    <div class="max-h-[70vh] overflow-y-auto px-1">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <FieldTextInput
          fieldName="saleInvoices[0].invoiceNo"
          labelName="Fatura No"
          placeholder="Fatura numarası girin"
        />

        <FieldDateInput
          fieldName="saleInvoices[0].invoiceDate"
          labelName="Fatura Tarihi"
        />

        <FieldSelect
          fieldName="saleInvoices[0].partnerId"
          labelName="Müşteri"
          :options="optionPartners"
          placeholder="Müşteri seçin"
        />

        <FieldSelect
          fieldName="saleInvoices[0].customerAccountId"
          labelName="Müşteri Hesabı"
          :options="optionAccountsSartsWithCode(['1100.'])"
          placeholder="Hesap seçin"
        />
        <FieldSelect
          fieldName="saleInvoices[0].warehouseId"
          labelName="Depo"
          :options="optionWarehouses"
          placeholder="Depo seçin"
        />

        <!-- Currency as label (not editable) -->
        <div class="space-y-2">
          <label
            class="block text-sm font-medium text-gray-700 dark:text-gray-300"
          >
            Para Birimi
          </label>
          <div
            class="px-3 py-2 bg-gray-50 dark:bg-gray-700 border border-gray-300 dark:border-gray-600 rounded-lg text-sm text-gray-900 dark:text-white"
          >
            EUR - Euro
          </div>
        </div>

        <!-- Payment Status and Sale Type -->
        <div class="md:col-span-2 grid grid-cols-1 md:grid-cols-2 gap-4">
          <FieldCheckbox
            fieldName="saleInvoices[0].isPaid"
            labelName="Ödendi"
          />

          <FieldCheckbox
            fieldName="saleInvoices[0].isWholeSale"
            labelName="Toptan Satış"
          />
        </div>

        <!-- Cash Payment Amount -->
        <FieldNumberInput
          v-show="formValues?.saleInvoices?.[0]?.isPaid"
          fieldName="saleInvoices[0].cashPaymentAmount"
          labelName="Nakit Ödeme Tutarı"
          placeholder="0.00"
          step="0.01"
        />

        <!-- Note -->
        <div class="md:col-span-2">
          <FieldTextArea
            fieldName="saleInvoices[0].note"
            labelName="Not"
            placeholder="Fatura notu (opsiyonel)"
          />
        </div>
      </div>
    </div>    <template #footer>
      <button
        type="button"
        @click="cancelInvoice"
        class="px-4 py-2 text-sm text-gray-800 bg-white border border-gray-300 rounded-lg hover:bg-gray-100 dark:bg-gray-800 dark:text-gray-300 dark:border-gray-600 dark:hover:bg-gray-700"
      >
        İptal
      </button>
      <button
        type="button"
        @click="saveInvoiceData"
        class="px-4 py-2 text-sm text-white bg-blue-600 rounded-lg hover:bg-blue-700 dark:bg-blue-500 dark:hover:bg-blue-600"
      >
        Kaydet
      </button>
    </template>
  </BaseModalPersistent>
</template>

<script setup>
import { computed, watchEffect } from "vue";
import { storeToRefs } from "pinia";
import { useFormContext } from "vee-validate";
import { useRouter } from "vue-router";
import BaseModalPersistent from "@/components/UI/Modal/BaseModalPersistent.vue";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";
import FieldTextArea from "@/components/Form/FieldTextArea.vue";
import FieldDateInput from "@/components/Form/FieldDateInput.vue";
import FieldNumberInput from "@/components/Form/FieldNumberInput.vue";
import FieldSelect from "@/components/Form/FieldSelect.vue";
import FieldCheckbox from "@/components/Form/FieldCheckbox.vue";
import { usePartnerStore } from "@/stores/partner.store";
import { useAccountStore } from "@/stores/account.store";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { useModalStore } from "@/stores/modal.store";

// Get form context from vee-validate
const { values: formValues, setFieldValue } = useFormContext();

// Router
const router = useRouter();

// Stores
const partnerStore = usePartnerStore();
const accountStore = useAccountStore();
const warehouseStore = useWarehouseStore();
const modalStore = useModalStore();

const { optionPartners } = storeToRefs(partnerStore);
const { optionAccountsSartsWithCode } = storeToRefs(accountStore);
const { optionWarehouses } = storeToRefs(warehouseStore);

// Helper function to get default customer account ID
const getDefaultCustomerAccountId = () => {
  return optionAccountsSartsWithCode.value(['1100.'])?.[0]?.value || null;
};

// Set default customer account ID for main sale invoice
watchEffect(() => {
  if (!formValues?.saleInvoices?.[0]?.customerAccountId) {
    setFieldValue("saleInvoices[0].customerAccountId", getDefaultCustomerAccountId());
  }
});

const saveInvoiceData = () => {
  modalStore.closeInvoiceModal();
};

const cancelInvoice = () => {
  modalStore.closeInvoiceModal();
  router.push({ name: 'table-sale' });
};
</script>
