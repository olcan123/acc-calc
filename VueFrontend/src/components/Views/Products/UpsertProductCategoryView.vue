<template>
  <div class="flex justify-between items-center mb-4">
    <h3 class="text-lg font-semibold text-gray-700 dark:text-gray-300">
      Kategoriler
    </h3>
    <button type="button" @click="addCategory"
      class="text-sm text-white bg-green-600 hover:bg-green-700 px-3 py-1.5 rounded-md"
      :disabled="availableCategories.length === 0">
      + Kategori Ekle
    </button>
  </div>
  <div v-for="(field, index) in fields" :key="field.key"
    class="flex flex-wrap md:flex-nowrap items-end gap-4 mb-4 p-3 border border-gray-200 dark:border-gray-700 rounded-lg">
    <div class="w-full md:w-4/5">
      <FieldSelect :fieldName="`productCategories[${index}].categoryId`" labelName="Kategori"
        placeholderName="Kategori seçiniz" :options="getAvailableCategories(index)" />
    </div>
    <div class="w-full md:w-auto flex justify-end mt-2 md:mt-0">
      <button @click="removeCategory(index, field.value)" type="button"
        class="w-full text-white bg-red-600 hover:bg-red-700 px-4 py-2 rounded-md font-medium">
        Sil
      </button>
    </div>
  </div>
</template>

<script setup>
import { computed } from "vue";
import { useRoute } from "vue-router";
import { useFieldArray, useFormValues } from "vee-validate";
import { storeToRefs } from "pinia";
import { useCategoryStore } from "@/stores/category.store";
import { useProductCategoryStore } from "@/stores/product-category.store";
import FieldSelect from "@/components/Form/FieldSelect.vue";

const categoryStore = useCategoryStore();
const productCategoryStore = useProductCategoryStore();
const { categories } = storeToRefs(categoryStore);

// Get current form values
const values = useFormValues();

// Categories Array yönetimi
const { fields, push, remove } = useFieldArray("productCategories");

// Seçili kategorilerin ID'lerini hesapla
const selectedCategoryIds = computed(() => {
  return (
    values.value.productCategories
      ?.map((pc) => pc.categoryId)
      .filter(Boolean) || []
  );
});

// Mevcut kategorileri filtrele
const availableCategories = computed(() => {
  return categories.value
    .filter((category) => !selectedCategoryIds.value.includes(category.id))
    .map((category) => ({
      value: category.id,
      label: category.name,
    }));
});

// Helper fonksiyon - mevcut kategoriyle birlikte kullanılabilir kategorileri getir
const getAvailableCategories = (index) => {
  const currentCategoryId = values.value.productCategories?.[index]?.categoryId;
  return categories.value
    .filter(
      (category) =>
        !selectedCategoryIds.value.includes(category.id) ||
        category.id === currentCategoryId
    )
    .map((category) => ({
      value: category.id,
      label: category.name,
    }));
};

// Yeni kategori ekle
const addCategory = () => {
  if (availableCategories.value.length > 0) {
    push({
      categoryId: "",
    });
  }
};

// Kategori sil
const removeCategory = async (index, productCategory) => {
  if (productCategory.categoryId && productCategory.productId) {
    await productCategoryStore.deleteProductCategory(productCategory.productId, productCategory.categoryId);
  }
  remove(index);

};
</script>
