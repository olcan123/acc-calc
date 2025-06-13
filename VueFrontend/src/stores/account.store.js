import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";

export const useAccountStore = defineStore("account", () => {
  const router = useRouter();
  const toast = useToast();
  const accounts = ref([]);
  const account = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);
  const optionAccounts = computed(() => {
    return accounts.value.map((account) => ({
      value: account.id,
      label: `${account.code} - ${account.name}`,
    }));
  });

  // Ebeveyn hesaplar için seçenekler
  const parentAccountOptions = computed(() => {
    return accounts.value
      .filter((account) => account.parentId === null)
      .map((account) => ({
        value: account.id,
        label: `${account.code} - ${account.name}`,
      }));
  });
  // Hesap tipleri için seçenekler
  const accountTypeOptions = ref([
    { value: 1, label: "Varlık (Asset)" },
    { value: 2, label: "Yükümlülük (Liability)" },
    { value: 3, label: "Özsermaye (Equity)" },
    { value: 4, label: "Gelir (Revenue)" },
    { value: 5, label: "Gider (Expense)" },
  ]);

  // Normal bakiye tipleri için seçenekler
  const normalBalanceOptions = ref([
    { value: 1, label: "Borç (Debit)" },
    { value: 2, label: "Alacak (Credit)" },
  ]);

  // Sadece `isPostable: true` olanları temel alan computed özellik
  const parentAccountFilterOptions = computed(() => {
    console.log(
      "parentAccountFilterOptions ana computed fonksiyonu yeniden hesaplandı."
    );

    // `filterByPostable` parametresi kaldırıldı.
    return (parentIdOrIds) => {
      let logMessage = "tümü için";
      if (parentIdOrIds) {
        if (Array.isArray(parentIdOrIds)) {
          logMessage = `parent ID'ler [${parentIdOrIds.join(", ")}] için`;
        } else {
          logMessage = `parent ID ${parentIdOrIds} için`;
        }
      }

      // 1. ADIM: HER ZAMAN isPostable: true olan hesapları al.
      const postableAccounts = accounts.value.filter(
        (account) => account.isPostable === true
      );

      // 2. ADIM: Bu "postableAccounts" listesi üzerinden parentIdOrIds filtresini uygula.
      let filteredParentAccounts;
      if (!parentIdOrIds) {
        // parentIdOrIds yoksa, tüm "postable" hesapları kullan.
        filteredParentAccounts = postableAccounts;
      } else if (Array.isArray(parentIdOrIds)) {
        if (parentIdOrIds.length === 0) {
          // parentIdOrIds boş bir dizi ise, hiçbir ebeveynle eşleşmeyecek.
          filteredParentAccounts = [];
        } else {
          filteredParentAccounts = postableAccounts.filter(
            (account) =>
              account.parentAccountId !== null &&
              parentIdOrIds.includes(account.parentAccountId)
          );
        }
      } else {
        // parentIdOrIds tek bir değer ise, "postable" hesaplar arasından filtrele.
        filteredParentAccounts = postableAccounts.filter(
          (account) => account.parentAccountId === parentIdOrIds
        );
      }

      // 3. ADIM: Sonucu map et.
      // Etiketteki "(Puanlanabilir)" ifadesi artık gereksiz, çünkü hepsi zaten `isPostable:true`.
      return filteredParentAccounts.map((account) => ({
        value: account.id,
        label: `${account.code} - ${account.name}`,
      }));
    };
  });

// Filter accounts by code prefixes and return as options (value/label format)
  const optionAccountsSartsWithCode = computed(() => {
    // Rest parametresi kullanılarak birden fazla argüman alınır
    return (...startsWithCodes) => {
      // Gelen argümanları bir diziye dönüştürün ve her birini string'e çevirin.
      // Eğer hiç argüman verilmezse, boş bir dizi olur.
      const codesArray = startsWithCodes.flat().map(String);

      const filteredAccounts = accounts.value.filter((account) => {
        // Her zaman isPostable olan hesapları filtrele
        if (account.isPostable !== true) {
          return false;
        }

        // Eğer codesArray boşsa, isPostable olan tüm hesapları döndür
        if (codesArray.length === 0) {
          return true; // isPostable zaten kontrol edildi, o yüzden buraya gelenler zaten true
        }

        // Hesap kodunun mevcut olup olmadığını kontrol et
        if (!account.code) {
          return false;
        }

        const accountCode = account.code.toString();

        // Parametre olarak verilen tüm kod ön ekleriyle eşleşip eşleşmediğini kontrol et
        // "3" için özel aralık veya başka bir özel durum yok, hepsi 'startsWith' kuralına tabi
        return codesArray.some((code) => accountCode.startsWith(code));
      });

      // Map to option format with value and label
      return filteredAccounts.map((account) => ({
        value: account.id,
        label: `${account.code} - ${account.name}`,
      }));
    };
  });


  const fetchAccounts = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("accounts");
      accounts.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  const fetchAccount = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`accounts/${id}`);
      account.value = response.data.data;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const fetchAccountsByParentId = async (parentId) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`accounts/parent/${parentId}`);
      return response.data.data;
    } catch (err) {
      error.value = err;
      return [];
    } finally {
      loading.value = false;
    }
  };

  const createAccount = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post("accounts", data);
      message.value = response.data.message;
      toast.success(message.value || "Hesap başarıyla oluşturuldu");
      router.push({ name: "table-account" });
    } catch (err) {
      error.value = err;
      toast.error("Hesap oluşturulurken bir hata oluştu");
    } finally {
      loading.value = false;
    }
  };

  const updateAccount = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put(`accounts`, data);
      message.value = response.data.message;
      toast.success(message.value || "Hesap başarıyla güncellendi");
      router.push({ name: "table-account" });
    } catch (err) {
      error.value = err;
      toast.error("Hesap güncellenirken bir hata oluştu");
    } finally {
      loading.value = false;
    }
  };

  const deleteAccount = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`accounts/${id}`);
      accounts.value = accounts.value.filter((account) => account.id !== id);
      message.value = response.data.message;
      toast.success(message.value || "Hesap başarıyla silindi");
    } catch (err) {
      error.value = err;
      toast.error("Hesap silinirken bir hata oluştu");
    } finally {
      loading.value = false;
    }
  };
  return {
    accounts,
    account,
    message,
    loading,
    error,
    accountTypeOptions,
    parentAccountOptions,
    normalBalanceOptions,
    optionAccounts,
    parentAccountFilterOptions,
    optionAccountsSartsWithCode,
    fetchAccounts,
    fetchAccount,
    fetchAccountsByParentId,
    createAccount,
    updateAccount,
    deleteAccount,
  };
});

// HMR için gerekli
if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useAccountStore, import.meta.hot));
}
