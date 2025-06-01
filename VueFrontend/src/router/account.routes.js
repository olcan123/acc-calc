const TableAccountView = () => import("@/views/Accounts/TableAccountView.vue");
const CreateAccountView = () => import("@/views/Accounts/CreateAccountView.vue");
const UpdateAccountView = () => import("@/views/Accounts/UpdateAccountView.vue");
const CreateParentAccountView = () => import("@/views/Accounts/CreateParentAccountView.vue");
const UpdateParentAccountView = () => import("@/views/Accounts/UpdateParentAccountView.vue");

export default [
  {
    path: "accounts",
    name: "table-account",
    meta: { requiresAuth: false, breadcrumb: "accounts" },
    component: TableAccountView,
  },
  {
    path: "accounts/create",
    name: "create-account",
    meta: { requiresAuth: false, breadcrumb: "create-account" },
    component: CreateAccountView,
  },
  {
    path: "accounts/update/:id",
    name: "update-account",
    meta: { requiresAuth: false, breadcrumb: "update-account" },
    component: UpdateAccountView,
  },
  {
    path: "accounts/parents/create",
    name: "create-parent-account",
    meta: { requiresAuth: false, breadcrumb: "create-parent-account" },
    component: CreateParentAccountView,
  },
  {
    path: "accounts/parents/update/:id",
    name: "update-parent-account",
    meta: { requiresAuth: false, breadcrumb: "update-parent-account" },
    component: UpdateParentAccountView,
  },
];
