import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { useToast } from "vue-toastification";
import { axiosInstance } from "@/services/api/axiosInstance";

export const useSaleStore = defineStore("sale", () => {
  const router = useRouter();
  const toast = useToast();

  // State
  const sales = ref([]);
  const sale = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  // Computed - Select options için
  const optionSales = computed(() => {
    return sales.value.map((sale) => ({
      value: sale.id,
      label: `${sale.invoiceNo} - ${sale.partnerName}`,
    }));
  });

  // ✅ Fetch All Sales
  const fetchSales = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("sales");
      sales.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      toast.error("Satış faturaları yüklenirken bir hata oluştu");
      console.error("Sale fetch error:", err);
    } finally {
      loading.value = false;
    }
  };

  // ✅ Fetch Sale by ID
  const fetchSale = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`sales/id/${id}`);
      sale.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      toast.error("Satış faturası detayları yüklenirken bir hata oluştu");
      console.error("Sale detail fetch error:", err);
    } finally {
      loading.value = false;
    }
  };

  // ✅ Get Sale Invoice (alias for fetchSale for invoice component)
  const getSaleInvoice = async (id) => {
    await fetchSale(id);
    return sale.value;
  };

  // ✅ Add Sale Invoice with all related data
  const addSaleInvoice = async (data) => {
    loading.value = true;
    try {
      const payload = {
        ledger: data.ledger,
        saleInvoices: data.saleInvoices,
        saleInvoiceLines: data.saleInvoiceLines || [],
      };

      const response = await axiosInstance.post("sales", payload);
      message.value = response.data.message;
      toast.success(message.value || "Satış faturası başarıyla kaydedildi");
      router.push({ name: "table-sale" });
      return response.data;
    } catch (err) {
      error.value = err;

      // Validation hatalarını kontrol et
      if (err.response?.data?.errors) {
        const validationErrors = err.response.data.errors;
        Object.keys(validationErrors).forEach((field) => {
          validationErrors[field].forEach((errorMessage) => {
            toast.error(`${field}: ${errorMessage}`);
          });
        });
      } else {
        const errorMessage =
          err.response?.data?.message ||
          "Satış faturası kaydedilirken bir hata oluştu";
        toast.error(errorMessage);
      }

      console.error("Sale invoice add error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  // ✅ Update Sale Invoice
  const updateSale = async (data) => {
    loading.value = true;
    try {
      const payload = {
        ledger: data.ledger,
        saleInvoices: data.saleInvoices,
        saleInvoiceLines: data.saleInvoiceLines || [],
      };

      const response = await axiosInstance.put(`sales`, payload);
      message.value = response.data.message;
      toast.success(message.value || "Satış faturası başarıyla güncellendi");

      // Redirect to sale table
      router.push({ name: "table-sale" });
      return response.data;
    } catch (err) {
      error.value = err;

      // Validation hatalarını kontrol et
      if (err.response?.data?.errors) {
        const validationErrors = err.response.data.errors;
        Object.keys(validationErrors).forEach((field) => {
          validationErrors[field].forEach((errorMessage) => {
            toast.error(`${field}: ${errorMessage}`);
          });
        });
      } else {
        const errorMessage =
          err.response?.data?.message ||
          "Satış faturası güncellenirken bir hata oluştu";
        toast.error(errorMessage);
      }

      console.error("Sale update error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  // ✅ Delete Sale Invoice
  const deleteSale = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`sales/id/${id}`);
      message.value = response.data.message;
      toast.success(message.value || "Satış faturası başarıyla silindi");

      // Liste güncellemek için tekrar fetch
      await fetchSales();
      return response.data;
    } catch (err) {
      error.value = err;
      const errorMessage =
        err.response?.data?.message ||
        "Satış faturası silinirken bir hata oluştu";
      toast.error(errorMessage);
      console.error("Sale delete error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  // ✅ Calculate totals for sale invoice
  const calculateTotals = (lines) => {
    if (!Array.isArray(lines) || lines.length === 0) {
      return {
        subtotal: 0,
        totalVat: 0,
        grandTotal: 0,
      };
    }

    const subtotal = lines.reduce(
      (sum, line) => sum + line.unitPrice * line.quantity,
      0
    );
    const totalVat = lines.reduce(
      (sum, line) => sum + (line.vatTaxAmount || 0),
      0
    );
    const grandTotal = subtotal + totalVat;

    return {
      subtotal: Number(subtotal.toFixed(2)),
      totalVat: Number(totalVat.toFixed(2)),
      grandTotal: Number(grandTotal.toFixed(2)),
    };
  };

  return {
    // State
    sales,
    sale,
    message,
    loading,
    error,

    // Computed
    optionSales,

    // Actions
    fetchSales,
    fetchSale,
    getSaleInvoice,
    addSaleInvoice,
    updateSale,
    deleteSale,
    calculateTotals,
  };
});

// Hot Module Replacement
if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useSaleStore, import.meta.hot));
}
