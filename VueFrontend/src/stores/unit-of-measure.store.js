import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";

export const useUnitOfMeasureStore = defineStore("unitOfMeasure", () => {
  const router = useRouter();
  const toast = useToast();
  const unitOfMeasures = ref([]);
  const unitOfMeasure = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);
  const optionUnitOfMeasures = computed(() => {
    return unitOfMeasures.value.map((unit) => ({
      value: unit.id,
      label: `${unit.name} (${unit.abbreviation})`,
    }));
  });

  const fetchUnitOfMeasures = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("unitofmeasures");
      unitOfMeasures.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  const fetchUnitOfMeasure = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`unitofmeasures/id/${id}`);
      unitOfMeasure.value = response.data.data;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const createUnitOfMeasure = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post("unitofmeasures", data);
      message.value = response.data.message;
      toast.success(message.value);
      router.push({ name: "table-unit-of-measure" });
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const updateUnitOfMeasure = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put(`unitofmeasures`, data);
      message.value = response.data.message;
      toast.success(message.value);
      router.push({ name: "table-unit-of-measure" });
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const deleteUnitOfMeasure = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`unitofmeasures/id/${id}`);
      unitOfMeasures.value = unitOfMeasures.value.filter((unit) => unit.id !== id);
      message.value = response.data.message;
      toast.success(message.value);
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  return {
    unitOfMeasures,
    unitOfMeasure,
    message,
    loading,
    error,
    optionUnitOfMeasures,
    fetchUnitOfMeasures,
    fetchUnitOfMeasure,
    createUnitOfMeasure,
    updateUnitOfMeasure,
    deleteUnitOfMeasure,
  };
});

// Hot Module Replacement
if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useUnitOfMeasureStore, import.meta.hot));
}
