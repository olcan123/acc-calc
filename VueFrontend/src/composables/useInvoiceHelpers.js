import { computed } from 'vue';
import { storeToRefs } from 'pinia';
import { usePartnerStore } from '@/stores/partner.store';
import { useAccountStore } from '@/stores/account.store';
import { useWarehouseStore } from '@/stores/warehouse.store';
import { useCurrencyStore } from '@/stores/currency.store';

/**
 * Shared composable for common invoice helper functions and computed properties
 * Used across both Purchase and Sale invoice components to eliminate code duplication
 * 
 * @returns {Object} Helper functions and store references
 */
export function useInvoiceHelpers() {
  // Initialize stores
  const partnerStore = usePartnerStore();
  const accountStore = useAccountStore();
  const warehouseStore = useWarehouseStore();
  const currencyStore = useCurrencyStore();

  // Get reactive store data
  const { optionPartners } = storeToRefs(partnerStore);
  const { optionAccounts } = storeToRefs(accountStore);
  const { optionWarehouses } = storeToRefs(warehouseStore);
  const { optionCurrencies } = storeToRefs(currencyStore);

  /**
   * Format date to Turkish locale format
   * @param {Date|string} date - Date to format
   * @returns {string} Formatted date string or empty string if invalid
   */
  const formatDate = (date) => {
    if (!date) return '';
    return new Date(date).toLocaleDateString('tr-TR');
  };

  /**
   * Format currency amount with Turkish locale
   * @param {number} amount - Amount to format
   * @param {string} currency - Currency code (default: 'TRY')
   * @param {number} decimals - Number of decimal places (default: 2)
   * @returns {string} Formatted currency string
   */
  const formatCurrency = (amount, currency = 'TRY', decimals = 2) => {
    if (amount === null || amount === undefined) return '-';
    
    // For Euro currency in Purchase invoices
    if (currency === 'EUR') {
      return new Intl.NumberFormat('tr-TR', {
        style: 'currency',
        currency: 'EUR',
        minimumFractionDigits: decimals,
        maximumFractionDigits: decimals,
      }).format(amount || 0);
    }
    
    // For TRY or other currencies
    return new Intl.NumberFormat('tr-TR', {
      style: 'currency',
      currency: currency,
      minimumFractionDigits: decimals,
      maximumFractionDigits: decimals,
    }).format(amount || 0);
  };
  /**
   * Format currency amount without currency symbol (for table displays)
   * @param {number} amount - Amount to format
   * @param {string} currency - Currency symbol to append (default: '€')
   * @param {number} decimals - Number of decimal places (default: 2)
   * @returns {string} Formatted amount with currency symbol
   */
  const formatCurrencySimple = (amount, currency = '€', decimals = 2) => {
    if (amount === null || amount === undefined) return '-';
    return (
      new Intl.NumberFormat('tr-TR', {
        minimumFractionDigits: decimals,
        maximumFractionDigits: decimals,
      }).format(amount) + (currency ? ` ${currency}` : '')
    );
  };
  /**
   * Format exchange rate with 4-10 decimal places
   * @param {number} rate - Exchange rate to format
   * @returns {string} Formatted exchange rate
   */
  const formatExchangeRate = (rate) => {
    if (rate === null || rate === undefined || rate === 0) return '1,0000';
    return new Intl.NumberFormat('tr-TR', {
      minimumFractionDigits: 4,
      maximumFractionDigits: 10,
    }).format(rate);
  };

  /**
   * Get partner name by ID from store options
   * @param {number} partnerId - Partner ID
   * @returns {string} Partner name or empty string if not found
   */
  const getPartnerName = (partnerId) => {
    if (!partnerId || !optionPartners.value) return '';
    const partner = optionPartners.value.find((p) => p.value === partnerId);
    return partner?.label || '';
  };

  /**
   * Get account name by ID from store options
   * @param {number} accountId - Account ID
   * @returns {string} Account name or empty string if not found
   */
  const getAccountName = (accountId) => {
    if (!accountId || !optionAccounts.value) return '';
    const account = optionAccounts.value.find((a) => a.value === accountId);
    return account?.label || '';
  };

  /**
   * Get warehouse name by ID from store options
   * @param {number} warehouseId - Warehouse ID
   * @returns {string} Warehouse name or empty string if not found
   */
  const getWarehouseName = (warehouseId) => {
    if (!warehouseId || !optionWarehouses.value) return '';
    const warehouse = optionWarehouses.value.find((w) => w.value === warehouseId);
    return warehouse?.label || '';
  };

  /**
   * Get currency name by ID from store options
   * @param {number} currencyId - Currency ID
   * @returns {string} Currency name or empty string if not found
   */
  const getCurrencyName = (currencyId) => {
    if (!currencyId || !optionCurrencies.value) return '';
    const currency = optionCurrencies.value.find((c) => c.value === currencyId);
    return currency?.label || '';
  };

  /**
   * Get partner name from partners array (for cases where store options are not used)
   * @param {number} partnerId - Partner ID
   * @param {Array} partners - Partners array from store
   * @returns {string} Partner name or fallback string
   */
  const getPartnerNameFromArray = (partnerId, partners) => {
    if (!partnerId || !partners) return `Partner ${partnerId}`;
    const partner = partners.find(p => p.id === partnerId);
    return partner ? partner.name : `Partner ${partnerId}`;
  };

  /**
   * Get account name from accounts array (for cases where store options are not used)
   * @param {number} accountId - Account ID
   * @param {Array} accounts - Accounts array from store
   * @returns {string} Account name or fallback string
   */
  const getAccountNameFromArray = (accountId, accounts) => {
    if (!accountId || !accounts) return `Hesap ${accountId}`;
    const account = accounts.find(acc => acc.id === accountId);
    return account ? account.name : `Hesap ${accountId}`;
  };

  /**
   * Create a computed property for partner names mapping (for table views)
   * @param {Ref} partners - Reactive partners array
   * @returns {ComputedRef} Computed object mapping partner IDs to names
   */
  const createPartnerNamesMap = (partners) => {
    return computed(() => {
      return partners.value.reduce((acc, partner) => {
        acc[partner.id] = partner.name;
        return acc;
      }, {});
    });
  };

  return {
    // Store references
    optionPartners,
    optionAccounts,
    optionWarehouses,
    optionCurrencies,
    
    // Formatting functions
    formatDate,
    formatCurrency,
    formatCurrencySimple,
    formatExchangeRate,
    
    // Name getter functions (from store options)
    getPartnerName,
    getAccountName,
    getWarehouseName,
    getCurrencyName,
    
    // Name getter functions (from arrays)
    getPartnerNameFromArray,
    getAccountNameFromArray,
    
    // Utility functions
    createPartnerNamesMap,
  };
}
