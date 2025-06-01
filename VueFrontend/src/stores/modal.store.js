import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useModalStore = defineStore('modal', () => {
  // State
  const isInvoiceModalOpen = ref(false);

  // Actions
  const openInvoiceModal = () => {
    isInvoiceModalOpen.value = true;
  };

  const closeInvoiceModal = () => {
    isInvoiceModalOpen.value = false;
  };

  const toggleInvoiceModal = () => {
    isInvoiceModalOpen.value = !isInvoiceModalOpen.value;
  };

  return {
    // State
    isInvoiceModalOpen,
    
    // Actions
    openInvoiceModal,
    closeInvoiceModal,
    toggleInvoiceModal
  };
});
