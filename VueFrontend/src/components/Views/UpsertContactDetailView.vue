<template>
  <div class="flex justify-between items-center">
    <h3 class="text-lg font-semibold text-gray-700 dark:text-gray-300">
      İletişim Detayları
    </h3>
    <button
      type="button"
      @click="addContactDetail"
      class="text-sm text-white bg-green-600 hover:bg-green-700 px-3 py-1.5 rounded-md"
    >
      + Yeni Bilgi
    </button>
  </div>
  <div
    v-for="(field, index) in fields"
    :key="field.key"
    class="flex flex-wrap md:flex-nowrap items-center gap-4"
  >
    <!-- Name -->
    <div class="w-full md:w-1/4">
      <FieldTextInput
        :fieldName="`contactDetails[${index}].name`"
        labelName="Bilgi Adı"
        placeholderName="Örn. Telefon, Email"
      />
    </div>

    <!-- Value -->
    <div class="w-full md:w-1/4">
      <FieldTextInput
        :fieldName="`contactDetails[${index}].value`"
        labelName="Değer"
        placeholderName="Örn: +90 555 555 55 55"
      />
    </div>

    <!-- Description -->
    <div class="w-full md:w-1/4">
      <FieldTextInput
        :fieldName="`contactDetails[${index}].description`"
        labelName="Açıklama"
        placeholderName="Opsiyonel açıklama"
      />
    </div>

    <!-- Checkboxlar -->
    <div class="flex items-center gap-4 w-full md:w-1/4">
      <FieldCheckbox
        :field-name="`contactDetails[${index}].isActive`"
        label-name="Aktif"
      />

      <FieldCheckbox
        :field-name="`contactDetails[${index}].isPrimary`"
        label-name="Birincil"
      />
    </div>

    <!-- Sil Butonu -->
    <div class="w-full md:w-auto flex justify-end">
      <button
        type="button"
        @click="removeContactDetail(field.value.id, index)"
        class="w-full text-white bg-red-600 hover:bg-red-700 px-4 py-2 rounded-md font-medium"
      >
        Sil
      </button>
    </div>
  </div>
</template>

<script setup>
import { useFieldArray } from "vee-validate";
import { useContactWarehouseStore } from "@/stores/contact-warehouse.store";
import FieldTextInput from "@/components/Form/FieldTextInput.vue"; // Senin input componentin
import FieldCheckbox from "@/components/Form/FieldCheckbox.vue";

const contactWarehouseStore = useContactWarehouseStore();

// ContactDetails Array yönetimi
const { fields, push, remove, replace } = useFieldArray("contactDetails");

// Yeni ContactDetail eklemek
const addContactDetail = () => {
  push({
    id: 0,
    contactId: 0,
    name: "",
    value: "",
    description: "",
    isActive: false,
    isPrimary: false,
  });
};

const removeContactDetail = async (detailId, index) => {
  if (detailId) {
    console.log(detailId);
    await contactWarehouseStore.deleteContactDetail(detailId);
  }
  remove(index);
};
</script>

<style scoped>
/* Your styles here */
</style>
