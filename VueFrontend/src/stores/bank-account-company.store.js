import { defineStore, acceptHMRUpdate } from "pinia";
import { ref } from "vue";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";
import { useRouter } from "vue-router";

export const useBankAccountCompanyStore = defineStore(
  "bankAccountCompany",
  () => {
    const toast = useToast();
    const router = useRouter();

    const bankAccounts = ref([]);
    const bankAccount = ref({});
    const message = ref("");
    const loading = ref(false);
    const error = ref(null);

    const fetchBankAccounts = async (companyId) => {
      loading.value = true;
      try {
        const response = await axiosInstance.get(
          `companies/${companyId}/bankaccounts`
        );
        bankAccounts.value = response.data.data;
        message.value = response.data.message;
      } catch (err) {
        error.value = err;
        console.error(err);
      } finally {
        loading.value = false;
      }
    };

    const fetchBankAccount = async (companyId, bankAccountId) => {
      try {
        const response = await axiosInstance.get(
          `companies/${companyId}/bankaccounts/${bankAccountId}`
        );
        bankAccount.value = response.data.data;
      } catch (err) {
        error.value = err;
        console.error(err);
      }
    };

    const addBankAccount = async (companyId, payload) => {
      try {
        const response = await axiosInstance.post(
          `companies/${companyId}/bankaccounts`,
          payload
        );
        toast.success("Banka hesabı başarıyla eklendi");
        await fetchBankAccounts(companyId);
        router.push({
          name: "table-bank-account-company",
          params: { companyId },
        });
        return response.data.data;
      } catch (err) {
        toast.error("Ekleme işlemi başarısız");
        error.value = err;
        throw err;
      }
    };

    const updateBankAccount = async (companyId, bankAccountId, payload) => {
      try {
        await axiosInstance.put(
          `companies/${companyId}/bankaccounts/${bankAccountId}`,
          payload
        );
        toast.success("Banka hesabı güncellendi");
        await fetchBankAccounts(companyId);
        router.push({
          name: "table-bank-account-company",
          params: { companyId },
        });
      } catch (err) {
        toast.error("Güncelleme işlemi başarısız");
        error.value = err;
        throw err;
      }
    };

    const deleteBankAccount = async (companyId, bankAccountId) => {
      try {
        await axiosInstance.delete(
          `companies/${companyId}/bankaccounts/${bankAccountId}`
        );
        toast.success("Banka hesabı silindi");
        await fetchBankAccounts(companyId);
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
  }
);

if (import.meta.hot) {
  import.meta.hot.accept(
    acceptHMRUpdate(useBankAccountCompanyStore, import.meta.hot)
  );
}
