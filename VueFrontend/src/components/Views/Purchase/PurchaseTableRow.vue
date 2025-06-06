<template>
  <tr
    class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600"
    v-for="(field, index) in fields"
    :key="field.key"
  >
    <!-- Unit Info Section -->

    <!-- Product Selection -->
    <td class="px-3 py-3 min-w-[200px]">
      <TableFieldSelect
        :fieldName="`purchaseInvoiceLines[${index}].productId`"
        :options="optionProducts"
        placeholder="Ürün seçin"
        @change="onProductChange(index, $event)"
      />
    </td>

    <!-- Account Selection -->
    <td class="px-3 py-3 min-w-[200px]">
      <TableFieldSelect
        :fieldName="`purchaseInvoiceLines[${index}].purchaseAccountId`"
        :options="parentAccountFilterOptions()"
        placeholder="Hesap No"
      />
    </td>

    <!-- Warehouse Selection -->
    <td class="px-3 py-3 min-w-[120px]">
      <TableFieldSelect
        :fieldName="`purchaseInvoiceLines[${index}].warehouseId`"
        :options="optionWarehouses"
        placeholder="Depo seçin"
      />
    </td>
    <!-- Quantity -->
    <td class="px-3 py-3 min-w-[80px]">
      <TableFieldNumberInput
        :fieldName="`purchaseInvoiceLines[${index}].quantity`"
        placeholder="0"
        @input="handleQuantityChange(index, $event)"
      />
    </td>    <!-- Unit of Measure -->
    <td class="px-3 py-3 min-w-[100px]">
      <TableFieldSelect
        :fieldName="`purchaseInvoiceLines[${index}].unitOfMeasureId`"
        :options="optionUnitOfMeasures"
        placeholder="Birim"
      />
    </td>
    
    <!-- Import-only tax rate columns -->
    <!-- Excise Tax Rate (ÖTV %) -->
    <td v-if="isImportPurchase" class="px-3 py-3 min-w-[80px]">
      <TableFieldNumberInput
        :fieldName="`purchaseInvoiceLines[${index}].exciseTaxRate`"
        placeholder="0"
        step="0.01"
        :max="100"
        @input="handleExciseTaxRateChange(index, $event)"
      />
    </td>
    
    <!-- Customs Rate (Gümrük %) -->
    <td v-if="isImportPurchase" class="px-3 py-3 min-w-[80px]">
      <TableFieldNumberInput
        :fieldName="`purchaseInvoiceLines[${index}].customsRate`"
        placeholder="0"
        step="0.01"
        :max="100"
        @input="handleCustomsRateChange(index, $event)"
      />
    </td>
    
    <!-- VAT -->
    <td
      class="px-3 py-3 min-w-[80px] border-r-2 border-gray-300 dark:border-gray-600"
    >
      <TableFieldSelect
        :fieldName="`purchaseInvoiceLines[${index}].vatId`"
        :options="optionVats"
        placeholder="KDV"
        @change="handleVatChange(index, $event)"
      />
    </td>

    <!-- Price → Amount Flow Section -->

    <!-- Unit Price -->
    <td class="px-3 py-3 min-w-[100px]">
      <TableFieldNumberInput
        :fieldName="`purchaseInvoiceLines[${index}].unitPrice`"
        placeholder="0.00"
        step="0.000001"
        @input="handleUnitPriceChange(index, $event)"
      />
    </td>
    <!-- Discount Rate -->
    <td class="px-3 py-3 min-w-[80px]">
      <TableFieldNumberInput
        :fieldName="`purchaseInvoiceLines[${index}].discountRate`"
        placeholder="0"
        step="0.000001"
        :max="100"
        @input="handleDiscountRateChange(index, $event)"
      />
    </td>

    <!-- Cost Price -->
    <td class="px-3 py-3 min-w-[100px]">
      <TableFieldNumberInput
        :fieldName="`purchaseInvoiceLines[${index}].costPrice`"
        placeholder="0.00"
        step="0.000001"
        @input="handleCostPriceChange(index, $event)"
      />
    </td>
    <!-- Total Price -->
    <td class="px-3 py-3 min-w-[100px]">
      <TableFieldNumberInput
        :fieldName="`purchaseInvoiceLines[${index}].totalPrice`"
        placeholder="0.00"
        step="0.000001"
        @input="handleTotalPriceChange(index, $event)"
      />
    </td>
    <!-- Amount (Tutar) -->
    <td class="px-3 py-3 min-w-[100px]">
      <TableFieldNumberInput
        :fieldName="`purchaseInvoiceLines[${index}].amount`"
        placeholder="0.00"
        step="0.000001"
        @input="handleAmountChange(index, $event)"
      />
    </td>    <!-- Discount Amount -->
    <td class="px-3 py-3 min-w-[100px]">
      <div
        class="p-2 bg-gray-50 dark:bg-gray-700 rounded text-center font-medium text-gray-800 dark:text-gray-200"
      >
        {{ field.value.discountAmount || "0.00" }}
      </div>
    </td>    <!-- Expense Amount -->
    <td class="px-3 py-3 min-w-[100px]">
      <div
        class="p-2 bg-blue-50 dark:bg-blue-900 rounded text-center font-medium text-blue-800 dark:text-blue-200"
      >
        {{ field.value.expenseAmount || "0.00" }}
      </div>
    </td>
    
    <!-- Import-only tax amount columns -->
    <!-- Excise Tax Amount (ÖTV Tutarı) -->
    <td v-if="isImportPurchase" class="px-3 py-3 min-w-[100px]">
      <div
        class="p-2 bg-orange-50 dark:bg-orange-900 rounded text-center font-medium text-orange-800 dark:text-orange-200"
      >
        {{ field.value.exciseTaxAmount || "0.00" }}
      </div>
    </td>
    
    <!-- Customs Amount (Gümrük Tutarı) -->
    <td v-if="isImportPurchase" class="px-3 py-3 min-w-[100px]">
      <div
        class="p-2 bg-purple-50 dark:bg-purple-900 rounded text-center font-medium text-purple-800 dark:text-purple-200"
      >
        {{ field.value.customsAmount || "0.00" }}
      </div>
    </td>
    
    <!-- Cost Amount -->
    <td class="px-3 py-3 min-w-[100px]">
      <TableFieldNumberInput
        :fieldName="`purchaseInvoiceLines[${index}].costAmount`"
        placeholder="0.00"
        step="0.000001"
        @input="handleCostAmountChange(index, $event)"
      />
    </td>

    <!-- VAT Amount -->
    <td class="px-3 py-3 min-w-[100px]">
      <div
        class="p-2 bg-green-50 dark:bg-green-900 rounded text-center font-medium text-green-800 dark:text-green-200"
      >
        {{ field.value.vatTaxAmount || "0.00" }}
      </div>
    </td>
    <!-- Total Amount (Final) -->
    <td
      class="px-3 py-3 min-w-[120px] border-r-2 border-gray-300 dark:border-gray-600"
    >
      <TableFieldNumberInput
        :fieldName="`purchaseInvoiceLines[${index}].totalAmount`"
        placeholder="0.00"
        step="0.000001"
        @input="handleTotalAmountChange(index, $event)"
      />
    </td>
    <!-- Actions -->
    <td class="px-3 py-3 min-w-[80px]">
      <button
        type="button"
        @click="removeLine(index)"
        :disabled="fields.length <= 1"
        class="text-red-600 hover:text-red-800 dark:text-red-400 dark:hover:text-red-300 disabled:opacity-50 disabled:cursor-not-allowed p-2 rounded hover:bg-red-50 dark:hover:bg-red-900"
      >
        🗑️
      </button>
    </td>
  </tr>
