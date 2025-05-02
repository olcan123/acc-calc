import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";

export const useCompanyStore = defineStore("company", () => {
  const router = useRouter();
  const toast = useToast();
  const companies = ref([]);
  const company = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  const optionCompanies = computed(() => {
    return companies.value.map((company) => ({
      value: company.id,
      label: company.name,
    }));
  });

  const fetchCompanies = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("companies");
      companies.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  const fetchCompany = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`companies/id/${id}`);
      company.value = response.data.data;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const createCompany = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post("companies", data);
      message.value = response.data.message;
      toast.success(message.value);
      router.push({ name: "table-company" });
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const updateCompany = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put(`companies`, data);
      message.value = response.data.message;
      toast.success(message.value);
      router.push({ name: "table-company" });
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const deleteCompany = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`companies/id/${id}`);
      companies.value = companies.value.filter((company) => company.id !== id);
      message.value = response.data.message;
      toast.success(message.value);
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  return {
    companies,
    company,
    message,
    loading,
    error,
    optionCompanies,
    fetchCompanies,
    fetchCompany,
    createCompany,
    updateCompany,
    deleteCompany,
  };
});

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useCompanyStore, import.meta.hot));
}
