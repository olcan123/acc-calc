// Invoice formatting utilities

export const formatDate = (date) => {
  if (!date) return ''
  return new Intl.DateTimeFormat('de-DE').format(new Date(date))
}

export const formatCurrency = (amount) => {
  if (amount === null || amount === undefined) return '0,00 ₺'
  return new Intl.NumberFormat('de-DE', {
    style: 'currency',
    currency: 'EUR',
    minimumFractionDigits: 2
  }).format(amount)
}

export const formatNumber = (number) => {
  if (number === null || number === undefined) return '0'
  return new Intl.NumberFormat('de-DE', {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2
  }).format(number)
}

export const numberToWords = (number) => {
  // Simplified Turkish number to words conversion
  // In a real application, you'd want a more comprehensive implementation
  const ones = ['', 'bir', 'iki', 'üç', 'dört', 'beş', 'altı', 'yedi', 'sekiz', 'dokuz']
  const tens = ['', '', 'yirmi', 'otuz', 'kırk', 'elli', 'altmış', 'yetmiş', 'seksen', 'doksan']
  const thousands = ['', 'bin', 'milyon', 'milyar']
  
  if (number === 0) return 'sıfır'
  if (number < 0) return 'eksi ' + numberToWords(-number)
  
  // For simplicity, just return the formatted number
  return formatNumber(number).replace(' €', '')
}

// Print utilities
export const printInvoice = (invoiceNumber = '') => {
  // Store original title
  const originalTitle = document.title
  
  // Set clean title for printing
  document.title = `Fatura ${invoiceNumber}`
  
  // Add print-specific styles temporarily
  const printStyleElement = document.createElement('style')
  printStyleElement.innerHTML = `
    @page { 
      margin: 0; 
      size: A4; 
    }
    @media print {
      html, body {
        margin: 0 !important;
        padding: 0 !important;
        background: white !important;
        -webkit-print-color-adjust: exact !important;
        print-color-adjust: exact !important;
      }
      .no-print { 
        display: none !important; 
      }
    }
  `
  document.head.appendChild(printStyleElement)
  
  // Trigger print
  window.print()
  
  // Clean up after a short delay
  setTimeout(() => {
    document.title = originalTitle
    document.head.removeChild(printStyleElement)
  }, 100)
}

export const exportToPDF = () => {
  // In a real application, you'd use a library like jsPDF or html2pdf
  alert('PDF export özelliği geliştirilme aşamasında')
}
