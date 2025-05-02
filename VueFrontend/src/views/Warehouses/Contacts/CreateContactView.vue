<template>
  <!-- Üst Başlık ve Kaydet Butonu -->
  <div
    class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4"
  >
    <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
      Yeni İletişim Ekle
    </h2>

    <button
      type="submit"
      form="contactForm"
      :disabled="submitCount > 0 && !meta.valid"
      class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed"
    >
      <span v-if="submitCount > 0 && !meta.valid">🚫</span>
      Kaydet
    </button>
  </div>

  <ShowError :error />

  <!-- Form -->
  <form @submit="onSubmit" id="contactForm" class="space-y-6">
    <!-- Contact Name -->
    <FieldTextInput
      fieldName="contact.name"
      labelName="İletişim Adı"
      placeholderName="İletişim adı giriniz"
    />

    <!-- Contact Details -->
     <UpsertContactDetailView />
  </form>
</template>

<script setup>
import { useRoute } from "vue-router";
import { storeToRefs } from "pinia";
import { useForm } from "vee-validate";
import { toTypedSchema } from "@vee-validate/zod";
import { useContactWarehouseStore } from "@/stores/contact-warehouse.store";
import FieldTextInput from "@/components/Form/FieldTextInput.vue"; // Senin input componentin
import UpsertContactDetailView from "@/components/Views/UpsertContactDetailView.vue";
import ShowError from "@/components/UI/Error/ShowError.vue";
import contactSchema from "@/services/validations/contact-validation";

const { warehouseId } = useRoute().params;
const contactWarehouseStore = useContactWarehouseStore();
const { error } = storeToRefs(contactWarehouseStore);

// Form setup
const { handleSubmit, meta, submitCount } = useForm({
  validationSchema: toTypedSchema(contactSchema),
  initialValues: {
    contact: {
      name: "",
    },
    contactDetails: [
      {
        contactId: 0,
        name: "",
        value: "",
        description: "",
        isActive: false,
        isPrimary: false,
      },
    ],
  },
});

// Submit işlemi
const onSubmit = handleSubmit(async (values) => {
  // API'ye gönderilebilir burada
  await contactWarehouseStore.createContactWarehouse(warehouseId, values);
});
</script>

<style scoped></style>
