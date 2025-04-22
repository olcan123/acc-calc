import { defineStore, acceptHMRUpdate } from "pinia";
import { ref } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";

export const useCompanyStore = defineStore("company", () => {
  const router = useRouter();
  const companies = ref([]);
  const company = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  const fetchCompanies = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("companies/include");
      companies.value = response.data;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const fetchCompany = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`companies/id/${id}`);
      company.value = response.data;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const fetchCompanyWithInclude = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`companies/include/id/${id}`);
      company.value = response.data;
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
      // message.value = response.data.message;
      companies.value.data = companies.value.data.filter((company) => company.id !== id);
      console.log(companies.value);
      companies
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  }

  return {
    companies,
    company,
    message,
    loading,
    error,
    fetchCompanies,
    fetchCompany,
    fetchCompanyWithInclude,
    createCompany,
    updateCompany,
    deleteCompany
  };
});

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useCompanyStore, import.meta.hot));
}
