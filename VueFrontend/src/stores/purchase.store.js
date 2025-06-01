import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";

export const usePurchaseStore = defineStore("purchase", () => {
  const router = useRouter();
  const toast = useToast();

  // State
  const purchases = ref([]);
  const purchase = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  // Computed - Select options için
  const optionPurchases = computed(() => {
    return purchases.value.map((purchase) => ({
      value: purchase.id,
      label: `${purchase.invoiceNumber} - ${purchase.partnerName}`,
    }));
  });

  // ✅ Fetch All Purchases
  const fetchPurchases = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("purchases");
      purchases.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      toast.error("Satın alma faturaları yüklenirken bir hata oluştu");
      console.error("Purchase fetch error:", err);
    } finally {
      loading.value = false;
    }
  };

  // ✅ Fetch Purchase by ID
  const fetchPurchase = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`purchases/id/${id}`);
      purchase.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      toast.error("Satın alma faturası detayları yüklenirken bir hata oluştu");
      console.error("Purchase detail fetch error:", err);
    } finally {
      loading.value = false;
    }
  };

  // ✅ Create Purchase Invoice
  const createPurchase = async (purchaseData) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post("purchases", purchaseData);
      message.value = response.data.message;
      toast.success(
        message.value || "Satın alma faturası başarıyla oluşturuldu"
      );
      //    router.push({ name: "table-purchase" });
    } catch (err) {
      error.value = err;
      toast.error("Satın alma faturası oluşturulurken bir hata oluştu");
      console.error("Purchase create error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  // ✅ Update Purchase Invoice
  const updatePurchase = async (purchaseData) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put("purchases", purchaseData);
      message.value = response.data.message;
      toast.success(
        message.value || "Satın alma faturası başarıyla güncellendi"
      );
      router.push({ name: "table-purchase" });
    } catch (err) {
      error.value = err;
      toast.error("Satın alma faturası güncellenirken bir hata oluştu");
      console.error("Purchase update error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  // ✅ Delete Purchase Invoice
  const deletePurchase = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`purchases/id/${id}`);
      purchases.value = purchases.value.filter(
        (purchase) => purchase.id !== id
      );
      message.value = response.data.message;
      toast.success(message.value || "Satın alma faturası başarıyla silindi");
    } catch (err) {
      error.value = err;
      toast.error("Satın alma faturası silinirken bir hata oluştu");
      console.error("Purchase delete error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  // ✅ Get Purchase Lines by Purchase ID
  const fetchPurchaseLines = async (purchaseId) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`purchases/${purchaseId}/lines`);
      return response.data.data;
    } catch (err) {
      error.value = err;
      toast.error("Satın alma fatura satırları yüklenirken bir hata oluştu");
      console.error("Purchase lines fetch error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  // ✅ Get Purchase Expenses by Purchase ID
  const fetchPurchaseExpenses = async (purchaseId) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(
        `purchases/${purchaseId}/expenses`
      );
      return response.data.data;
    } catch (err) {
      error.value = err;
      toast.error("Satın alma fatura giderleri yüklenirken bir hata oluştu");
      console.error("Purchase expenses fetch error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  // ✅ Add Purchase Invoice with all related data
  const addPurchaseInvoice = async (data) => {
    loading.value = true;
    try {
      const payload = {
        ledger: data.ledger,
        purchaseInvoices: data.purchaseInvoices,
        purchaseInvoiceLines: data.purchaseInvoiceLines || [],
        purchaseInvoiceExpenses: data.purchaseInvoiceExpenses || [],
      };

      const response = await axiosInstance.post("purchases", payload);
      message.value = response.data.message;
      toast.success(
        message.value || "Satın alma faturası başarıyla kaydedildi"
      );
      //  router.push({ name: "table-purchase" });
      return response.data;
    } catch (err) {
      error.value = err;

      // Validation hatalarını kontrol et
      if (err.response?.data?.errors) {
        const validationErrors = Object.values(err.response.data.errors).flat();
        validationErrors.forEach((errorMsg) => toast.error(errorMsg));
      } else {
        toast.error("Satın alma faturası kaydedilirken bir hata oluştu");
      }

      console.error("Purchase invoice add error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  // ✅ Calculate totals for purchase invoice
  const calculateTotals = (lines) => {
    if (!Array.isArray(lines) || lines.length === 0) {
      return {
        subtotal: 0,
        totalVat: 0,
        totalCustoms: 0,
        totalExcise: 0,
        grandTotal: 0,
      };
    }

    const subtotal = lines.reduce(
      (sum, line) => sum + line.unitPrice * line.quantity,
      0
    );
    const totalVat = lines.reduce(
      (sum, line) => sum + (line.vatAmount || 0),
      0
    );
    const totalCustoms = lines.reduce(
      (sum, line) => sum + (line.customsAmount || 0),
      0
    );
    const totalExcise = lines.reduce(
      (sum, line) => sum + (line.exciseTaxAmount || 0),
      0
    );
    const grandTotal = subtotal + totalVat + totalCustoms + totalExcise;

    return {
      subtotal: Number(subtotal.toFixed(2)),
      totalVat: Number(totalVat.toFixed(2)),
      totalCustoms: Number(totalCustoms.toFixed(2)),
      totalExcise: Number(totalExcise.toFixed(2)),
      grandTotal: Number(grandTotal.toFixed(2)),
    };
  };

  return {
    // State
    purchases,
    purchase,
    message,
    loading,
    error,

    // Computed
    optionPurchases,

    // Actions
    fetchPurchases,
    fetchPurchase,
    createPurchase,
    updatePurchase,
    deletePurchase,
    fetchPurchaseLines,
    fetchPurchaseExpenses,
    addPurchaseInvoice,
    calculateTotals,
  };
});

// Hot Module Replacement
if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(usePurchaseStore, import.meta.hot));
}
