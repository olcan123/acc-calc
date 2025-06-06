<template>
  <BaseModalPersistent v-model="modalStore.isInvoiceModalOpen" size="lg">
    <template #header>
      <h3 class="text-lg font-semibold text-gray-900 dark:text-white">
        Satın Alma Faturası Bilgileri
      </h3>
    </template>

    <div class="max-h-[70vh] overflow-y-auto px-1">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <FieldTextInput
          fieldName="purchaseInvoices[0].invoiceNo"
          labelName="Fatura No"
          placeholder="Fatura numarası girin"
        />

        <FieldDateInput
          fieldName="purchaseInvoices[0].invoiceDate"
          labelName="Fatura Tarihi"
        />
        <FieldSelect
          fieldName="purchaseInvoices[0].partnerId"
          labelName="Tedarikçi"
          :options="optionPartners"
          placeholder="Tedarikçi seçin"
        />
        <FieldSelect
          fieldName="purchaseInvoices[0].vendorAccountId"
          labelName="Tedarikçi Hesabı"
          :options="parentAccountFilterOptions(16)"
          placeholder="Hesap seçin"
        />
        <FieldSelect
          fieldName="purchaseInvoices[0].warehouseId"
          labelName="Depo"
          :options="optionWarehouses"
          placeholder="Depo seçin"
        />

        <!-- İthalat Bilgileri - Ayrı Div -->
        <div
          v-if="isImportPurchase"
          class="md:col-span-2 grid grid-cols-1 md:grid-cols-2 gap-4"
        >
          <FieldTextInput
            fieldName="purchaseInvoices[0].importPartnerDocNo"
            labelName="İthalat Fatura Numarası"
            placeholder="Partner belge numarası girin"
          />

          <FieldDateInput
            fieldName="purchaseInvoices[0].importPartnerDocDate"
            labelName="İthalat Fatura Tarihi"
          />
          <FieldSelect
            fieldName="purchaseInvoices[0].currencyId"
            labelName="Para Birimi"
            :options="optionCurrencies"
            placeholder="Para birimi seçin"
          />

          <FieldNumberInput
            fieldName="purchaseInvoices[0].exchangeRate"
            labelName="Döviz Kuru"
            placeholder="1.00"
            step="0.0001"
          />
        </div>

        <!-- Ödeme Bilgileri - Tek Satır -->
        <div class="md:col-span-2 grid grid-cols-1 md:grid-cols-2 gap-4">
          <FieldCheckbox
            fieldName="purchaseInvoices[0].isPaid"
            labelName="Ödendi"
          />

          <FieldNumberInput
            v-show="values?.purchaseInvoices?.[0]?.isPaid"
            fieldName="purchaseInvoices[0].cashPaymentAmount"
            labelName="Nakit Ödeme Tutarı"
            placeholder="0.00"
            step="0.01"
          />
        </div>
        <div class="md:col-span-2">
          <FieldTextArea
            fieldName="purchaseInvoices[0].note"
            labelName="Not"
            placeholder="Fatura notu (opsiyonel)"
          />
        </div>

        <!-- Masraf/Gider Yönetimi - Collapsible Section -->
        <div
          class="md:col-span-2 border border-gray-200 dark:border-gray-600 rounded-lg"
        >
          <button
            type="button"
            @click="isExpensesOpen = !isExpensesOpen"
            class="w-full flex items-center justify-between p-4 text-left bg-gray-50 dark:bg-gray-700 hover:bg-gray-100 dark:hover:bg-gray-600 rounded-t-lg transition-colors"
          >
            <span class="font-medium text-gray-900 dark:text-white">
              Masraf/Gider Yönetimi ({{ expenses.length }})
            </span>
            <svg
              :class="{ 'rotate-180': isExpensesOpen }"
              class="w-5 h-5 text-gray-500 transition-transform duration-200"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M19 9l-7 7-7-7"
              />
            </svg>
          </button>

          <div v-if="isExpensesOpen" class="p-4 space-y-4">
            <!-- Add New Expense Button -->
            <button
              type="button"
              @click="addNewExpense"
              class="flex items-center gap-2 text-white bg-green-600 hover:bg-green-700 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-4 py-2 dark:bg-green-500 dark:hover:bg-green-600 dark:focus:ring-green-800 transition-colors"
            >
              <span>➕</span>
              Yeni Masraf Ekle
            </button>

            <!-- Expenses List -->
            <div v-if="expenses.length > 0" class="space-y-4">
              <div
                v-for="(expense, index) in expenses"
                :key="index"
                class="border border-gray-200 dark:border-gray-600 rounded-lg p-4 bg-gray-50 dark:bg-gray-700"
              >
                <div class="flex items-center justify-between mb-3">
                  <h4 class="font-medium text-gray-900 dark:text-white">
                    Masraf #{{ index + 1 }}
                  </h4>
                  <button
                    type="button"
                    @click="deleteExpense(index)"
                    class="text-red-600 hover:text-red-800 dark:text-red-400 dark:hover:text-red-300 transition-colors"
                    title="Masrafı Sil"
                  >
                    🗑️
                  </button>
                </div>

                <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                  <FieldSelect
                    :fieldName="`purchaseInvoiceExpenses[${index}].partnerId`"
                    labelName="Tedarikçi"
                    :options="optionPartners"
                    placeholder="Tedarikçi seçin"
                  />

                  <FieldSelect
                    :fieldName="`purchaseInvoiceExpenses[${index}].vendorAccountId`"
                    labelName="Tedarikçi Hesabı"
                    :options="parentAccountFilterOptions(16)"
                    placeholder="Hesap seçin"
                  />

                  <FieldTextInput
                    :fieldName="`purchaseInvoiceExpenses[${index}].partnerInvoiceNo`"
                    labelName="Partner Fatura No"
                    placeholder="Partner fatura numarası"
                  />

                  <FieldDateInput
                    :fieldName="`purchaseInvoiceExpenses[${index}].partnerInvoiceDate`"
                    labelName="Partner Fatura Tarihi"
                  />

                  <FieldSelect
                    :fieldName="`purchaseInvoiceExpenses[${index}].expenseType`"
                    labelName="Masraf Türü"
                    :options="expenseTypeOptions"
                    placeholder="Masraf türü seçin"
                  />

                  <FieldNumberInput
                    :fieldName="`purchaseInvoiceExpenses[${index}].amount`"
                    labelName="Tutar"
                    placeholder="0.0000"
                    step="0.0001"
                  />

                  <FieldNumberInput
                    :fieldName="`purchaseInvoiceExpenses[${index}].revaluationAmount`"
                    labelName="Yeniden Değerleme"
                    placeholder="0.0000"
                    step="0.0001"
                  />

                  <div class="flex items-center">
                    <FieldCheckbox
                      :fieldName="`purchaseInvoiceExpenses[${index}].isPaid`"
                      labelName="Ödendi"
                    />
                  </div>
                </div>
              </div>
            </div>
            <div v-else class="text-center py-8">
              <p class="text-gray-500 dark:text-gray-400">
                Henüz masraf/gider eklenmemiş. Yeni masraf eklemek için
                yukarıdaki butonu kullanın.
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>

    <template #footer>
      <button
        type="button"
        @click="modalStore.closeInvoiceModal()"
        class="px-4 py-2 text-sm text-gray-800 bg-white border border-gray-300 rounded-lg hover:bg-gray-100 dark:bg-gray-800 dark:text-gray-300 dark:border-gray-600 dark:hover:bg-gray-700"
      >
        İptal
      </button>
      <button
        type="button"
        @click="modalStore.closeInvoiceModal()"
        class="px-4 py-2 text-sm text-white bg-blue-600 rounded-lg hover:bg-blue-700 dark:bg-blue-500 dark:hover:bg-blue-600"
      >
        Kaydet
      </button>
    </template>
  </BaseModalPersistent>
