// src/services/constants/purchase-enum.js

export const invoiceTypeOptions = [
  { label: "Lokal Fatura", value: 1 },      // LocalInvoice = 1
  { label: "İthalat Faturası", value: 2 },  // ImportInvoice = 2
  { label: "Borç Dekontu", value: 3 },      // DebitNote = 3
  { label: "Alacak Dekontu", value: 4 },    // CreditNote = 4
];

export const statusOptions = [
  { label: "Taslak", value: 0 },
  { label: "Onaylandı", value: 1 },
  { label: "İptal Edildi", value: 2 },
];

export const expenseTypeOptions = [
  { label: "Nakliye", value: 1 },           // Freight = 1
  { label: "Sigorta", value: 2 },          // Insurance = 2  
  { label: "Gümrük Masrafı", value: 3 },   // CustomsExpense = 3
  { label: "Diğer Masraflar", value: 99 }, // OtherExpense = 99
];

export const getInvoiceTypeLabel = (value) =>
  invoiceTypeOptions.find((opt) => opt.value === value)?.label ?? "-";

export const getStatusLabel = (value) =>
  statusOptions.find((opt) => opt.value === value)?.label ?? "-";

export const getExpenseTypeLabel = (value) =>
  expenseTypeOptions.find((opt) => opt.value === value)?.label ?? "-";

export const getInvoiceTypeColor = (value) => {
  switch (value) {
    case 1: return "bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-300"; // Local
    case 2: return "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300"; // Import
    case 3: return "bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-300"; // Debit
    case 4: return "bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-300"; // Credit
    default: return "bg-gray-100 text-gray-800 dark:bg-gray-900 dark:text-gray-300";
  }
};

export const getStatusColor = (value) => {
  switch (value) {
    case 0: return "bg-gray-100 text-gray-800 dark:bg-gray-900 dark:text-gray-300"; // Draft
    case 1: return "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300"; // Approved
    case 2: return "bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-300"; // Cancelled
    default: return "bg-gray-100 text-gray-800 dark:bg-gray-900 dark:text-gray-300";
  }
};
