<template>
  <tr
    class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600"
    v-for="(field, index) in fields"
    :key="`sale-line-${field.key}-${index}`"
  >
    <!-- Unit Info Section -->

    <!-- Product Selection -->
    <td class="px-3 py-3 min-w-[200px]">
      <TableFieldSelect
        :fieldName="`saleInvoiceLines[${index}].productId`"
        :options="optionProducts"
        placeholder="Ürün seçin"
        @change="onProductChange(index, $event)"
      />
    </td>

    <!-- Account Selection -->
    <td class="px-3 py-3 min-w-[200px]">
      <TableFieldSelect
        :fieldName="`saleInvoiceLines[${index}].saleAccountId`"
        :options="saleAccountFilterOptions()"
        placeholder="Hesap No"
      />
    </td>

    <!-- Warehouse Selection -->
    <td class="px-3 py-3 min-w-[120px]">
      <TableFieldSelect
        :fieldName="`saleInvoiceLines[${index}].warehouseId`"
        :options="optionWarehouses"
        placeholder="Depo seçin"
      />
    </td>

    <!-- Quantity -->
    <td class="px-3 py-3 min-w-[80px]">
      <TableFieldNumberInput
        :fieldName="`saleInvoiceLines[${index}].quantity`"
        placeholder="0"
        @input="handleQuantityChange(index, $event)"
      />
    </td>

    <!-- Unit of Measure -->
    <td class="px-3 py-3 min-w-[100px]">
      <TableFieldSelect
        :fieldName="`saleInvoiceLines[${index}].unitOfMeasureId`"
        :options="optionUnitOfMeasures"
        placeholder="Birim"
      />
    </td>    <!-- Description -->
    <td class="px-3 py-3 min-w-[150px]">
      <TableFieldTextarea
        :fieldName="`saleInvoiceLines[${index}].description`"
        placeholder="Açıklama girin"
        :rows="2"
      />
    </td><!-- VAT -->
    <td
      class="px-3 py-3 min-w-[80px] border-r-2 border-gray-300 dark:border-gray-600"
    >
      <TableFieldSelect
        :fieldName="`saleInvoiceLines[${index}].vatId`"
        :options="optionVats"
        placeholder="KDV"
        :disabled="isExportSale"
        @change="handleVatChange(index, $event)"
      />
    </td>

    <!-- Price → Amount Flow Section -->

    <!-- Unit Price -->
    <td class="px-3 py-3 min-w-[100px]">
      <TableFieldNumberInput
        :fieldName="`saleInvoiceLines[${index}].unitPrice`"
        placeholder="0.00"
        step="0.000001"
        @input="handleUnitPriceChange(index, $event)"
      />
    </td>

    <!-- Discount Rate -->
    <td class="px-3 py-3 min-w-[80px]">
      <TableFieldNumberInput
        :fieldName="`saleInvoiceLines[${index}].discountRate`"
        placeholder="0"
        step="0.000001"
        :max="100"
        @input="handleDiscountRateChange(index, $event)"
      />
    </td>

    <!-- Total Price -->
    <td class="px-3 py-3 min-w-[100px]">
      <TableFieldNumberInput
        :fieldName="`saleInvoiceLines[${index}].totalPrice`"
        placeholder="0.00"
        step="0.000001"
        @input="handleTotalPriceChange(index, $event)"
      />
    </td>

    <!-- Amount (Tutar) -->
    <td class="px-3 py-3 min-w-[100px]">
      <TableFieldNumberInput
        :fieldName="`saleInvoiceLines[${index}].amount`"
        placeholder="0.00"
        step="0.000001"
        @input="handleAmountChange(index, $event)"
      />
    </td>

    <!-- Discount Amount -->
    <td class="px-3 py-3 min-w-[100px]">
      <div
        class="p-2 bg-gray-50 dark:bg-gray-700 rounded text-center font-medium text-gray-800 dark:text-gray-200"
      >
        {{ field.value.discountAmount || "0.00" }}
      </div>
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
        :fieldName="`saleInvoiceLines[${index}].totalAmount`"
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
        title="Satırı Sil"
      >
        🗑️
      </button>
    </td>
  </tr>
</template>

