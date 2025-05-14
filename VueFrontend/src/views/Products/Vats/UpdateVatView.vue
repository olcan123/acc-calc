<template>
    <!-- Üst Başlık ve Kaydet Butonu -->
    <div class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4">
        <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
            KDV Oranı Düzenle
        </h2>

        <button type="submit" form="vatForm" :disabled="submitCount > 0 && !meta.valid"
            class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed">
            <span v-if="submitCount > 0 && !meta.valid">🚫</span>
            Duzenle
        </button>
    </div>

    <!-- Form Grid -->
    <form @submit="onSubmit" id="vatForm" class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <FieldTextInput fieldName="name" labelName="Ad" placeholderName="KDV Oranı Adı Girin" />
        <FieldNumberInput fieldName="rate" labelName="Oran (%)" placeholderName="KDV Oranı Girin (Örn: 18)"
            step="0.01" />

        <div class="col-span-1">
            <FieldCheckbox fieldName="isDefault" labelName="Varsayılan" />
        </div>

        <div class="col-span-1">
            <FieldCheckbox fieldName="isActive" labelName="Aktif" />
        </div>
    </form>
</template>

<script setup>
import { useForm } from "vee-validate";
import { toTypedSchema } from "@vee-validate/zod";
import { useRoute } from "vue-router";
import { storeToRefs } from "pinia";
import { useVatStore } from "@/stores/vat.store";
import { vatValidationSchema } from "@/services/validations/vat-validation";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";
import FieldNumberInput from "@/components/Form/FieldNumberInput.vue";
import FieldCheckbox from "@/components/Form/FieldCheckbox.vue";

const { id } = useRoute().params;

const vatStore = useVatStore();
const { vat } = storeToRefs(vatStore);
await vatStore.fetchVat(id);

// Form setup
const { handleSubmit, meta, submitCount, setValues } = useForm({
    validationSchema: toTypedSchema(vatValidationSchema),
    initialValues: {
        id: 0,
        name: "",
        rate: 0,
        isDefault: false,
        isActive: true,
    },
});

// Form'a mevcut değerleri yükle
setValues(vat.value);

// ✅ Form submit işlemi
const onSubmit = handleSubmit(async (values) => {
    values.id = Number(id);
    // API'ye gönder
    await vatStore.updateVat(values);
});
</script>

<style scoped></style>
