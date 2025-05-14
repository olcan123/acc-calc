const TableVatView = () => import("@/views/Products/Vats/TableVatView.vue");
const CreateVatView = () => import("@/views/Products/Vats/CreateVatView.vue");
const UpdateVatView = () => import("@/views/Products/Vats/UpdateVatView.vue");

export default [
  {
    path: "vats",
    name: "table-vat",
    meta: { requiresAuth: false, breadcrumb: "vats" },
    component: TableVatView,
  },
  {
    path: "vats/create",
    name: "create-vat",
    meta: { requiresAuth: false, breadcrumb: "create-vat" },
    component: CreateVatView,
  },
  {
    path: "vats/update/:id",
    name: "update-vat",
    meta: { requiresAuth: false, breadcrumb: "update-vat" },
    component: UpdateVatView,
  },
];
