import { defineStore, acceptHMRUpdate } from "pinia";
import { ref } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";

export const useLedgerStore = defineStore("ledger", () => {
  const router = useRouter();
  const toast = useToast();
    // State
  const ledgers = ref([]);
  const ledger = ref({});
  const ledgerEntries = ref([]);
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);
  // ✅ GetList - LedgersController.GetList() ile aynı
  const fetchLedgers = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("ledgers");
      ledgers.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      toast.error("Muhasebe fişleri yüklenirken bir hata oluştu");
    } finally {
      loading.value = false;
    }
  };
  // ✅ GetById - LedgersController.GetById(id) ile aynı
  const fetchLedger = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`ledgers/id/${id}`);
      const data = response.data.data;
      ledger.value = data;
      // Extract ledger entries if they exist in the response
      ledgerEntries.value = data.ledgerEntries || [];
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      toast.error("Muhasebe fişi yüklenirken bir hata oluştu");
    } finally {
      loading.value = false;
    }
  };

  // ✅ Add - LedgersController.Add(model) ile aynı
  const addLedger = async (model) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post("ledgers", model);
      message.value = response.data.message;
      toast.success(message.value || "Muhasebe fişi başarıyla kaydedildi");
      router.push({ name: "table-ledger" });
      return response.data;
    } catch (err) {
      error.value = err;
      toast.error("Muhasebe fişi kaydedilirken bir hata oluştu");
      throw err;
    } finally {
      loading.value = false;
    }
  };

  // ✅ Update - LedgersController.Update(model) ile aynı
  const updateLedger = async (model) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put("ledgers", model);
      message.value = response.data.message;
      toast.success(message.value || "Muhasebe fişi başarıyla güncellendi");
      router.push({ name: "table-ledger" });
      return response.data;
    } catch (err) {
      error.value = err;
      toast.error("Muhasebe fişi güncellenirken bir hata oluştu");
      throw err;
    } finally {
      loading.value = false;
    }
  };

  // ✅ Delete - LedgersController.Delete(id) ile aynı
  const deleteLedger = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`ledgers/id/${id}`);
      message.value = response.data.message;
      toast.success(message.value || "Muhasebe fişi başarıyla silindi");
      ledgers.value = ledgers.value.filter((l) => l.id !== id);
      return response.data;
    } catch (err) {
      error.value = err;
      toast.error("Muhasebe fişi silinirken bir hata oluştu");
      throw err;
    } finally {
      loading.value = false;
    }
  };
  return {
    // State
    ledgers,
    ledger,
    ledgerEntries,
    message,
    loading,
    error,
    
    // Actions
    fetchLedgers,
    fetchLedger,
    addLedger,
    updateLedger,
    deleteLedger,
  };
});

// HMR support
if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useLedgerStore, import.meta.hot));
}