</template>

<script setup>
import { computed, ref, watchEffect } from "vue";
import { storeToRefs } from "pinia";
import { useRoute } from "vue-router";
import { useFormContext, useFieldArray } from "vee-validate";
import { usePurchaseCalculations } from "@/composables/usePurchaseCalculations";
import BaseModalPersistent from "@/components/UI/Modal/BaseModalPersistent.vue";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";
import FieldTextArea from "@/components/Form/FieldTextArea.vue";
import FieldDateInput from "@/components/Form/FieldDateInput.vue";
import FieldNumberInput from "@/components/Form/FieldNumberInput.vue";
import FieldSelect from "@/components/Form/FieldSelect.vue";
import FieldCheckbox from "@/components/Form/FieldCheckbox.vue";
import { usePartnerStore } from "@/stores/partner.store";
import { useAccountStore } from "@/stores/account.store";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { useModalStore } from "@/stores/modal.store";
import { useCurrencyStore } from "@/stores/currency.store";
import { useVatStore } from "@/stores/vat.store";

// Route to determine invoice type
const route = useRoute();

// Check if this is an import purchase
const isImportPurchase = computed(() => {
  return route.path.includes("/import");
});

// Expense types options - matching backend ExpenseType enum
const expenseTypeOptions = [
  { value: 1, label: "Nakliye" },           // Freight = 1
  { value: 2, label: "Sigorta" },          // Insurance = 2  
  { value: 3, label: "Gümrük Masrafı" },   // CustomsExpense = 3
  { value: 99, label: "Diğer Masraflar" }, // OtherExpense = 99
];

// Expenses dropdown state
const isExpensesOpen = ref(false);

// Stores
const partnerStore = usePartnerStore();
const accountStore = useAccountStore();
const warehouseStore = useWarehouseStore();
const modalStore = useModalStore();
const currencyStore = useCurrencyStore();

// Store reactive data
const { optionPartners } = storeToRefs(partnerStore);
const { parentAccountFilterOptions } = storeToRefs(accountStore);
const { optionWarehouses } = storeToRefs(warehouseStore);
const { optionCurrencies } = storeToRefs(currencyStore);

// Form context to get current values
const { values, setFieldValue } = useFormContext();

// Initialize purchase calculations for expense redistribution
const vatStore = useVatStore();
const { updateExpenseDistribution } = usePurchaseCalculations(vatStore);

// Field array for managing expenses
const { push: pushExpense, remove: removeExpense } = useFieldArray(
  "purchaseInvoiceExpenses"
);

// Helper function to get default vendor account ID
const getDefaultVendorAccountId = () => {
  return parentAccountFilterOptions.value(16)?.[0]?.value || null;
};

// Expense management functions
const addNewExpense = () => {
  pushExpense({
    partnerId: null,
    vendorAccountId: getDefaultVendorAccountId(),
    partnerInvoiceNo: "",
    partnerInvoiceDate: new Date().toISOString().split("T")[0],
    expenseType: 1, // Default to Freight
    revaluationAmount: 0,
    amount: 0,
    amountFc: 0,
    isPaid: false,
    cashPaymentAmount: 0,
  });
};

const deleteExpense = (index) => {
  removeExpense(index);
};

// Get expenses array from form values
const expenses = computed(() => values.purchaseInvoiceExpenses || []);

// Set default vendor account ID for main purchase invoice
watchEffect(() => {
  setFieldValue("purchaseInvoices[0].vendorAccountId",
    getDefaultVendorAccountId()
  );
});
</script>
