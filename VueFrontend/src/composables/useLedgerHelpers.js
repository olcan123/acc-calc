// Ledger modal'ları için ortak helper fonksiyonları

export function useLedgerHelpers() {
  // Tarih formatlama fonksiyonu
  const formatDate = (date) => {
    if (!date) return '-';
    return new Date(date).toLocaleDateString('tr-TR');
  };

  // Tarih ve saat formatlama fonksiyonu
  const formatDateTime = (date) => {
    if (!date) return '-';
    return new Date(date).toLocaleString('tr-TR');
  };

  // Para birimi formatlama fonksiyonu
  const formatCurrency = (amount, decimals = 2) => {
    if (amount === null || amount === undefined) return '-';
    return new Intl.NumberFormat('de-DE', {
      style: 'currency',
      currency: 'EUR',
      minimumFractionDigits: decimals,
      maximumFractionDigits: decimals,
    }).format(amount);
  };

  // Belge türü label'ı alma fonksiyonu
  const getDocumentTypeLabel = (type) => {
    const types = {
      1: 'Satın Alma Faturası',
      2: 'Satış Faturası',
      3: 'Ödeme',
      4: 'Tahsilat'
    };
    return types[type] || 'Bilinmeyen';
  };

  // Durum label'ı alma fonksiyonu
  const getStatusLabel = (status) => {
    const statuses = {
      1: 'Aktif',
      2: 'Pasif',
      3: 'İptal'
    };
    return statuses[status] || 'Bilinmeyen';
  };

  // Durum CSS class'ı alma fonksiyonu
  const getStatusClass = (status) => {
    const classes = {
      1: 'text-green-600 dark:text-green-400',
      2: 'text-yellow-600 dark:text-yellow-400',
      3: 'text-red-600 dark:text-red-400'
    };
    return classes[status] || 'text-gray-600 dark:text-gray-400';
  };

  // Hesap adı alma fonksiyonu
  const getAccountName = (accountId, accounts) => {
    const account = accounts.find(acc => acc.id === accountId);
    return account ? account.name : `Hesap ${accountId}`;
  };

  // Partner adı alma fonksiyonu
  const getPartnerName = (partnerId, partners) => {
    const partner = partners.find(p => p.id === partnerId);
    return partner ? partner.name : `Partner ${partnerId}`;
  };

  // Çalışan bakiye hesaplama fonksiyonu
  const calculateRunningBalance = (lineNo, ledgerEntries) => {
    if (!ledgerEntries) return 0;
    
    // Sort entries by line number and calculate running balance up to the current line
    const sortedEntries = [...ledgerEntries].sort((a, b) => a.lineNo - b.lineNo);
    let runningBalance = 0;
    
    for (const entry of sortedEntries) {
      runningBalance += (entry.debit || 0) - (entry.credit || 0);
      
      if (entry.lineNo === lineNo) {
        break;
      }
    }
    
    return runningBalance;
  };

  // Toplam borç hesaplama fonksiyonu
  const calculateTotalDebit = (ledgerEntries) => {
    if (!ledgerEntries) return 0;
    return ledgerEntries.reduce((sum, entry) => sum + (entry.debit || 0), 0);
  };

  // Toplam alacak hesaplama fonksiyonu
  const calculateTotalCredit = (ledgerEntries) => {
    if (!ledgerEntries) return 0;
    return ledgerEntries.reduce((sum, entry) => sum + (entry.credit || 0), 0);
  };

  // Net bakiye hesaplama fonksiyonu
  const calculateFinalBalance = (totalDebit, totalCredit) => {
    return totalDebit - totalCredit;
  };

  // Bakiye farkı hesaplama fonksiyonu
  const calculateBalanceDifference = (totalDebit, totalCredit) => {
    return Math.abs(totalDebit - totalCredit);
  };

  return {
    formatDate,
    formatDateTime,
    formatCurrency,
    getDocumentTypeLabel,
    getStatusLabel,
    getStatusClass,
    getAccountName,
    getPartnerName,
    calculateRunningBalance,
    calculateTotalDebit,
    calculateTotalCredit,
    calculateFinalBalance,
    calculateBalanceDifference
  };
}
