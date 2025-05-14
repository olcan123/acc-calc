import { defineStore, acceptHMRUpdate } from "pinia";
import { ref } from "vue";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";
import { useRouter } from "vue-router";

export const usePartnerStore = defineStore("partner", () => {
  const toast = useToast();
  const router = useRouter();
  //NOTE - Kullanılmıyor useRouter

  const partners = ref([]);
  const partner = ref({});
  const loading = ref(false);
  const error = ref(null);
  const message = ref("");

  const fetchPartners = async () => {
    loading.value = true;
    try {
      const response = await axiosInstance.get("partners");
      partners.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const getPartnerDetails = async (id) => {
    try {
      const response = await axiosInstance.get(`partners/details/id/${id}`);
      partner.value = response.data.data;
    } catch (err) {
      error.value = err;
    }
  };

  const getPartner = async (id) => {
    try {
      const response = await axiosInstance.get(`partners/id/${id}`);
      partner.value = response.data.data;
    } catch (err) {
      error.value = err;
    }
  };

  const addPartner = async (payload) => {
    try {
      const response = await axiosInstance.post("partners", payload);
      toast.success("Partner başarıyla eklendi");
      router.push({ name: "table-partner" });
    } catch (err) {
      toast.error("Partner eklenemedi");
      error.value = err;
      throw err;
    }
  };

  const updatePartner = async (payload) => {
    try {
      const response = await axiosInstance.put("partners", payload);
      toast.success("Partner güncellendi");
      router.push({ name: "table-partner" });
    } catch (err) {
      toast.error("Partner güncellenemedi");
      error.value = err;
      throw err;
    }
  };

  const deletePartner = async (partnerId, addressId) => {
    try {
      await axiosInstance.delete(
        `partners/${partnerId}/addresses/${addressId}`
      );
      toast.success("Partner silindi");
      partners.value = partners.value.filter((p) => p.id !== partnerId);
    } catch (err) {
      toast.error("Partner silinemedi");
      error.value = err;
      throw err;
    }
  };

  return {
    partners,
    partner,
    message,
    loading,
    error,
    fetchPartners,
    getPartner,
    getPartnerDetails,
    addPartner,
    updatePartner,
    deletePartner,
  };
});

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(usePartnerStore, import.meta.hot));
}
