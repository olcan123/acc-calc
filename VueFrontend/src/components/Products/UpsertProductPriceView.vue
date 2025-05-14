<template>
    <div class="flex justify-between items-center mb-4">
        <h3 class="text-lg font-semibold text-gray-700 dark:text-gray-300">
            Fiyatlar
        </h3>
        <button type="button" @click="addPrice"
            class="text-sm text-white bg-green-600 hover:bg-green-700 px-3 py-1.5 rounded-md">
            + Fiyat Ekle
        </button>
    </div>
    <div v-for="(field, index) in fields" :key="field.key"
        class="mb-4 p-3 border border-gray-200 dark:border-gray-700 rounded-lg">
        <div class="flex flex-wrap md:flex-nowrap gap-4 items-start">
            <!-- Main price info -->
            <div class="w-full md:w-1/3">
                <FieldNumberInput :fieldName="`productPrices[${index}].unitPrice`" labelName="Birim Fiyat"
                    placeholderName="Fiyat giriniz" :min="0" step="0.01" />
            </div>

            <div class="w-full md:w-1/3">
                <FieldSelect :fieldName="`productPrices[${index}].category`" labelName="Fiyat Kategorisi"
                    placeholderName="Kategori seçiniz" :options="priceCategories" />
            </div>

            <div class="w-full md:w-1/3">
                <FieldSelect :fieldName="`productPrices[${index}].side`" labelName="Fiyat Yönü"
                    placeholderName="Yön seçiniz" :options="priceSides" />
            </div>
        </div>

        <!-- Promotional price dates -->
        <div v-if="false" class="flex flex-wrap md:flex-nowrap gap-4 mt-4 items-start">
            <div class="w-full md:w-1/2">
                <FieldDateInput :fieldName="`productPrices[${index}].validFrom`" labelName="Geçerlilik Başlangıç"
                    placeholderName="Başlangıç tarihi seçiniz" />
            </div>

            <div class="w-full md:w-1/2">
                <FieldDateInput :fieldName="`productPrices[${index}].validTo`" labelName="Geçerlilik Bitiş"
                    placeholderName="Bitiş tarihi seçiniz" />
            </div>
        </div>

        <!-- Delete Button -->
        <div class="flex justify-end mt-4">            <button @click="removePrice(index, field.value.id)" type="button"
                class="text-white bg-red-600 hover:bg-red-700 px-4 py-2 rounded-md font-medium">
                Fiyatı Sil
            </button>
        </div>
    </div>
</template>

<script setup>
import { useFieldArray } from "vee-validate";
import { useProductPriceStore } from "@/stores/product-price.store";
import FieldNumberInput from "@/components/Form/FieldNumberInput.vue";
import FieldSelect from "@/components/Form/FieldSelect.vue";
import FieldDateInput from "@/components/Form/FieldDateInput.vue";

const productPriceStore = useProductPriceStore();



// Price Array yönetimi
const { fields, push, remove } = useFieldArray("productPrices");

// Fiyat kategorileri ve yönleri
const priceCategories = [
    { value: 0, label: 'Standart' },
    { value: 1, label: 'Promosyon' }
];

const priceSides = [
    { value: 0, label: 'Alış' },
    { value: 1, label: 'Satış' }
];

// Yeni fiyat ekle
const addPrice = () => {
    push({
        unitPrice: 0,
        category: '',
        side: '',
        validFrom: null,
        validTo: null
    });
};

// Fiyat sil
const removePrice = async (index, id) => {
    if (id) {
        await productPriceStore.deleteProductPrice(id);
    }
    remove(index);
};
</script>
