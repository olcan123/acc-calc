<template> <!-- Üst Başlık ve Kaydet Butonu -->
    <div class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4">
        <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
            Para Birimi Güncelle
        </h2>

        <button type="submit" form="currencyForm" :disabled="submitCount > 0 && !meta.valid"
            class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed">
            <span v-if="submitCount > 0 && !meta.valid">🚫</span>
            Güncelle
        </button>    </div>

    <!-- Form Grid -->
    <form @submit="onSubmit" id="currencyForm" class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <FieldTextInput fieldName="code" labelName="Para Birimi Kodu" placeholderName="Örn: USD, EUR, TRY" required />

        <FieldTextInput fieldName="name" labelName="Para Birimi Adı"
            placeholderName="Örn: Amerikan Doları, Euro, Türk Lirası" required />
    </form>
</template>

<script setup>
import { useForm } from "vee-validate";
import { toTypedSchema } from "@vee-validate/zod";
import { useRoute } from "vue-router";
import { storeToRefs } from "pinia";
import { useCurrencyStore } from "@/stores/currency.store";
import { currencyValidationSchema } from "@/services/validations/currency-validation";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";

const route = useRoute();
const { id } = route.params;

const currencyStore = useCurrencyStore();
const { currency, loading } = storeToRefs(currencyStore);

// Form setup
const { handleSubmit, meta, submitCount, setValues } = useForm({
    validationSchema: toTypedSchema(currencyValidationSchema),
    initialValues: {
        id: 0,
        code: "",
        name: ""
    },
});

// Form submission
const onSubmit = handleSubmit(async (values) => {
    values.id = parseInt(id);
    await currencyStore.updateCurrency(values);
});


await currencyStore.fetchCurrency(id);
setValues(currency.value);
</script>
