import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { axiosInstance } from "@/services/api/axiosInstance";

export const useBankStore = defineStore("bank", () => {
  const banks = ref([]);
  const bank = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  const getSelectBanks = computed(() => {
    return banks.value.data.map((bank) => ({
      label: bank.name,
      value: bank.id,
    }));
  });

  const fetchBanks = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("banks");
      banks.value = response.data;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const fetchBank = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`banks/id/${id}`);
      bank.value = response.data;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const createBank = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post("banks", data);
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const updateBank = async (id, data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put(`banks/id/${id}`, data);
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const deleteBank = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`banks/id/${id}`);
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  return {
    banks,
    bank,
    message,
    loading,
    error,
    getSelectBanks,
    fetchBanks,
    fetchBank,
    createBank,
    updateBank,
    deleteBank,
  };
});

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useBankStore, import.meta.hot));
}
