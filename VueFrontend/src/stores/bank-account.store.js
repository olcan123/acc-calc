import { defineStore, acceptHMRUpdate } from "pinia";
import { ref } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useCompanyStore } from "./company.store";

const companyStore = useCompanyStore();

export const useBankAccountStore = defineStore("bank-account", () => {
  const router = useRouter();
  const bankAccounts = ref([]);
  const bankAccount = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  const fetchBankAccount = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`bankaccounts/id/${id}`);
      bankAccount.value = response.data.data;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const createBankAccount = async (id, data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post(`bankaccounts/id/${id}`, data);
      message.value = response.data.message;
      router.push({ name: "table-bank-account" });
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const deleteBankAccount = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`bankaccounts/id/${id}`);
      companyStore.company.data.bankAccounts =
        companyStore.company.data.bankAccounts.filter(
          (bankAccount) => bankAccount.id !== id
        );
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const updateBankAccount = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put(`bankaccounts`, data);
      message.value = response.data.message;
      router.push({ name: "table-bank-account" });
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  return {
    bankAccounts,
    bankAccount,
    message,
    loading,
    error,
    fetchBankAccount,
    createBankAccount,
    deleteBankAccount,
    updateBankAccount,
  };
});

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useBankAccountStore, import.meta.hot));
}
