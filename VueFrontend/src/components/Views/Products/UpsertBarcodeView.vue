<template>
  <div class="flex justify-between items-center mb-4">
    <h3 class="text-lg font-semibold text-gray-700 dark:text-gray-300">
      Barkodlar
    </h3>
    <button type="button" @click="addBarcode"
      class="text-sm text-white bg-green-600 hover:bg-green-700 px-3 py-1.5 rounded-md">
      + Barkod Ekle
    </button>
  </div>
  <div v-for="(field, index) in fields" :key="field.key"
    class="flex flex-wrap md:flex-nowrap items-end gap-4 mb-4 p-3 border border-gray-200 dark:border-gray-700 rounded-lg">
    <div class="w-full md:w-2/5">
      <FieldTextInput :fieldName="`barcodes[${index}].code`" labelName="Barkod" placeholderName="Barkod giriniz" />
    </div>
    <div class="w-full md:w-2/5">
      <FieldSelect :fieldName="`barcodes[${index}].type`" labelName="Barkod Tipi" placeholderName="Barkod tipi seçiniz"
        :options="barcodeTypes" />
    </div>
    <div class="w-full md:w-auto flex justify-end mt-2 md:mt-0">
      <button @click="removeBarcode(index,field.value.id)" type="button"
        class="w-full text-white bg-red-600 hover:bg-red-700 px-4 py-2 rounded-md font-medium">
        Sil
      </button>
    </div>
  </div>
</template>

<script setup>
import { useFieldArray } from "vee-validate";
import { useBarcodeStore } from "@/stores/barcode.store";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";
import FieldSelect from "@/components/Form/FieldSelect.vue";

const barcodeStore = useBarcodeStore();

// Barcodes Array yönetimi
const { fields, push, remove } = useFieldArray("barcodes");

// Barcode tipleri
const barcodeTypes = [
  { value: 'EAN-13', label: 'EAN-13' },
  { value: 'EAN-8', label: 'EAN-8' },
  { value: 'UPC', label: 'UPC' },
  { value: 'CODE39', label: 'CODE39' },
  { value: 'CODE128', label: 'CODE128' },
  { value: 'QR', label: 'QR Code' },
  { value: 'DATAMATRIX', label: 'Data Matrix' }
];

// Yeni Barcode eklemek
const addBarcode = () => {
  push({
    code: '',
    type: ''
  });
};

// Barcode silmek
const removeBarcode = async (index, id) => {
  if (id) {
    await barcodeStore.deleteBarcode(id);
  }
  remove(index);
};
</script>
