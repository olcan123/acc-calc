<template>
    <!-- Başlık ve Kaydet Butonu -->
    <div class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4">
      <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
        Yeni Banka Ekle
      </h2>
  
      <button
        type="submit"
        form="bankForm"
        :disabled="submitCount > 0 && !meta.valid"
        class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed"
      >
        <span v-if="submitCount > 0 && !meta.valid">🚫</span>
        Kaydet
      </button>
    </div>
  
    <!-- Form Alanı -->
    <form @submit="onSubmit" id="bankForm" class="grid grid-cols-1 md:grid-cols-2 gap-6">
      <FieldTextInput
        fieldName="name"
        labelName="Banka Adı"
        placeholderName="Banka adı giriniz"
      />
    </form>
  </template>
  
  <script setup>
  import { useForm } from "vee-validate";
  import { toTypedSchema } from "@vee-validate/zod";
  import { useBankStore } from "@/stores/bank.store";
  import FieldTextInput from "@/components/Form/FieldTextInput.vue";
  import { z } from "zod";
  
  // ✅ Validation şeması
  const bankValidationSchema = z.object({
    name: z
      .string()
      .nonempty("Banka adı boş olamaz.")
      .max(150, "Banka adı en fazla 150 karakter olabilir."),
  });
  
  // ✅ Store
  const bankStore = useBankStore();
  
  // ✅ Form tanımı
  const { handleSubmit, meta, submitCount } = useForm({
    validationSchema: toTypedSchema(bankValidationSchema),
    initialValues: {
      name: "",
    },
  });
  
  // ✅ Gönderim işlemi
  const onSubmit = handleSubmit(async (values) => {
    await bankStore.createBank(values);
  });
  </script>
  
  <style scoped></style>
  