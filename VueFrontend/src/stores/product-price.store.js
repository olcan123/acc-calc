import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";
import { useProductStore } from "./product.store";

export const useProductPriceStore = defineStore("productPrice", () => {
  const productStore = useProductStore();
  const router = useRouter();
  const toast = useToast();
  const productPrices = ref([]);
  const productPrice = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  const optionProductPrices = computed(() => {
    return productPrices.value.map((productPrice) => ({
      value: productPrice.id,
      label: `${productPrice.price} ${productPrice.currency}`,
    }));
  });

  const deleteProductPrice = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`productprices/id/${id}`);
      message.value = response.data.message;
      toast.success(message.value || "Ürün fiyatı başarıyla silindi");
      productStore.product.productPrices = productStore.product.productPrices.filter(
        (productPrice) => productPrice.id !== id
      );
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  return {
    productPrices,
    productPrice,
    message,
    loading,
    error,
    optionProductPrices,
    deleteProductPrice,
  };
});

// Hot Module Replacement
if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useProductPriceStore, import.meta.hot));
}
