import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";

export const useProductStore = defineStore("product", () => {
  const router = useRouter();
  const toast = useToast();
  const products = ref([]);
  const product = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  // optionProductTypes for select components
  const optionProductTypes = ref([
    { value: 1, label: "Stoklanabilir Mal" },
    { value: 2, label: "Hammadde" },
    { value: 3, label: "Yarı Mamul" },
    { value: 4, label: "Mamül" },
    { value: 5, label: "Hizmet" },
    { value: 7, label: "Sabit Kıymet" },
    { value: 8, label: "Gider" },
    { value: 9, label: "Avans" },
  ]);

  // Options for select components
  const optionProducts = computed(() => {
    return products.value.map((product) => ({
      value: product.id,
      label: product.name,
    }));
  });

  // Fetch all products
  const fetchProducts = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("products");
      products.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      toast.error("Ürünler yüklenirken bir hata oluştu.");
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  // Fetch product by id
  const fetchProduct = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`products/id/${id}`);
      product.value = response.data.data;
    } catch (err) {
      error.value = err;
      toast.error("Ürün detayları yüklenirken bir hata oluştu.");
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  // Create product with related entities
  const createProduct = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post("products", {
        product: data.product,
        barcodes: data.barcodes || [],
        productPrices: data.productPrices || [],
        productCategories: data.productCategories || [],
      });
      message.value = response.data.message;
      toast.success(message.value || "Ürün başarıyla oluşturuldu");
      router.push({ name: "table-product" });
    } catch (err) {
      error.value = err;
      toast.error("Ürün oluşturulurken bir hata oluştu.");
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  // Update product with related entities
  const updateProduct = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put("products", {
        product: data.product,
        barcodes: data.barcodes || [],
        productPrices: data.productPrices || [],
        productCategories: data.productCategories || [],
      });
      message.value = response.data.message;
      toast.success(message.value || "Ürün başarıyla güncellendi");
      router.push({ name: "table-product" });
    } catch (err) {
      error.value = err;
      toast.error("Ürün güncellenirken bir hata oluştu.");
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  // Delete product
  const deleteProduct = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`products/id/${id}`);
      products.value = products.value.filter((product) => product.id !== id);
      message.value = response.data.message;
      toast.success(message.value || "Ürün başarıyla silindi");
    } catch (err) {
      error.value = err;
      toast.error("Ürün silinirken bir hata oluştu.");
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  return {
    products,
    product,
    message,
    loading,
    error,
    optionProducts,
    optionProductTypes,
    fetchProducts,
    fetchProduct,
    createProduct,
    updateProduct,
    deleteProduct,
  };
});

// Hot Module Replacement
if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useProductStore, import.meta.hot));
}
