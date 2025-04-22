const TableCompanyView = () => import("@/views/companies/TableCompanyView.vue");
const CreateCompanyView = () =>
  import("@/views/companies/CreateCompanyView.vue");
const UpdateCompanyView = () =>
  import("@/views/companies/UpdateCompanyView.vue");

import bankAccountRoutes from "./bank-account.routes";

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
];
