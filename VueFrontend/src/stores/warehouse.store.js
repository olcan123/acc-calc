import { defineStore, acceptHMRUpdate } from "pinia";
import { ref } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useCompanyStore } from "./company.store";

const companyStore = useCompanyStore();

export const useWarehouseStore = defineStore("warehouse", () => {
  const warehouses = ref([]);
  const warehouse = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  const router = useRouter();

  const fetchWarehouse = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`warehouses/id/${id}`);
      console.log(response);
      warehouse.value = response.data.data;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const createWarehouse = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post(`warehouses`, data);
      message.value = response.data.message;
      router.push({ name: "table-warehouse" });
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const deleteWarehouse = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(`warehouses/id/${id}`);
      message.value = response.data.message;
      companyStore.company.data.warehouses =
        companyStore.company.data.warehouses.filter(
          (warehouse) => warehouse.id !== id
        );
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const updateWarehouse = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put(`warehouses`, data);
      message.value = response.data.message;
      router.push({ name: "table-warehouse" });
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  return {
    warehouses,
    warehouse,
    message,
    loading,
    error,
    fetchWarehouse,
    createWarehouse,
    deleteWarehouse,
    updateWarehouse,
  };
});

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useWarehouseStore, import.meta.hot));
}
