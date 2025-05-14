import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";

export const useCategoryStore = defineStore("category", () => {
  const router = useRouter();
  const toast = useToast();
  const categories = ref([]);
  const category = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  const optionCategories = computed(() => {
    return categories.value.map((category) => ({
      value: category.id,
      label: category.name,
    }));
  });

  const fetchCategories = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("categories");
      categories.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  const fetchCategory = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`categories/id/${id}`);
      category.value = response.data.data;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const createCategory = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post("categories", data);
      message.value = response.data.message;
      toast.success(message.value);
      router.push({ name: "table-category" });
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const updateCategory = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put(`categories`, data);
      message.value = response.data.message;
      toast.success(message.value);
      router.push({ name: "table-category" });
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const deleteCategory = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`categories/id/${id}`);
      categories.value = categories.value.filter((category) => category.id !== id);
      message.value = response.data.message;
      toast.success(message.value);
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  return {
    categories,
    category,
    message,
    loading,
    error,
    optionCategories,
    fetchCategories,
    fetchCategory,
    createCategory,
    updateCategory,
    deleteCategory,
  };
});

// Hot Module Replacement
if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useCategoryStore, import.meta.hot));
}
