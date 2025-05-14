// src/constants/partnerEnums.js

export const partnerTypeOptions = [
  { label: "İşletme", value: 0 },
  { label: "Bireysel", value: 1 },
  { label: "Çalışan", value: 2 },
];

export const businessPartnerTypeOptions = [
  { label: "Tedarikçi", value: 0 },
  { label: "Müşteri", value: 1 },
  { label: "Her ikisi", value: 2 },
];

export const getPartnerTypeLabel = (value) =>
  partnerTypeOptions.find((opt) => opt.value === value)?.label ?? "-";

export const getBusinessPartnerTypeLabel = (value) =>
  businessPartnerTypeOptions.find((opt) => opt.value === value)?.label ?? "-";
