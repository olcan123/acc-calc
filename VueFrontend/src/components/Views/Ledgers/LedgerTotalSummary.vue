<template>
  <div
    class="p-6 bg-gray-50 dark:bg-gray-700 border-t border-gray-200 dark:border-gray-600"
  >
    <div class="grid grid-cols-1 md:grid-cols-4 gap-4 text-sm">
      <!-- Toplam Borç -->
      <div class="flex justify-between">
        <span class="text-gray-600 dark:text-gray-400">Toplam Borç:</span>
        <span class="font-medium text-red-600 dark:text-red-400">
          {{ formatCurrency(totals.totalDebit) }}
        </span>
      </div>
      
      <!-- Toplam Alacak -->
      <div class="flex justify-between">
        <span class="text-gray-600 dark:text-gray-400">Toplam Alacak:</span>
        <span class="font-medium text-green-600 dark:text-green-400">
          {{ formatCurrency(totals.totalCredit) }}
        </span>
      </div>
      
      <!-- Fark -->
      <div class="flex justify-between">
        <span class="text-gray-600 dark:text-gray-400">Fark:</span>
        <span 
          class="font-medium"
          :class="totals.difference === 0 ? 'text-gray-600 dark:text-gray-400' : 'text-orange-600 dark:text-orange-400'"
        >
          {{ formatCurrency(Math.abs(totals.difference)) }}
        </span>
      </div>
      
      <!-- Denge Durumu -->
      <div class="flex justify-between text-lg font-bold">
        <span class="text-gray-800 dark:text-white">Durum:</span>
        <span 
          :class="totals.isBalanced ? 'text-green-600 dark:text-green-400' : 'text-red-600 dark:text-red-400'"
        >
          {{ totals.isBalanced ? "✓ Dengeli" : "⚠ Dengesiz" }}
        </span>
      </div>
    </div>
    
    <!-- Warning Message -->
    <div 
      v-if="!totals.isBalanced" 
      class="mt-4 p-3 bg-orange-100 dark:bg-orange-900 border border-orange-300 dark:border-orange-700 rounded-lg"
    >
      <div class="flex items-center gap-2">
        <span class="text-orange-600 dark:text-orange-400">⚠</span>
        <span class="text-sm text-orange-800 dark:text-orange-200 font-medium">
          Dikkat: Muhasebe fişi dengesiz! Borç ve alacak tutarları eşit olmalıdır.
        </span>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed } from "vue";
import { useFormContext } from "vee-validate";

// Get form context to access ledger entries
const { values: formValues } = useFormContext();

// Computed totals - calculated from form values
const totals = computed(() => {
  const entries = formValues.ledgerEntries || [];
  
  const totalDebit = entries.reduce((sum, entry) => {
    return sum + (parseFloat(entry.debit) || 0);
  }, 0);
  
  const totalCredit = entries.reduce((sum, entry) => {
    return sum + (parseFloat(entry.credit) || 0);
  }, 0);
  
  const difference = totalDebit - totalCredit;
  const isBalanced = Math.abs(difference) < 0.01;
  
  return {
    totalDebit,
    totalCredit,
    difference,
    isBalanced
  };
});

// Helper function for currency formatting
const formatCurrency = (amount) => {
  return new Intl.NumberFormat("tr-TR", {
    style: "currency",
    currency: "TRY",
  }).format(amount || 0);
};
</script>
