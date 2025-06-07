<template>
  <div class="bg-white dark:bg-gray-800 rounded-lg shadow p-6 mb-6">    <button
      type="button"
      @click="modalStore.openInvoiceModal()"
      class="w-full p-6 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors"
    >
      <div class="text-left space-y-4">
        <!-- Fatura No ve Durum -->
        <div class="flex items-center justify-between mb-3">
          <h3 class="font-bold text-gray-800 dark:text-white text-xl">
            {{
              formValues.purchaseInvoices?.[0]?.invoiceNo ||
              "Fatura No Girilmedi"
            }}
          </h3>
          <span
            class="text-xs bg-green-100 text-green-800 px-3 py-1 rounded-full font-medium dark:bg-green-900 dark:text-green-300"
          >
            ✓ Dolduruldu
          </span>
        </div>

        <!-- Label'lar Grid İçinde -->
        <div class="grid grid-cols-2 gap-2">
          <!-- Tedarikçi Label -->
          <div
            class="flex items-center gap-2 bg-blue-50 dark:bg-blue-900 px-3 py-2 rounded-lg"
          >
            <span class="text-blue-600 dark:text-blue-400">🏢</span>
            <div class="flex-1 min-w-0">
              <p class="text-xs text-blue-600 dark:text-blue-400 font-medium">
                Tedarikçi
              </p>
              <p
                class="text-sm text-blue-800 dark:text-blue-200 font-medium truncate"
              >
                {{
                  getPartnerName(formValues.purchaseInvoices?.[0]?.partnerId) ||
                  "Seçilmedi"
                }}
              </p>
            </div>
          </div>

          <!-- Tarih Label -->
          <div
            class="flex items-center gap-2 bg-purple-50 dark:bg-purple-900 px-3 py-2 rounded-lg"
          >
            <span class="text-purple-600 dark:text-purple-400">📅</span>
            <div class="flex-1 min-w-0">
              <p
                class="text-xs text-purple-600 dark:text-purple-400 font-medium"
              >
                Fatura Tarihi
              </p>
              <p
                class="text-sm text-purple-800 dark:text-purple-200 font-medium"
              >
                {{
                  formatDate(formValues.purchaseInvoices?.[0]?.invoiceDate) ||
                  "Tarih Seçilmedi"
                }}
              </p>
            </div>
          </div>

          <!-- Ödendi mi ve Ödenmişse değeri isPaid ve cashPaymentAmount -->
          <div
            class="flex items-center gap-2 bg-yellow-50 dark:bg-yellow-900 px-3 py-2 rounded-lg"
          >
            <span class="text-yellow-600 dark:text-yellow-400">💰</span>
            <div class="flex-1 min-w-0">
              <p
                class="text-xs text-yellow-600 dark:text-yellow-400 font-medium"
              >
                Ödendi mi?
              </p>
              <p
                class="text-sm text-yellow-800 dark:text-yellow-200 font-medium"
              >
                {{
                  formValues.purchaseInvoices?.[0]?.isPaid ? "Evet" : "Hayır"
                }}
              </p>
              <p class="text-xs text-yellow-600 dark:text-yellow-400">
                Ödeme Tutarı:
                {{
                  formValues.purchaseInvoices?.[0]?.cashPaymentAmount || "0,00"
                }}
              </p>
            </div>
          </div>          <!-- Hesap Label -->
          <div
            class="flex items-center gap-2 bg-emerald-50 dark:bg-emerald-900 px-3 py-2 rounded-lg"
          >
            <span class="text-emerald-600 dark:text-emerald-400">🏦</span>
            <div class="flex-1 min-w-0">
              <p
                class="text-xs text-emerald-600 dark:text-emerald-400 font-medium"
              >
                Hesap
              </p>
              <p
                class="text-sm text-emerald-800 dark:text-emerald-200 font-medium truncate"
              >
                {{
                  getAccountName(
                    formValues.purchaseInvoices?.[0]?.vendorAccountId
                  ) || "Seçilmedi"
                }}
              </p>
            </div>
          </div>

          <!-- Currency Label (only for import purchases) -->
          <div
            v-if="isImportPurchase"
            class="flex items-center gap-2 bg-indigo-50 dark:bg-indigo-900 px-3 py-2 rounded-lg"
          >
            <span class="text-indigo-600 dark:text-indigo-400">💱</span>
            <div class="flex-1 min-w-0">
              <p
                class="text-xs text-indigo-600 dark:text-indigo-400 font-medium"
              >
                Para Birimi
              </p>
              <p
                class="text-sm text-indigo-800 dark:text-indigo-200 font-medium truncate"
              >
                {{
                  getCurrencyName(formValues.purchaseInvoices?.[0]?.currencyId) ||
                  "Seçilmedi"
                }}
              </p>
            </div>
          </div>

          <!-- Exchange Rate Label (only for import purchases) -->
          <div
            v-if="isImportPurchase"
            class="flex items-center gap-2 bg-orange-50 dark:bg-orange-900 px-3 py-2 rounded-lg"
          >
            <span class="text-orange-600 dark:text-orange-400">📊</span>
            <div class="flex-1 min-w-0">
              <p
                class="text-xs text-orange-600 dark:text-orange-400 font-medium"
              >
                Döviz Kuru
              </p>
              <p
                class="text-sm text-orange-800 dark:text-orange-200 font-medium"
              >
                {{ formatExchangeRate(formValues.purchaseInvoices?.[0]?.exchangeRate) }}
              </p>
            </div>
          </div>
        </div>

        <!-- Not -->
        <div class="bg-gray-50 dark:bg-gray-700 px-3 py-2 rounded-lg">
          <div class="flex items-start gap-2">
            <span class="text-gray-500 dark:text-gray-400 mt-0.5">📝</span>
            <div>
              <p class="text-xs text-gray-600 dark:text-gray-400 font-medium">
                Not
              </p>
              <p class="text-sm text-gray-700 dark:text-gray-300 italic">
                {{ formValues.purchaseInvoices?.[0]?.note || "Not girilmedi" }}
              </p>
            </div>
          </div>
        </div>
      </div>
    </button>
  </div>