<script setup>
import { computed, watch } from "vue";
import { useRoute } from "vue-router";
import { storeToRefs } from "pinia";
import { useFormContext, useFieldArray } from "vee-validate";
import TableFieldNumberInput from "@/components/TableForm/TableFieldNumberInput.vue";
import TableFieldSelect from "@/components/TableForm/TableFieldSelect.vue";
import TableFieldTextInput from "@/components/TableForm/TableFieldTextInput.vue";
import TableFieldTextarea from "@/components/TableForm/TableFieldTextarea.vue";
import { useProductStore } from "@/stores/product.store";
import { useWarehouseStore } from "@/stores/warehouse.store";
import { useUnitOfMeasureStore } from "@/stores/unit-of-measure.store";
import { useAccountStore } from "@/stores/account.store";
import { useVatStore } from "@/stores/vat.store";
import { useSaleCalculations } from "@/composables/useSaleCalculations.js";

// Define component options
defineOptions({
  inheritAttrs: false
});

// Check if this is an export sale using route
const route = useRoute();
const isExportSale = computed(() => {
  return route.name === 'create-export-sale' || route.name === 'update-export-sale';
});

// Stores
const productStore = useProductStore();
const warehouseStore = useWarehouseStore();
const unitOfMeasureStore = useUnitOfMeasureStore();
const accountStore = useAccountStore();
const vatStore = useVatStore();

// Store data with reactive refs
const { products } = storeToRefs(productStore);
const { warehouses } = storeToRefs(warehouseStore);
const { unitOfMeasures } = storeToRefs(unitOfMeasureStore);
const { accounts } = storeToRefs(accountStore);
const { vats } = storeToRefs(vatStore);

// Computed options for selects
const optionProducts = computed(() => {
  return products.value.map(product => ({
    value: product.id,
    label: product.name,
  }));
});

const optionWarehouses = computed(() => {
  return warehouses.value.map(warehouse => ({
    value: warehouse.id,
    label: warehouse.name,
  }));
});

const optionUnitOfMeasures = computed(() => {
  return unitOfMeasures.value.map(unit => ({
    value: unit.id,
    label: unit.name,
  }));
});

const optionVats = computed(() => {
  return vats.value.map(vat => ({
    value: vat.id,
    label: `%${vat.rate} - ${vat.name}`,
  }));
});

const saleAccountFilterOptions = () => {
  return accounts.value
    .map(account => ({
      value: account.id,
      label: `${account.code} - ${account.name}`,
    }));
};

// Access parent form context
const { setFieldValue, values: formValues } = useFormContext();
const { remove, fields } = useFieldArray("saleInvoiceLines");

// Sale calculations composable
const {
  onQuantityChange,
  onUnitPriceChange,
  onDiscountRateChange,
  onVatChange,
  onAmountChange,
  onTotalPriceChange,
  onTotalAmountChange,
  updateCalculations,
} = useSaleCalculations(vats);

// Event handlers
const onProductChange = (index, $event) => {
  const productId = Number($event.target.value);
  const product = products.value.find((p) => p.id === productId);
  if (product) {
    // Auto-fill product-related fields when product is selected
    setFieldValue(`saleInvoiceLines[${index}].saleAccountId`, product.saleAccountId);
    setFieldValue(`saleInvoiceLines[${index}].unitOfMeasureId`, product.unitOfMeasureId);
    
    // For export sales, always set VAT to 0% (vatId=1), otherwise use product's VAT
    if (isExportSale.value) {
      setFieldValue(`saleInvoiceLines[${index}].vatId`, 1);
    } else {
      setFieldValue(`saleInvoiceLines[${index}].vatId`, product.vatId);
    }
    
    setFieldValue(`saleInvoiceLines[${index}].unitPrice`, product.salePrice || 0);
    setFieldValue(`saleInvoiceLines[${index}].description`, product.description || "");
    
    // Set warehouse from invoice data if available
    const warehouseId = formValues.saleInvoices?.[0]?.warehouseId;
    if (warehouseId && !formValues.saleInvoiceLines[index]?.warehouseId) {
      setFieldValue(`saleInvoiceLines[${index}].warehouseId`, warehouseId);
    }

    // Trigger recalculation with the updated values
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

const removeLine = (index) => {
  remove(index);
};

// Watch for export sale changes and set VAT to 0% (vatId=1)
watch(
  () => isExportSale.value,
  (newValue) => {
    if (newValue) {
      // Set VAT to 0% for all existing lines
      formValues.saleInvoiceLines?.forEach((line, index) => {
        setFieldValue(`saleInvoiceLines[${index}].vatId`, 1);
      });
    }
  },
  { immediate: true }
);

// Watch for new lines being added to ensure VAT is set correctly
watch(
  () => formValues.saleInvoiceLines?.length,
  (newLength, oldLength) => {
    if (isExportSale.value && newLength > oldLength) {
      // Set VAT to 0% for newly added lines
      const newIndex = newLength - 1;
      setFieldValue(`saleInvoiceLines[${newIndex}].vatId`, 1);
    }
  }
);
</script>
