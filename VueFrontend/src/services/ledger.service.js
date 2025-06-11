import apiService from "./api.service";

const ENDPOINTS = {
  LEDGERS: "ledgers",
};

const ledgerService = {
  // Get all ledgers with entries
  getAll() {
    return apiService.get(ENDPOINTS.LEDGERS);
  },

  // Get ledger by ID with entries
  getById(id) {
    return apiService.get(`${ENDPOINTS.LEDGERS}/id/${id}`);
  },

  // Create new ledger with entries (ledgerization)
  create(ledgerData) {
    return apiService.post(ENDPOINTS.LEDGERS, ledgerData);
  },

  // Update ledger with entries (ledgerization)
  update(ledgerData) {
    return apiService.put(ENDPOINTS.LEDGERS, ledgerData);
  },

  // Delete ledger and its entries (ledgerization)
  delete(id) {
    return apiService.delete(`${ENDPOINTS.LEDGERS}/id/${id}`);
  },

  // Helper: Create empty ledger structure
  createEmptyLedger() {
    return {
      id: 0,
      documentType: 5, // Default to Journal
      documentDate: new Date().toISOString().split('T')[0],
      referenceNo: "",
      description: "",
      status: 1 // Active
    };
  },

  // Helper: Create empty ledger entry
  createEmptyLedgerEntry(lineNo = 1) {
    return {
      id: 0,
      ledgerId: 0,
      partnerId: null,
      lineNo: lineNo,
      accountId: 0,
      description: "",
      debit: 0,
      credit: 0
    };
  },

  // Helper: Validate ledger balance
  validateLedgerBalance(ledgerEntries) {
    const totalDebit = ledgerEntries.reduce((sum, entry) => sum + (parseFloat(entry.debit) || 0), 0);
    const totalCredit = ledgerEntries.reduce((sum, entry) => sum + (parseFloat(entry.credit) || 0), 0);
    
    return {
      isBalanced: Math.abs(totalDebit - totalCredit) < 0.01, // Allow small rounding differences
      totalDebit,
      totalCredit,
      difference: totalDebit - totalCredit
    };
  },

  // Helper: Format ledger data for API
  formatLedgerForApi(ledger, ledgerEntries) {
    return {
      ledger: {
        ...ledger,
        documentDate: new Date(ledger.documentDate).toISOString()
      },
      ledgerEntries: ledgerEntries.map((entry, index) => ({
        ...entry,
        lineNo: index + 1,
        debit: parseFloat(entry.debit) || 0,
        credit: parseFloat(entry.credit) || 0
      }))
    };
  }
};

export default ledgerService;
