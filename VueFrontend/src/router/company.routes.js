const TableCompanyView = () => import("@/views/Companies/TableCompanyView.vue");
const CreateCompanyView = () =>
  import("@/views/Companies/CreateCompanyView.vue");
const UpdateCompanyView = () =>
  import("@/views/Companies/UpdateCompanyView.vue");

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
