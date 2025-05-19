import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";
import { useRouter } from "vue-router";

export const useCurrencyStore = defineStore("currency", () => {
  const toast = useToast();
  const router = useRouter();

  // State
  const currencies = ref([]);
  const currency = ref({});
  const loading = ref(false);
  const error = ref(null);

  // Getters
  const optionCurrencies = computed(() => {
    return currencies.value.map((currency) => ({
      label: `${currency.code} - ${currency.name}`,
      value: currency.id,
    }));
  });

  // Actions

  const fetchCurrenciesOtions = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("currencies/async");
      currencies.value = response.data.data;
    } catch (err) {
      error.value = err;
      console.error("Currency fetch error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const fetchCurrencies = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("currencies");
      currencies.value = response.data.data;
      return response.data;
    } catch (err) {
      error.value = err;
      console.error("Currency fetch error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const fetchCurrency = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`currencies/id/${id}`);
      currency.value = response.data.data;
      return response.data;
    } catch (err) {
      error.value = err;
      console.error("Currency fetch error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const addCurrency = async (currencyData) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post("currencies", currencyData);
      toast.success("Para birimi başarıyla eklendi");
      router.push({ name: "table-currency" });
    } catch (err) {
      error.value = err;
      toast.error("Para birimi eklenirken bir hata oluştu");
      console.error("Currency add error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const updateCurrency = async (currencyData) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put("currencies", currencyData);
      toast.success("Para birimi başarıyla güncellendi");
      router.push({ name: "table-currency" });
    } catch (err) {
      error.value = err;
      toast.error("Para birimi güncellenirken bir hata oluştu");
      console.error("Currency update error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const deleteCurrency = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`currencies/id/${id}`);
      toast.success("Para birimi başarıyla silindi");
      currencies.value = currencies.value.filter(
        (currency) => currency.id !== id
      );
    } catch (err) {
      error.value = err;
      toast.error("Para birimi silinirken bir hata oluştu");
      console.error("Currency delete error:", err);
      throw err;
    } finally {
      loading.value = false;
    }
  };

  return {
    currencies,
    currency,
    loading,
    error,
    optionCurrencies,
    fetchCurrenciesOtions,
    fetchCurrencies,
    fetchCurrency,
    addCurrency,
    updateCurrency,
    deleteCurrency,
  };
});

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useCurrencyStore, import.meta.hot));
}