</template>

<script setup>
import { computed } from "vue";
import { useRoute } from "vue-router";
import { storeToRefs } from "pinia";
import { useFormContext, useFieldArray } from "vee-validate";
import TableFieldNumberInput from "@/components/TableForm/TableFieldNumberInput.vue";
import TableFieldSelect from "@/components/TableForm/TableFieldSelect.vue";
import { useProductStore } from "@/stores/product.store";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { useUnitOfMeasureStore } from "@/stores/unit-of-measure.store";
import { useAccountStore } from "@/stores/account.store";
import { useVatStore } from "@/stores/vat.store";
import { usePurchaseCalculations } from "@/composables/usePurchaseCalculations.js";

// Check if this is an import purchase using route
const route = useRoute();
const isImportPurchase = computed(() => {
  return route.path.includes("/import");
});

// Stores
const productStore = useProductStore();
const warehouseStore = useWarehouseStore();
const unitOfMeasureStore = useUnitOfMeasureStore();
const accountStore = useAccountStore();
const vatStore = useVatStore();

// Store data with reactive refs
const { optionProducts, products } = storeToRefs(productStore);
const { optionWarehouses } = storeToRefs(warehouseStore);
const { optionUnitOfMeasures } = storeToRefs(unitOfMeasureStore);
const { optionVats } = storeToRefs(vatStore);
const { parentAccountFilterOptions } = storeToRefs(accountStore);

