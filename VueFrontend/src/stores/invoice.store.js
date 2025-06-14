import { defineStore } from 'pinia'
import { ref, computed } from 'vue'

export const useInvoiceStore = defineStore('invoice', () => {
  // State
  const currentInvoice = ref(null)
  const loading = ref(false)
  const error = ref(null)

  // Company information (could be moved to a separate company store)
  const companyInfo = ref({
    name: 'ABC Şirketi Ltd. Şti.',
    address: 'Merkez Mahallesi, Atatürk Caddesi No: 123',
    city: 'İstanbul',
    postalCode: '34000',
    phone: '+90 212 555 0123',
    email: 'info@abcsirketi.com.tr',
    taxNumber: '1234567890',
    website: 'www.abcsirketi.com.tr'
  })

  // Company bank information
  const companyBankInfo = ref([
    {
      id: 1,
      bankName: 'ABC Bank',
      branchName: 'Merkez Şubesi',
      iban: 'TR12 3456 7890 1234 5678 9012 34',
      accountNumber: '12345678'
    },
    {
      id: 2,
      bankName: 'XYZ Bank',
      branchName: 'Şişli Şubesi',
      iban: 'TR98 7654 3210 9876 5432 1098 76',
      accountNumber: '87654321'
    }
  ])

  // Customer bank information (for payment details)
  const bankInfo = ref({
    bankName: 'ABC Bank',
    branchName: 'Merkez Şubesi',
    iban: 'TR12 3456 7890 1234 5678 9012 34',
    accountNumber: '12345678'
  })

  // Computed properties
  const invoiceData = computed(() => {
    if (!currentInvoice.value) return getDefaultInvoiceData()
    return {
      id: currentInvoice.value.id,
      invoiceNumber: currentInvoice.value.invoiceNumber || 'SAT-2025-0001',
      invoiceDate: currentInvoice.value.invoiceDate || new Date(),
      dueDate: currentInvoice.value.dueDate || new Date(),
      documentType: currentInvoice.value.documentType || 'Lokal',
      paymentTerms: currentInvoice.value.paymentTerms || 'Vadeli - 30 gün',
      notes: currentInvoice.value.notes || 'Teşekkür ederiz.'
    }
  })

  const customerInfo = computed(() => {
    if (!currentInvoice.value || !currentInvoice.value.partner) return getDefaultCustomerInfo()
    const partner = currentInvoice.value.partner
    return {
      name: partner.name || 'Müşteri Adı',
      address: partner.address || 'Müşteri Adresi',
      city: partner.city || 'Şehir',
      postalCode: partner.postalCode || '00000',
      phone: partner.phone || 'Telefon',
      email: partner.email || 'email@domain.com',
      taxNumber: partner.taxNumber || 'Vergi No',
      shippingAddress: partner.shippingAddress || null
    }
  })

  const invoiceLines = computed(() => {
    if (!currentInvoice.value || !currentInvoice.value.saleInvoiceLines) return getDefaultInvoiceLines()
    return currentInvoice.value.saleInvoiceLines.map(line => ({
      productName: line.product?.name || 'Ürün Adı',
      description: line.description || '',
      quantity: line.quantity || 0,
      unitName: line.unitOfMeasure?.name || 'Adet',
      unitPrice: line.unitPrice || 0,
      discountRate: line.discountRate || 0,
      amount: line.amount || 0,
      vatRate: line.vat?.rate || 0,
      vatTaxAmount: line.vatTaxAmount || 0,
      totalAmount: line.totalAmount || 0
    }))
  })

  const summary = computed(() => {
    const lines = invoiceLines.value
    const subtotal = lines.reduce((sum, line) => sum + (line.amount || 0), 0)
    const totalDiscount = lines.reduce((sum, line) => sum + (line.discountAmount || 0), 0)
    const totalVat = lines.reduce((sum, line) => sum + (line.vatTaxAmount || 0), 0)
    const grandTotal = lines.reduce((sum, line) => sum + (line.totalAmount || 0), 0)
    
    return {
      subtotal,
      totalDiscount,
      totalVat,
      grandTotal
    }
  })

  // Actions
  const loadInvoice = async (invoiceId) => {
    loading.value = true
    error.value = null
    try {
      if (invoiceId && invoiceId !== 'sample') {
        // Load from API
        // const response = await invoiceAPI.getInvoice(invoiceId)
        // currentInvoice.value = response.data
        
        // For now, use sample data
        loadSampleData()
      } else {
        loadSampleData()
      }
    } catch (err) {
      error.value = err.message || 'Failed to load invoice'
      loadSampleData() // Fallback to sample data
    } finally {
      loading.value = false
    }
  }

  const loadSampleData = () => {
    currentInvoice.value = {
      id: 1,
      invoiceNumber: 'SAT-2025-0001',
      invoiceDate: new Date(),
      dueDate: new Date(Date.now() + 30 * 24 * 60 * 60 * 1000),
      documentType: 'Lokal',
      paymentTerms: 'Vadeli - 30 gün',
      notes: 'İyi çalışmalar dileriz.',
      partner: {
        name: 'Örnek Müşteri Ltd. Şti.',
        address: 'Müşteri Mahallesi, Müşteri Caddesi No: 456',
        city: 'İstanbul',
        postalCode: '34100',
        phone: '+90 212 555 4567',
        email: 'musteri@ornek.com.tr',
        taxNumber: '9876543210'
      },
      saleInvoiceLines: [
        {
          product: { name: 'Dizüstü Bilgisayar' },
          description: 'Dell Latitude 5520, 16GB RAM, 512GB SSD',
          quantity: 2,
          unitOfMeasure: { name: 'Adet' },
          unitPrice: 15000,
          discountRate: 5,
          amount: 28500,
          vat: { rate: 20 },
          vatTaxAmount: 5700,
          totalAmount: 34200
        },
        {
          product: { name: 'Klavye' },
          description: 'Mekanik Klavye - RGB',
          quantity: 2,
          unitOfMeasure: { name: 'Adet' },
          unitPrice: 500,
          discountRate: 0,
          amount: 1000,
          vat: { rate: 20 },
          vatTaxAmount: 200,
          totalAmount: 1200
        }
      ]
    }
  }

  const getDefaultInvoiceData = () => ({
    id: null,
    invoiceNumber: 'SAT-2025-0001',
    invoiceDate: new Date(),
    dueDate: new Date(),
    documentType: 'Lokal',
    paymentTerms: 'Vadeli - 30 gün',
    notes: 'Teşekkür ederiz.'
  })

  const getDefaultCustomerInfo = () => ({
    name: 'Müşteri Adı',
    address: 'Müşteri Adresi',
    city: 'Şehir',
    postalCode: '00000',
    phone: 'Telefon',
    email: 'email@domain.com',
    taxNumber: 'Vergi No'
  })

  const getDefaultInvoiceLines = () => ([
    {
      productName: 'Örnek Ürün',
      description: 'Ürün açıklaması',
      quantity: 1,
      unitName: 'Adet',
      unitPrice: 100,
      discountRate: 0,
      amount: 100,
      vatRate: 20,
      vatTaxAmount: 20,
      totalAmount: 120
    }
  ])

  const clearInvoice = () => {
    currentInvoice.value = null
    error.value = null
  }

  const updateCompanyInfo = (newInfo) => {
    companyInfo.value = { ...companyInfo.value, ...newInfo }
  }

  const updateBankInfo = (newInfo) => {
    bankInfo.value = { ...bankInfo.value, ...newInfo }
  }

  return {
    // State
    currentInvoice,
    loading,
    error,
    companyInfo,
    companyBankInfo,
    bankInfo,
    
    // Computed
    invoiceData,
    customerInfo,
    invoiceLines,
    summary,
    
    // Actions
    loadInvoice,
    loadSampleData,
    clearInvoice,
    updateCompanyInfo,
    updateBankInfo
  }
})
