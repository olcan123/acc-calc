<template>
  <!-- Üst Başlık ve Kaydet Butonu -->
  <div class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4">
    <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
      Ölçü Birimi Düzenle
    </h2>

    <button
      type="submit"
      form="unitOfMeasureForm"
      :disabled="submitCount > 0 && !meta.valid"
      class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed"
    >
      <span v-if="submitCount > 0 && !meta.valid">🚫</span>
      Duzenle
    </button>
  </div>

  <!-- Form Grid -->
  <form
    @submit="onSubmit"
    id="unitOfMeasureForm"
    class="grid grid-cols-1 md:grid-cols-2 gap-6"
  >
    <FieldTextInput
      fieldName="name"
      labelName="Ad"
      placeholderName="Ölçü Birimi Adını Girin"
    />

    <FieldTextInput
      fieldName="abbreviation"
      labelName="Kısaltma"
      placeholderName="Kısaltmayı Girin (Örn: adet, kg)"
    />
  </form>
</template>

<script setup>
import { useForm } from "vee-validate";
import { toTypedSchema } from "@vee-validate/zod";
import { useRoute } from "vue-router";
import { storeToRefs } from "pinia";
import { useUnitOfMeasureStore } from "@/stores/unit-of-measure.store";
import { unitOfMeasureValidationSchema } from "@/services/validations/unit-of-measure-validation";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";

const { id } = useRoute().params;

const unitOfMeasureStore = useUnitOfMeasureStore();
const { unitOfMeasure } = storeToRefs(unitOfMeasureStore);
await unitOfMeasureStore.fetchUnitOfMeasure(id);

// Form setup
const { handleSubmit, meta, submitCount, setValues } = useForm({
  validationSchema: toTypedSchema(unitOfMeasureValidationSchema),
  initialValues: {
    id: 0,
    name: "",
    abbreviation: "",
  },
});

// Form'a mevcut değerleri yükle
setValues(unitOfMeasure.value);

// ✅ Form submit işlemi
const onSubmit = handleSubmit(async (values) => {
  values.id = Number(id);
  // API'ye gönder
  await unitOfMeasureStore.updateUnitOfMeasure(values);
});
</script>

<style scoped></style>
