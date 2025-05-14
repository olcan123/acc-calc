const TableCategoryView = () => import("@/views/Products/Categories/TableCategoryView.vue");
const CreateCategoryView = () => import("@/views/Products/Categories/CreateCategoryView.vue");
const UpdateCategoryView = () => import("@/views/Products/Categories/UpdateCategoryView.vue");

export default [
  {
    path: "categories",
    name: "table-category",
    meta: { requiresAuth: false, breadcrumb: "categories" },
    component: TableCategoryView,
  },
  {
    path: "categories/create",
    name: "create-category",
    meta: { requiresAuth: false, breadcrumb: "create-category" },
    component: CreateCategoryView,
  },
  {
    path: "categories/update/:id",
    name: "update-category",
    meta: { requiresAuth: false, breadcrumb: "update-category" },
    component: UpdateCategoryView,
  },
];
