<template>
    <!-- Başlık ve Kaydet Butonu -->
    <div class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4">
        <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
            Ürün Güncelle
        </h2>

        <button type="submit" form="productForm" :disabled="submitCount > 0 && !meta.valid"
            class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-blue-300 disabled:cursor-not-allowed">
            <span v-if="submitCount > 0 && !meta.valid">🚫</span>
            <span>Güncelle</span>
        </button>
    </div>

    <!-- Form -->
    <form @submit="onSubmit" id="productForm" class="space-y-6">
        <!-- Temel Bilgiler -->
        <div class="border-b pb-3">
            <h3 class="text-lg font-semibold text-gray-700 dark:text-white mb-3">
                Temel Bilgiler
            </h3>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                <!-- Ürün Adı -->
                <FieldTextInput fieldName="product.name" labelName="Ürün Adı" placeholderName="Ürün adını giriniz" />

                <!-- Açıklama -->
                <FieldTextArea fieldName="product.description" labelName="Açıklama"
                    placeholderName="Ürün açıklamasını giriniz" />

                <!-- Ölçü Birimi -->
                <FieldSelect fieldName="product.unitOfMeasureId" labelName="Ölçü Birimi"
                    placeholderName="Ölçü birimi seçiniz" :options="unitOfMeasures" />

                <!-- KDV Oranı -->
                <FieldSelect fieldName="product.vatId" labelName="KDV Oranı" placeholderName="KDV oranı seçiniz"
                    :options="vats" />

                <!-- Gümrük Vergisi Oranı -->
                <FieldNumberInput fieldName="product.customsTaxRate" labelName="Gümrük Vergisi Oranı (%)"
                    placeholderName="Gümrük vergisi oranı giriniz" :min="0" :max="100" step="0.01" />

                <!-- ÖTV Oranı -->
                <FieldNumberInput fieldName="product.exciseTaxRate" labelName="ÖTV Oranı (%)"
                    placeholderName="ÖTV oranı giriniz" :min="0" :max="100" step="0.01" />
            </div>
        </div>

        <!-- Barkodlar -->
        <div class="border-b pb-3">
            <UpsertBarcodeView />
        </div>

        <!-- Kategoriler -->
        <div class="border-b pb-3">
            <UpsertProductCategoryView />
        </div>

        <!-- Fiyatlar -->
        <div class="border-b pb-3">
            <UpsertProductPriceView />
        </div>
    </form>
</template>

<script setup>
import { useRoute, useRouter } from 'vue-router';
import { useForm } from "vee-validate";
import { storeToRefs } from "pinia";
import { useProductStore } from '@/stores/product.store';
import { useCategoryStore } from '@/stores/category.store';
import { useVatStore } from '@/stores/vat.store';
import { useUnitOfMeasureStore } from '@/stores/unit-of-measure.store';
import FieldTextInput from "@/components/Form/FieldTextInput.vue";
import FieldTextArea from "@/components/Form/FieldTextArea.vue";
import FieldSelect from "@/components/Form/FieldSelect.vue";
import FieldNumberInput from "@/components/Form/FieldNumberInput.vue";
import UpsertBarcodeView from "@/components/Products/UpsertBarcodeView.vue";
import UpsertProductCategoryView from "@/components/Products/UpsertProductCategoryView.vue";
import UpsertProductPriceView from "@/components/Products/UpsertProductPriceView.vue";

const route = useRoute();
const productId = Number(route.params.id);

const productStore = useProductStore();
const categoryStore = useCategoryStore();
const vatStore = useVatStore();
const unitOfMeasureStore = useUnitOfMeasureStore();

const { fetchProduct, updateProduct } = productStore;
const { optionVats: vats } = storeToRefs(vatStore);
const { optionUnitOfMeasures: unitOfMeasures } = storeToRefs(unitOfMeasureStore);
const { product: productData } = storeToRefs(productStore);

//Fetch data for setValues
await Promise.all([
    fetchProduct(productId),
    categoryStore.fetchCategories(),
    vatStore.fetchVats(),
    unitOfMeasureStore.fetchUnitOfMeasures()
]);





// Form setup with validation
const { handleSubmit, values, meta, submitCount, setValues } = useForm({
    initialValues: {
        product: {
            id: productId,
            name: '',
            description: '',
            customsTaxRate: 0,
            exciseTaxRate: 0,
            vatId: '',
            unitOfMeasureId: '',
        },
        barcodes: [],
        productPrices: [],
        productCategories: [],
    }
});

setValues({
    product: {
        id: productData.value.id,
        name: productData.value.name,
        description: productData.value.description,
        customsTaxRate: productData.value.customsTaxRate,
        exciseTaxRate: productData.value.exciseTaxRate,
        vatId: productData.value.vatId,
        unitOfMeasureId: productData.value.unitOfMeasureId,
    },
    barcodes: productData.value.barcodes || [],
    productPrices: productData.value.productPrices || [],
    productCategories: productData.value.productCategories || [],
});

// Form submission
const onSubmit = handleSubmit(async (formValues) => {
    //formValues.productCategories.category remove
    formValues.productCategories?.forEach((item) => {
        delete item.category;
    });
    await updateProduct(formValues);


});
</script>
