import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";

export const useWarehouseStore = defineStore("warehouse", () => {
  const router = useRouter();
  const toast = useToast();
  const warehouses = ref([]);
  const warehouse = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  // 📦 Getter (Computed)
  const flattenedWarehouses = computed(() => {
    return warehouses.value.flatMap((warehouse) =>
      warehouse.addressWarehouses.map((aw) => ({
        id: warehouse.id,
        warehouseName: warehouse.name,
        street: aw.address?.street ?? "-",
        city: aw.address?.city ?? "-",
        country: aw.address?.country ?? "-",
        zipCode: aw.address?.zipCode ?? "-",
      }))
    );
  });

  const fetchIncludeWarehouses = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("warehouses");
      warehouses.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  const fetchIncludeWarehouse = async (id) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(`warehouses/id/${id}`);
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
      const response = await axiosInstance.post("warehouses", data);
      message.value = response.data.message;
      toast.success(message.value);
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
      warehouses.value = warehouses.value.filter(
        (warehouse) => warehouse.id !== id
      );
      message.value = response.data.message;
      toast.success(message.value);
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
      toast.success(message.value);
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
    flattenedWarehouses,
    fetchIncludeWarehouses,
    fetchIncludeWarehouse,
    createWarehouse,
    deleteWarehouse,
    updateWarehouse,
  };
});

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useWarehouseStore, import.meta.hot));
}
