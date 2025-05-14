import { defineStore, acceptHMRUpdate } from "pinia";
import { ref } from "vue";
import { useRouter } from "vue-router";
import { axiosInstance } from "@/services/api/axiosInstance";
import { useToast } from "vue-toastification";
import parseValidationException from "@/services/validations/parse-validation-exception";

export const useContactPartnerStore = defineStore("contact-partner", () => {
  const router = useRouter();
  const toast = useToast();
  const contactPartners = ref([]);
  const contactPartner = ref({});
  const contact = ref({});
  const message = ref("");
  const loading = ref(false);
  const error = ref(null);

  const fetchContactsByPartnerId = async (partnerId) => {
    loading.value = true;
    try {
      const response = await axiosInstance.get(
        `partners/${partnerId}/contacts`
      );
      contactPartner.value = response.data.data;
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
        `partners/contacts/${contactId}`
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

  const createContactPartner = async (partnerId, data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.post(
        `partners/${partnerId}/contacts`,
        data
      );
      message.value = response.data.message;
      toast.success(message.value);
      router.push({ name: "table-contact-partner", params: { partnerId } });
    } catch (err) {
      error.value = parseValidationException(err.response.data);
    } finally {
      loading.value = false;
    }
  };

  const deleteContact = async (partnerId, contactId) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(
        `partners/${partnerId}/contacts/${contactId}`
      );
      message.value = response.data.message;
      toast.success(message.value);
      removeContact(contactId);
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const updateContact = async (partnerId, contactId, data) => {
    loading.value = true;
    try {
      const response = await axiosInstance.put(
        `partners/contacts/${contactId}`,
        data
      );
      message.value = response.data.message;
      toast.success(message.value);
      router.push({ name: "table-contact-partner", params: { partnerId } });
    } catch (err) {
      error.value = parseValidationException(err.response.data);
    } finally {
      loading.value = false;
    }
  };

  const deleteContactDetail = async (detailId) => {
    loading.value = true;
    try {
      const response = await axiosInstance.delete(
        `contactdetails/id/${detailId}`
      );
      message.value = response.data.message;
      toast.success(message.value);
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
      const response = await axiosInstance.put(`contactdetails`, data);
      updateContactDetailInStore(data);
      message.value = response.data.message;
      toast.success(message.value);
    } catch (err) {
      error.value = err;
    } finally {
      loading.value = false;
    }
  };

  const removeContactDetail = (detailId) => {
    if (!Array.isArray(contactPartner.value?.contactPartners)) return;

    contactPartner.value.contactPartners.forEach((cp) => {
      if (Array.isArray(cp.contact?.contactDetails)) {
        cp.contact.contactDetails = cp.contact.contactDetails.filter(
          (cd) => cd.id !== detailId
        );
      }
    });
  };

  const removeContact = (contactId) => {
    if (!Array.isArray(contactPartner.value?.contactPartners)) return;

    contactPartner.value.contactPartners = contactPartner.value.contactPartners.filter(
      (cp) => cp.contact?.id !== contactId
    );
  };

  const updateContactDetailInStore = (updatedDetail) => {
    if (!Array.isArray(contactPartner.value.contactPartners)) return;

    contactPartner.value.contactPartners.forEach((cp) => {
      if (Array.isArray(cp.contact?.contactDetails)) {
        const index = cp.contact.contactDetails.findIndex(
          (cd) => cd.id === updatedDetail.id
        );
        if (index !== -1) {
          cp.contact.contactDetails[index] = {
            ...cp.contact.contactDetails[index],
            ...updatedDetail,
          };
        }
      }
    });
  };

  return {
    contactPartners,
    contactPartner,
    contact,
    message,
    loading,
    error,
    fetchContactByIdIncludeDetails,
    fetchContactsByPartnerId,
    createContactPartner,
    deleteContact,
    updateContact,
    deleteContactDetail,
    updateContactDetail,
  };
});

if (import.meta.hot) {
  import.meta.hot.accept(
    acceptHMRUpdate(useContactPartnerStore, import.meta.hot)
  );
}