<template>
  <!-- Üst Başlık ve Kaydet Butonu -->
  <div
    class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4"
  >
    <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
      Depo Bilgilerini Duzenle
    </h2>

    <button
      type="submit"
      form="warehouseForm"
      :disabled="submitCount > 0 && !meta.valid"
      class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed"
    >
      <span v-if="submitCount > 0 && !meta.valid">🚫</span>
      Duzenle
    </button>
  </div>

  <!-- Form -->

  <form @submit="onSubmit" id="warehouseForm" class="space-y-6">
    <!-- Tekil name alanı -->
    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
      <FieldTextInput
        fieldName="warehouse.name"
        labelName="Depo Adı"
        placeholderName="Depo adını giriniz"
      />

      <FieldSelect
        fieldName="warehouse.companyId"
        labelName="Sirket"
        placeholderName="Sirket seciniz"
        :options="optionCompanies"
      />
    </div>

    <!-- Diğer alanlar grid halinde -->
    <h1 class="text-3xl font-bold text-gray-900 dark:text-white">
      Adres Bilgileri
    </h1>
    <div
      class="grid grid-cols-1 md:grid-cols-2 gap-6"
      v-for="(field, index) in fields"
      :key="field.key"
    >
      <!-- Stret -->
      <FieldTextInput
        :fieldName="`addresses[${index}].street`"
        labelName="Sokak"
        placeholderName="Sokak bilgisi giriniz"
      />

      <!-- City -->
      <FieldTextInput
        :fieldName="`addresses[${index}].city`"
        labelName="Şehir"
        placeholderName="Şehir giriniz"
      />

      <!-- Country -->
      <FieldTextInput
        :fieldName="`addresses[${index}].country`"
        labelName="Ülke"
        placeholderName="Ülke giriniz"
      />

      <!-- ZipCode -->
      <FieldTextInput
        :fieldName="`addresses[${index}].zipCode`"
        labelName="Posta Kodu"
        placeholderName="Posta kodu giriniz"
      />
    </div>
  </form>
</template>

<script setup>
import { useForm, useFieldArray } from "vee-validate";
import { storeToRefs } from "pinia";
import { useRoute } from "vue-router";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { useCompanyStore } from "@/stores/company.store";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";
import FieldSelect from "@/components/Form/FieldSelect.vue";

const { id } = useRoute().params;

const warehouseStore = useWarehouseStore();
const companyStore = useCompanyStore();
const { optionCompanies } = storeToRefs(companyStore);
const { warehouse } = storeToRefs(warehouseStore);
await companyStore.fetchCompanies();
await warehouseStore.fetchIncludeWarehouse(id);

const { handleSubmit, meta, submitCount, setValues } = useForm({
  initialValues: {
    warehouse: {
      name: "",
      companyId: 0,
    },
    addresses: [
      {
        street: "",
        city: "",
        country: "",
        zipCode: "",
      },
    ],
  },
});

setValues({
  warehouse: {
    id: warehouse.value.id ?? 0,
    name: warehouse.value.name ?? "",
    companyId: warehouse.value.companyId ?? 0,
  },
  addresses: warehouse.value.addressWarehouses.map((aw) => ({
    ...aw.address,
  })),
});

const { fields } = useFieldArray("addresses");

const onSubmit = handleSubmit(async (values) => {
  await warehouseStore.updateWarehouse(values);
});
</script>