// Access parent form context
const { setFieldValue, values: formValues } = useFormContext();
const { remove, fields } = useFieldArray("purchaseInvoiceLines");

// Purchase calculations composable
const {
  onQuantityChange,
  onUnitPriceChange,
  onDiscountRateChange,
  onVatChange,
  onAmountChange,
  onTotalPriceChange,
  onTotalAmountChange,
  onCostPriceChange,
  onCostAmountChange,
  updateCalculations,
} = usePurchaseCalculations(vatStore);

// Event handlers
const onProductChange = (index, $event) => {
  const productId = Number($event.target.value);
  const product = products.value.find((p) => p.id === productId);
  if (product) {
    // Auto-fill product-related fields when product is selected
    setFieldValue(
      `purchaseInvoiceLines[${index}].purchaseAccountId`,
      product.purchaseAccountId || null
    );
    setFieldValue(
      `purchaseInvoiceLines[${index}].unitOfMeasureId`,
      product.unitOfMeasureId || null
    );
    setFieldValue(
      `purchaseInvoiceLines[${index}].unitPrice`,
      product.purchasePrice || 0
    );
    setFieldValue(
      `purchaseInvoiceLines[${index}].vatId`,
      product.vatId || null
    );

    // Ürün seçildikten sonra hesaplamaları güncelle
    setTimeout(() => {
      updateCalculations(setFieldValue, index, formValues);
    }, 50);
  }
};

// Calculation event handlers
const handleQuantityChange = (index, event) => {
  const newQuantity = Number(event.target.value) || 0;
  onQuantityChange(setFieldValue, index, formValues, newQuantity);
};

const handleUnitPriceChange = (index, event) => {
  const newUnitPrice = Number(event.target.value) || 0;
  onUnitPriceChange(setFieldValue, index, formValues, newUnitPrice);
};

const handleDiscountRateChange = (index, event) => {
  const newDiscountRate = Number(event.target.value) || 0;
  onDiscountRateChange(setFieldValue, index, formValues, newDiscountRate);
};

const handleVatChange = (index, event) => {
  const newVatId = Number(event.target.value) || null;
  onVatChange(setFieldValue, index, formValues, newVatId);
};

const handleAmountChange = (index, event) => {
  const newAmount = Number(event.target.value) || 0;
  onAmountChange(setFieldValue, index, formValues, newAmount);
};

const handleTotalPriceChange = (index, event) => {
  const newTotalPrice = Number(event.target.value) || 0;
  onTotalPriceChange(setFieldValue, index, formValues, newTotalPrice);
};

const handleTotalAmountChange = (index, event) => {
  const newTotalAmount = Number(event.target.value) || 0;
  onTotalAmountChange(setFieldValue, index, formValues, newTotalAmount);
};

const handleCostPriceChange = (index, event) => {
  const newCostPrice = Number(event.target.value) || 0;
  onCostPriceChange(setFieldValue, index, formValues, newCostPrice);
};

const handleCostAmountChange = (index, event) => {
  const newCostAmount = Number(event.target.value) || 0;
  onCostAmountChange(setFieldValue, index, formValues, newCostAmount);
};

// Import-only tax handlers
const handleExciseTaxRateChange = (index, event) => {
  const newExciseTaxRate = Number(event.target.value) || 0;
  setFieldValue(`purchaseInvoiceLines[${index}].exciseTaxRate`, newExciseTaxRate);
  
  // Trigger recalculation with the updated values
  setTimeout(() => {
    updateCalculations(setFieldValue, index, formValues, null);
  }, 50);
};

const handleCustomsRateChange = (index, event) => {
  const newCustomsRate = Number(event.target.value) || 0;
  setFieldValue(`purchaseInvoiceLines[${index}].customsRate`, newCustomsRate);
  
  // Trigger recalculation with the updated values
  setTimeout(() => {
    updateCalculations(setFieldValue, index, formValues, null);
  }, 50);
};

const removeLine = (index) => {
  // Handle removal internally using useFieldArray
  if (fields.value.length > 1) {
    remove(index);
  }
};
</script>
