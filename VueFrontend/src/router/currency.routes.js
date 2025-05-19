const TableCurrencyView = () => import("@/views/Currencies/TableCurrencyView.vue");
const CreateCurrencyView = () => import("@/views/Currencies/CreateCurrencyView.vue");
const UpdateCurrencyView = () => import("@/views/Currencies/UpdateCurrencyView.vue");

export default [
  {
    path: "currencies",
    name: "table-currency",
    meta: { requiresAuth: false, breadcrumb: "currencies" },
    component: TableCurrencyView,
  },
  {
    path: "currencies/create",
    name: "create-currency",
    meta: { requiresAuth: false, breadcrumb: "create-currency" },
    component: CreateCurrencyView,
  },
  {
    path: "currencies/update/:id",
    name: "update-currency",
    meta: { requiresAuth: false, breadcrumb: "update-currency" },
    component: UpdateCurrencyView,
  },
];
