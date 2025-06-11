<template>
    <div class="p-6 bg-gray-50 dark:bg-gray-700 border-t border-gray-200 dark:border-gray-600">
        <div class="grid grid-cols-1 md:grid-cols-5 gap-4 text-sm">
            <div class="flex justify-between">
                <span class="text-gray-600 dark:text-gray-400">Toplam Tutar:</span>
                <span class="font-medium">{{ formatCurrency(totals.totalAmount) }}</span>
            </div>
            <div class="flex justify-between">
                <span class="text-gray-600 dark:text-gray-400">İskonto:</span>
                <span class="font-medium">{{ formatCurrency(totals.totalDiscountAmount) }}</span>
            </div>
            <div class="flex justify-between">
                <span class="text-gray-600 dark:text-gray-400">Net Tutar:</span>
                <span class="font-medium">{{ formatCurrency(totals.netAmount) }}</span>
            </div>
            <div class="flex justify-between">
                <span class="text-gray-600 dark:text-gray-400">KDV:</span>
                <span class="font-medium">{{ formatCurrency(totals.totalVatAmount) }}</span>
            </div>
            <div class="flex justify-between text-lg font-bold">
                <span class="text-gray-800 dark:text-white">Genel Toplam:</span>
                <span class="text-blue-600 dark:text-blue-400">{{ formatCurrency(totals.grandTotal) }}</span>
            </div>
        </div>
    </div>
</template>

<script setup>
import { computed } from 'vue';
import { useFormContext } from 'vee-validate';
import { storeToRefs } from 'pinia';
import { useVatStore } from '@/stores/vat.store';
import { useSaleCalculations } from '@/composables/useSaleCalculations.js';

// Get form context to access sale invoice lines
const { values: formValues } = useFormContext();

// Stores
const vatStore = useVatStore();
const { vats } = storeToRefs(vatStore);

// Sale calculations composable
const { calculateTotals } = useSaleCalculations(vats);

// Computed totals - calculated from form values
const totals = computed(() => {
  return calculateTotals(formValues.saleInvoiceLines || []);
});

// Helper function for currency formatting
const formatCurrency = (amount) => {
    return new Intl.NumberFormat('tr-TR', {
        style: 'currency',
        currency: 'TRY',
    }).format(amount || 0);
};
</script>
