// 📁 stores/bank.store.js

import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";

export const useBankStore = defineStore("bank", () => {
  const router = useRouter();
  const toast = useToast();

  const banks = ref([]);
  const bank = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  // Select options için computed field
  const optionBanks = computed(() => {
    return banks.value.map((bank) => ({
      value: bank.id,
      label: bank.name,
    }));
  });

  // ✅ Fetch All Banks
  const fetchBanks = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("banks");
      banks.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  // ✅ Fetch Single Bank by ID
  const fetchBank = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`banks/id/${id}`);
      bank.value = response.data.data;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  // ✅ Create Bank
  const createBank = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post("banks", data);
      message.value = response.data.message;
      toast.success(message.value);
      router.push({ name: "table-bank" });
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  // ✅ Update Bank
  const updateBank = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put("banks", data);
      message.value = response.data.message;
      toast.success(message.value);
      router.push({ name: "table-bank" });
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  // ✅ Delete Bank
  const deleteBank = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`banks/id/${id}`);
      banks.value = banks.value.filter((b) => b.id !== id);
      message.value = response.data.message;
      toast.success(message.value);
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
    optionBanks,
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
