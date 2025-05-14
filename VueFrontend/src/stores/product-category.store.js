import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";
import { useProductStore } from "./product.store";

export const useProductCategoryStore = defineStore("productCategory", () => {
  const productStore = useProductStore();
  const router = useRouter();
  const toast = useToast();
  const productCategories = ref([]);
  const productCategory = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  const optionProductCategories = computed(() => {
    return productCategories.value.map((productCategory) => ({
      value: productCategory.id,
      label: productCategory.name,
    }));
  });

  const deleteProductCategory = async (productId, categoryId) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(
        `productcategories/productId/${productId}/categoryId/${categoryId}`
      );
      productStore.product.productCategories.splice(
        productStore.product.productCategories.findIndex(
          (rel) => rel.productId === productId && rel.categoryId === categoryId
        ),
        1
      );

      toast.success(
        response.data.message || "Ürün kategorisi başarıyla silindi"
      );
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  return {
    productCategories,
    productCategory,
    message,
    loading,
    error,
    optionProductCategories,
    deleteProductCategory,
  };
});

// Hot Module Replacement
if (import.meta.hot) {
  import.meta.hot.accept(
    acceptHMRUpdate(useProductCategoryStore, import.meta.hot)
  );
}
