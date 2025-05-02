const CreateBankView = () => import("@/views/Banks/CreateBankView.vue");
const TableBankView = () => import("@/views/Banks/TableBankView.vue");
const UpdateBankView = () => import("@/views/Banks/UpdateBankView.vue");

export default [
  {
    path: "banks",
    name: "table-bank",
    meta: { requiresAuth: false, breadcrumb: "banks" },
    component: TableBankView,
  },
  {
    path: "banks/create",
    name: "create-bank",
    meta: { requiresAuth: false, breadcrumb: "create-bank" },
    component: CreateBankView,
  },
  {
    path: "banks/update/:id",
    name: "update-bank",
    meta: { requiresAuth: false, breadcrumb: "update-bank" },
    component: UpdateBankView,
  },
];
