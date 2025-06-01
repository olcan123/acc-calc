<template>
  <BaseModalPersistent v-model="isVisible" size="lg">
    <template #header>
      <h3 class="text-lg font-semibold text-gray-900 dark:text-white">
        Satın Alma Faturası Bilgileri
      </h3>
    </template>

    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <FieldTextInput
        fieldName="purchaseInvoices[0].invoiceNo"
        labelName="Fatura No"
        placeholder="Fatura numarası girin"
      />

      <FieldDateInput
        fieldName="purchaseInvoices[0].invoiceDate"
        labelName="Fatura Tarihi"
      />
      <FieldSelect
        fieldName="purchaseInvoices[0].partnerId"
        labelName="Tedarikçi"
        :options="optionPartners"
        placeholder="Tedarikçi seçin"
      />
      <FieldSelect
        fieldName="purchaseInvoices[0].vendorAccountId"
        labelName="Tedarikçi Hesabı"
        :options="parentAccountFilterOptions(16)"
        placeholder="Hesap seçin"
      />

      <FieldSelect
        fieldName="purchaseInvoices[0].warehouseId"
        labelName="Depo"
        :options="optionWarehouses"
        placeholder="Depo seçin"
      />

      <!-- <FieldSelect fieldName="purchaseInvoices[0].currencyId" labelName="Para Birimi" :options="optionCurrencies"
                placeholder="Para birimi seçin" />

            <FieldNumberInput fieldName="purchaseInvoices[0].exchangeRate" labelName="Döviz Kuru" placeholder="1.00"
                step="0.0001" /> -->

      <!-- Ödeme Bilgileri - Tek Satır -->
      <div class="md:col-span-2 grid grid-cols-1 md:grid-cols-2 gap-4">
        <FieldCheckbox
          fieldName="purchaseInvoices[0].isPaid"
          labelName="Ödendi"
        />

        <FieldNumberInput
          v-show="values?.purchaseInvoices?.[0]?.isPaid"
          fieldName="purchaseInvoices[0].cashPaymentAmount"
          labelName="Nakit Ödeme Tutarı"
          placeholder="0.00"
          step="0.01"
        />
      </div>

      <div class="md:col-span-2">
        <FieldTextArea
          fieldName="purchaseInvoices[0].note"
          labelName="Not"
          placeholder="Fatura notu (opsiyonel)"
        />
      </div>
    </div>

    <template #footer>
      <button
        type="button"
        @click="closeModal"
        class="px-4 py-2 text-sm text-gray-800 bg-white border border-gray-300 rounded-lg hover:bg-gray-100 dark:bg-gray-800 dark:text-gray-300 dark:border-gray-600 dark:hover:bg-gray-700"
      >
        İptal
      </button>
      <button
        type="button"
        @click="saveModal"
        class="px-4 py-2 text-sm text-white bg-blue-600 rounded-lg hover:bg-blue-700 dark:bg-blue-500 dark:hover:bg-blue-600"
      >
        Kaydet
      </button>
    </template>
  </BaseModalPersistent>
</template>

<script setup>
import { computed, toRefs, watchEffect } from "vue";
import { storeToRefs } from "pinia";
import { useFormContext } from "vee-validate";
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

// Stores
const partnerStore = usePartnerStore();
const accountStore = useAccountStore();
const warehouseStore = useWarehouseStore();

// Store reactive data
const { optionPartners } = storeToRefs(partnerStore);
const { parentAccountFilterOptions } = storeToRefs(accountStore);
const { optionWarehouses } = storeToRefs(warehouseStore);

// Form context to get current values
const { values, setFieldValue } = useFormContext();

watchEffect(() => {
  setFieldValue(
    "purchaseInvoices[0].vendorAccountId",
    parentAccountFilterOptions.value(16)?.[0]?.value || null
  );
});

const props = defineProps({
  modelValue: {
    type: Boolean,
    default: false,
  },
});

const { modelValue } = toRefs(props);

const emit = defineEmits(["update:modelValue", "save"]);

const isVisible = computed({
  get: () => modelValue.value,
  set: (value) => emit("update:modelValue", value),
});

const closeModal = () => {
  isVisible.value = false;
};

const saveModal = () => {
  // Form verilerini emit et
  emit("save");
  isVisible.value = false;
};
</script>
