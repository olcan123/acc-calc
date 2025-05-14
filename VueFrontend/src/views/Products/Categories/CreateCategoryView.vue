<template>
    <!-- Üst Başlık ve Kaydet Butonu -->
    <div class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4">
        <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
            Yeni Kategori Ekle
        </h2>

        <button type="submit" form="categoryForm" :disabled="submitCount > 0 && !meta.valid"
            class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed">
            <span v-if="submitCount > 0 && !meta.valid">🚫</span>
            Kaydet
        </button>
    </div>

    <!-- Form Grid -->
    <form @submit="onSubmit" id="categoryForm" class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <FieldTextInput fieldName="name" labelName="Kategori Adı" placeholderName="Kategori Adı Girin" />
    </form>
</template>

<script setup>
import { useForm } from "vee-validate";
import { toTypedSchema } from "@vee-validate/zod";
import { useCategoryStore } from "@/stores/category.store";
import { categoryValidationSchema } from "@/services/validations/category-validation";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";

const categoryStore = useCategoryStore();

// Form setup
const { handleSubmit, meta, submitCount } = useForm({
    validationSchema: toTypedSchema(categoryValidationSchema),
    initialValues: {
        name: "",
    },
});

// ✅ Form submit işlemi
const onSubmit = handleSubmit(async (values) => {
    // API'ye gönder
    await categoryStore.createCategory(values);
});
</script>

<style scoped></style>
