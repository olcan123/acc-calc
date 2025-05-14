const TableCompanyView = () => import("@/views/Companies/TableCompanyView.vue");
const CreateCompanyView = () =>
  import("@/views/Companies/CreateCompanyView.vue");
const UpdateCompanyView = () =>
  import("@/views/Companies/UpdateCompanyView.vue");

// Bank Account Company

const TableBankAccountCompanyView = () =>
  import(
    "@/views/Companies/BankAccounts/TableBankAccountCompanyView.vue"
  );
const CreateBankAccountCompanyView = () =>
  import(
    "@/views/Companies/BankAccounts/CreateBankAccountCompanyView.vue"
  );
const UpdateBankAccountCompanyView = () =>
  import(
    "@/views/Companies/BankAccounts/UpdateBankAccountCompanyView.vue"
  );

export default [
  {
    path: "companies",
    name: "table-company",
    meta: { requiresAuth: false, breadcrumb: "companies" },
    component: TableCompanyView,
  },
  {
    path: "companies/create",
    name: "create-company",
    meta: { requiresAuth: false, breadcrumb: "create-company" },
    component: CreateCompanyView,
  },
  {
    path: "companies/update/:id",
    name: "update-company",
    meta: { requiresAuth: false, breadcrumb: "update-company" },
    component: UpdateCompanyView,
  },

  // Bank Account Company
  {
    path: "companies/:companyId/bankaccounts",
    name: "table-bank-account-company",
    meta: { requiresAuth: false, breadcrumb: "table-bank-account-company" },
    component: TableBankAccountCompanyView,
  },
  {
    path: "companies/:companyId/bankaccounts/create",
    name: "create-bank-account-company",
    meta: { requiresAuth: false, breadcrumb: "create-bank-account-company" },
    component: CreateBankAccountCompanyView,
  },
  {
    path: "companies/:companyId/bankaccounts/update/:bankAccountId",
    name: "update-bank-account-company",
    meta: { requiresAuth: false, breadcrumb: "update-bank-account-company" },
    component: UpdateBankAccountCompanyView,
  },
];
