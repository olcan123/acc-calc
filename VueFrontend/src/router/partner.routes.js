const TablePartnerView = () => import("@/views/Partners/TablePartnerView.vue");
const UpdatePartnerView = () =>
  import("@/views/Partners/UpdatePartnerView.vue");
const CreatePartnerView = () =>
  import("@/views/Partners/CreatePartnerView.vue");

//SECTION - Bank Account
const TableBankAccountPartnerView = () =>
  import("@/views/Partners/BankAccounts/TableBankAccountPartnerView.vue");
const UpdateBankAccountPartnerView = () =>
  import("@/views/Partners/BankAccounts/UpdateBankAccountPartnerView.vue");
const CreateBankAccountPartnerView = () =>
  import("@/views/Partners/BankAccounts/CreateBankAccountPartnerView.vue");

//SECTION - Contacts
const TableContactView = () =>
  import("@/views/Partners/Contacts/TableContactView.vue");
const UpdateContactView = () =>
  import("@/views/Partners/Contacts/UpdateContactView.vue");
const CreateContactView = () =>
  import("@/views/Partners/Contacts/CreateContactView.vue");

export default [
  {
    path: "partners",
    name: "table-partner",
    meta: { requiresAuth: false, breadcrumb: "partners" },
    component: TablePartnerView,
  },
  {
    path: "partners/create",
    name: "create-partner",
    meta: { requiresAuth: false, breadcrumb: "create-partner" },
    component: CreatePartnerView,
  },
  {
    path: "partners/update/:id",
    name: "update-partner",
    meta: { requiresAuth: false, breadcrumb: "update-partner" },
    component: UpdatePartnerView,
  },

  //SECTION - Bank Accounts

  {
    path: "partners/:partnerId/bankaccounts",
    name: "table-bank-account-partner",
    meta: { requiresAuth: false, breadcrumb: "table-bank-account-partner" },
    component: TableBankAccountPartnerView,
  },
  {
    path: "partners/:partnerId/bankaccounts/create",
    name: "create-bank-account-partner",
    meta: { requiresAuth: false, breadcrumb: "create-bank-account-partner" },
    component: CreateBankAccountPartnerView,
  },
  {
    path: "partners/:partnerId/bankaccounts/update/:bankAccountId",
    name: "update-bank-account-partner",
    meta: { requiresAuth: false, breadcrumb: "update-bank-account-partner" },
    component: UpdateBankAccountPartnerView,
  },

  //SECTION - Contacts
  {
    path: "partners/:partnerId/contacts",
    name: "table-contact-partner",
    meta: { requiresAuth: true, breadcrumb: "table-contact-partner" },
    component: TableContactView,
  },
  {
    path: "partners/:partnerId/contacts/create",
    name: "partner-contacts-create",
    meta: { requiresAuth: true, breadcrumb: "partner-contacts-create" },
    component: CreateContactView,
  },
  {
    path: "partners/:partnerId/contacts/update/:contactId",
    name: "partner-contacts-update",
    meta: { requiresAuth: true, breadcrumb: "partner-contacts-update" },
    component: UpdateContactView,
  },
];
