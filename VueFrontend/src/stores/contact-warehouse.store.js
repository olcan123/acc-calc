import { defineStore, acceptHMRUpdate } from "pinia";
import { ref, computed } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";
import parseValidationException from "@/services/validations/parse-validation-exception";

export const useContactWarehouseStore = defineStore("contact-warehouse", () => {
  const router = useRouter();
  const toast = useToast();
  const contactWarehouses = ref([]);
  const contactWarehouse = ref({});
  const contact = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  // 📦 Getter

  // 📦 Action
  const fetchContactsByWarehouseId = async (warehouseId) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(
        `warehouses/${warehouseId}/contacts`
      );
      contactWarehouse.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  const fetchContactByIdIncludeDetails = async (contactId) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(
        `warehouses/contacts/${contactId}`
      );
      contact.value = response.data.data;
      message.value = response.data.message;
    } catch (err) {
      error.value = err;
      console.error(err);
    } finally {
      loading.value = false;
    }
  };

  const createContactWarehouse = async (warehouseId, data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post(
        `warehouses/${warehouseId}/contacts`,
        data
      );
      message.value = response.data.message;
      toast.success(message.value);
      router.push({ name: "table-contact-warehouse", params: { warehouseId } });
    } catch (err) {
      error.value = parseValidationException(err.response.data);
    } finally {
      loading.value = false;
    }
  };

  const deleteContact = async (warehouseId, contactId) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(
        `warehouses/${warehouseId}/contacts/${contactId}`
      );
      message.value = response.data.message;
      toast.success(message.value);

      // ➡️ LOCAL STORE GÜNCELLE
      removeContact(contactId);
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const updateContact = async (warehouseId, contactId, data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put(
        `warehouses/contacts/${contactId}`,
        data
      );
      message.value = response.data.message;
      toast.success(message.value);
      //NOTE: warehouseId sadece route icin kullaniliyor
      router.push({ name: "table-contact-warehouse", params: { warehouseId } });
    } catch (err) {
      error.value = parseValidationException(err.response.data);
    } finally {
      loading.value = false;
    }
  };

  //NOTE: Contact Detail methods */
  const deleteContactDetail = async (detailId) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(
        `contactdetails/id/${detailId}`
      );
      message.value = response.data.message;
      toast.success(message.value);

      // ➡️ LOCAL STORE GÜNCELLE
      removeContactDetail(detailId);
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const updateContactDetail = async (data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put(
        `contactdetails`,
        data
      );
      updateContactDetailInStore(data);
      message.value = response.data.message;
      toast.success(message.value);
    } catch (err) {
      // error.value = parseValidationException(err.response.data);
      console.error(err);
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  // 📦 Local Mutation Actions (Buraya aldık)
  const removeContactDetail = (detailId) => {
    if (!Array.isArray(contactWarehouse.value?.contactWarehouses)) return;

    contactWarehouse.value.contactWarehouses.forEach((cw) => {
      if (Array.isArray(cw.contact?.contactDetails)) {
        cw.contact.contactDetails = cw.contact.contactDetails.filter(
          (cd) => cd.id !== detailId
        );
      }
    });
  };

  const removeContact = (contactId) => {
    if (!Array.isArray(contactWarehouse.value?.contactWarehouses)) return;

    contactWarehouse.value.contactWarehouses =
      contactWarehouse.value.contactWarehouses.filter(
        (cw) => cw.contact?.id !== contactId
      );
  };

  // 🔥 Store içinde contactDetail güncelleyen saf bir mutation
  const updateContactDetailInStore = (updatedDetail) => {
    if (!Array.isArray(contactWarehouse.value.contactWarehouses)) return;

    contactWarehouse.value.contactWarehouses.forEach((cw) => {
      if (Array.isArray(cw.contact?.contactDetails)) {
        const index = cw.contact.contactDetails.findIndex(
          (cd) => cd.id === updatedDetail.id
        );
        if (index !== -1) {
          cw.contact.contactDetails[index] = {
            ...cw.contact.contactDetails[index],
            ...updatedDetail,
          };
        }
      }
    });
  };

  return {
    contactWarehouses,
    contactWarehouse,
    contact,
    message,
    loading,
    error,
    fetchContactByIdIncludeDetails,
    fetchContactsByWarehouseId,
    createContactWarehouse,
    deleteContact,
    updateContact,
    deleteContactDetail,
    updateContactDetail,
  };
});

if (import.meta.hot) {
  import.meta.hot.accept(
    acceptHMRUpdate(useContactWarehouseStore, import.meta.hot)
  );
}
