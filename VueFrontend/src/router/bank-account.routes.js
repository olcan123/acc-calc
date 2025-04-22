const BankAccountView = () =>
  import("@/views/companies/bank-accounts/TableBankAccountView.vue");

const CreateBankAccountView = () =>
  import("@/views/companies/bank-accounts/CreateBankAccountView.vue");

const UpdateBankAccountView = () =>
  import("@/views/companies/bank-accounts/UpdateBankAccountView.vue");

export default [
  {
    path: "companies/id/:id/bankaccounts",
    name: "table-bank-account",
    meta: { requiresAuth: false, breadcrumb: "bank-accounts" },
    component: BankAccountView,
  },
  {
    path: "companies/id/:id/bankaccounts/create",
    name: "create-bank-account",
    meta: { requiresAuth: false, breadcrumb: "create-bank-account" },
    component: CreateBankAccountView,
  },
  {
    path: "companies/id/:id/bankaccounts/update/:bankAccountId",
    name: "update-bank-account",
    meta: { requiresAuth: false, breadcrumb: "update-bank-account" },
    component: UpdateBankAccountView,
  },
];
