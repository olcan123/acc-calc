import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";

export const useVatStore = defineStore("vat", () => {
  const router = useRouter();
  const toast = useToast();
  const vats = ref([]);
  const vat = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  const optionVats = computed(() => {
    return vats.value.map((vat) => ({
      value: vat.id,
      label: `${vat.name} (${vat.rate}%)`,
    }));
  });

  const fetchVats = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("vats");
      vats.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  const fetchVat = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`vats/id/${id}`);
      vat.value = response.data.data;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const createVat = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post("vats", data);
      message.value = response.data.message;
      toast.success(message.value);
      router.push({ name: "table-vat" });
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const updateVat = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put(`vats`, data);
      message.value = response.data.message;
      toast.success(message.value);
      router.push({ name: "table-vat" });
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const deleteVat = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`vats/id/${id}`);
      vats.value = vats.value.filter((vat) => vat.id !== id);
      message.value = response.data.message;
      toast.success(message.value);
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  return {
    vats,
    vat,
    message,
    loading,
    error,
    optionVats,
    fetchVats,
    fetchVat,
    createVat,
    updateVat,
    deleteVat,
  };
});

// Hot Module Replacement
if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useVatStore, import.meta.hot));
}
