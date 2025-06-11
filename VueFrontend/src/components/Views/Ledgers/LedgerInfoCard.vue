<template>
  <div class="bg-white dark:bg-gray-800 rounded-lg shadow p-6 mb-6">
    <div class="text-left space-y-4">
      <!-- Form Fields -->
      <div
        class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 pt-4 border-t border-gray-200 dark:border-gray-700"
      >        <!-- Belge Türü Select -->
        <div>
          <label
            class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2"
          >
            Belge Türü
          </label>
          <FieldSelect
            fieldName="ledger.documentType"
            :options="[
              { value: '', label: 'Belge Türü Seçin' },
              { value: '1', label: 'Alım Faturası' },
              { value: '2', label: 'Satış Faturası' },
              { value: '3', label: 'Ödeme' },
              { value: '4', label: 'Tahsilat' },
              { value: '5', label: 'Yevmiye' }
            ]"
          />
        </div>

        <!-- Belge Tarihi -->
        <div>
          <label
            class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2"
          >
            Belge Tarihi
          </label>
          <FieldDateInput
            fieldName="ledger.documentDate"
          />
        </div>

        <!-- Referans No -->
        <div>
          <label
            class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2"
          >
            Referans No
          </label>
          <FieldTextInput
            fieldName="ledger.referenceNo"
            placeholderName="Referans numarası"
          />
        </div>        <!-- Açıklama -->
        <div>
          <label
            class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2"
          >
            Açıklama
          </label>
          <FieldTextInput
            fieldName="ledger.description"
            placeholderName="Fiş açıklaması"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import FieldSelect from "@/components/Form/FieldSelect.vue";
import FieldDateInput from "@/components/Form/FieldDateInput.vue";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";

// Document types mapping
const documentTypeLabels = {
  1: "Alım Faturası",
  2: "Satış Faturası",
  3: "Ödeme",
  4: "Tahsilat",
  5: "Yevmiye",
};

const getDocumentTypeLabel = (type) => {
  return documentTypeLabels[type] || "Belirtilmemiş";
};

const formatDate = (dateString) => {
  if (!dateString) return "Tarih seçilmedi";
  return new Date(dateString).toLocaleDateString("tr-TR");
};
</script>
