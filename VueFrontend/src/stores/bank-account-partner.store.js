import { defineStore, acceptHMRUpdate } from "pinia";
import { ref } from "vue";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";
import { useRouter } from "vue-router";

export const useBankAccountPartnerStore = defineStore("bankAccountPartner", () => {
  const toast = useToast();
  const router = useRouter();

  const bankAccounts = ref([]);
  const bankAccount = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  const fetchBankAccounts = async (partnerId) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`partners/${partnerId}/bankaccounts`);
      bankAccounts.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  const fetchBankAccount = async (partnerId, bankAccountId) => {
    try {
      const response = await axiosInstance.get(
        `partners/${partnerId}/bankaccounts/${bankAccountId}`
      );
      bankAccount.value = response.data.data;
    } catch (err) {
      error.value = err;
      console.error(err);
    }
  };

  const addBankAccount = async (partnerId, payload) => {
    try {
      const response = await axiosInstance.post(
        `partners/${partnerId}/bankaccounts`,
        payload
      );
      toast.success("Banka hesabı başarıyla eklendi");
      await fetchBankAccounts(partnerId);
      router.push({
        name: "table-bank-account-partner",
        params: { partnerId },
      });
      return response.data.data;
    } catch (err) {
      toast.error("Ekleme işlemi başarısız");
      error.value = err;
      throw err;
    }
  };

  const updateBankAccount = async (partnerId, bankAccountId, payload) => {
    try {
      await axiosInstance.put(
        `partners/${partnerId}/bankaccounts/${bankAccountId}`,
        payload
      );
      toast.success("Banka hesabı güncellendi");
      await fetchBankAccounts(partnerId);
      router.push({
        name: "table-bank-account-partner",
        params: { partnerId },
      });
    } catch (err) {
      toast.error("Güncelleme işlemi başarısız");
      error.value = err;
      throw err;
    }
  };

  const deleteBankAccount = async (partnerId, bankAccountId) => {
    try {
      await axiosInstance.delete(
        `partners/${partnerId}/bankaccounts/${bankAccountId}`
      );
      toast.success("Banka hesabı silindi");
      await fetchBankAccounts(partnerId);
    } catch (err) {
      toast.error("Silme işlemi başarısız");
      error.value = err;
      throw err;
    }
  };

  return {
    bankAccounts,
    bankAccount,
    message,
    loading,
    error,
    fetchBankAccounts,
    fetchBankAccount,
    addBankAccount,
    updateBankAccount,
    deleteBankAccount,
  };
});

if (import.meta.hot) {
  import.meta.hot.accept(
    acceptHMRUpdate(useBankAccountPartnerStore, import.meta.hot)
  );
}