</template>

<script setup>
import { computed } from "vue";
import { storeToRefs } from "pinia";
import { useRoute } from "vue-router";
import { useFormContext } from "vee-validate";
import { usePartnerStore } from "@/stores/partner.store";
import { useAccountStore } from "@/stores/account.store";
import { useCurrencyStore } from "@/stores/currency.store";
import { useModalStore } from "@/stores/modal.store";

// Route for import purchase detection
const route = useRoute();

// Check if this is an import purchase
const isImportPurchase = computed(() => {
  return route.path.includes("/import");
});

// Stores
const partnerStore = usePartnerStore();
const accountStore = useAccountStore();
const currencyStore = useCurrencyStore();
const modalStore = useModalStore();

const { optionPartners } = storeToRefs(partnerStore);
const { optionAccounts } = storeToRefs(accountStore);
const { optionCurrencies } = storeToRefs(currencyStore);

const { values: formValues } = useFormContext();

// Helper functions
const formatDate = (date) => {
  if (!date) return "";
  return new Date(date).toLocaleDateString("tr-TR");
};

const formatExchangeRate = (rate) => {
  if (!rate || rate === 0) return "1,0000";
  return new Intl.NumberFormat("tr-TR", {
    minimumFractionDigits: 4,
    maximumFractionDigits: 10,
  }).format(rate);
};

const getPartnerName = (partnerId) => {
  if (!partnerId || !optionPartners.value) return "";
  const partner = optionPartners.value.find((p) => p.value === partnerId);
  return partner?.label || "";
};

const getAccountName = (accountId) => {
  if (!accountId || !optionAccounts.value) return "";
  const account = optionAccounts.value.find((a) => a.value === accountId);
  return account?.label || "";
};

const getCurrencyName = (currencyId) => {
  if (!currencyId || !optionCurrencies.value) return "";
  const currency = optionCurrencies.value.find((c) => c.value === currencyId);
  return currency?.label || "";
};
</script>
