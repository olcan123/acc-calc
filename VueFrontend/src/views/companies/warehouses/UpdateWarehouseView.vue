<template>
  <form class="w-full min-h-screen mx-auto bg-white dark:bg-gray-900 space-y-6 px-4 py-6" @submit.prevent="onSubmit">
    <!-- Başlık ve Kaydet -->
    <div class="flex flex-col md:flex-row items-start md:items-center justify-between mb-6">
      <h1 class="text-2xl font-bold text-gray-900 dark:text-white">Depo Bilgilerini Güncelle</h1>
      <button
        type="submit"
        class="mt-4 md:mt-0 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-semibold rounded-lg text-sm px-6 py-2.5 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
      >
        Duzenle
      </button>
    </div>

    <!-- Depo Adı -->
    <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
      <FieldTextInput fieldName="name" labelName="Depo Adı" placeholderName="Örn. Merkez Depo" />
    </div>

    <!-- Adres Başlık -->
    <h2 class="text-xl font-bold text-gray-900 dark:text-white pt-4">📍 Adresler</h2>

    <!-- Adresler -->
    <div
      v-for="(address, index) in addressFields"
      :key="index"
      class="grid grid-cols-1 md:grid-cols-2 gap-6 border border-gray-200 dark:border-gray-700 p-4 rounded-lg"
    >
      <FieldTextInput :fieldName="`addresses[${index}].street`" labelName="Sokak" placeholderName="Sokak adı" />
      <FieldTextInput :fieldName="`addresses[${index}].type`" labelName="Adres Tipi" placeholderName="Ev, Ofis..." />
      <FieldTextInput :fieldName="`addresses[${index}].city`" labelName="Şehir" placeholderName="İstanbul" />
      <FieldTextInput :fieldName="`addresses[${index}].country`" labelName="Ülke" placeholderName="Türkiye" />
      <FieldTextInput :fieldName="`addresses[${index}].zipCode`" labelName="Posta Kodu" placeholderName="34000" />
    </div>
  </form>
</template>

<script setup>
import { useForm, useFieldArray } from "vee-validate";
import { useRoute } from "vue-router";
import { storeToRefs } from "pinia";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";
import { useWarehouseStore } from "@/stores/warehouse.store";

// 🔁 Route Params
const { warehouseId } = useRoute().params;

// 🔁 Store & Fetch
const warehouseStore = useWarehouseStore();
const { warehouse } = storeToRefs(warehouseStore);

await warehouseStore.fetchWarehouse(warehouseId);

// 🧩 useForm setup
const { handleSubmit, setValues } = useForm({
  initialValues: {
    name: "",
    addresses: [
      {
        street: "",
        type: "",
        city: "",
        country: "",
        zipCode: "",
      },
    ],
  },
});

// 📥 Gelen warehouse verisini forma yerleştir
if (warehouse.value) {
  setValues(warehouse.value);
}

// 🧩 useFieldArray
const { fields: addressFields } = useFieldArray("addresses");

// 📤 Submit
const onSubmit = handleSubmit(async (formValues) => {
  await warehouseStore.updateWarehouse(formValues);
});
</script>
