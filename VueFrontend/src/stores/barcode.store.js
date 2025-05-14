import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";
import { useProductStore } from "./product.store";

export const useBarcodeStore = defineStore("barcode", () => {
  const productStore = useProductStore();
  const router = useRouter();
  const toast = useToast();
  const barcodes = ref([]);
  const barcode = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  const optionBarcodes = computed(() => {
    return barcodes.value.map((barcode) => ({
      value: barcode.id,
      label: barcode.code,
    }));
  });
  const deleteBarcode = async (id) => {
    loading.value = true;
    console.log("Deleting barcode with ID:", productStore.product.barcodes);
    try {
      const response = await axiosInstance.delete(`barcodes/id/${id}`);
      message.value = response.data.message;
      toast.success(message.value || "Barkod başarıyla silindi");
      productStore.product.barcodes = productStore.product.barcodes.filter((barcode) => barcode.id !== id);
    } catch (err) {
      error.value = err;
      toast.error("Barkod silinirken bir hata oluştu: " + (err.response?.data?.message || err.message));
    } finally {
      loading.value = false;
    }
  };

  return {
    barcodes,
    barcode,
    message,
    loading,
    error,
    optionBarcodes,
    deleteBarcode,
  };
});

// Hot Module Replacement
if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useBarcodeStore, import.meta.hot));
}
