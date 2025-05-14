const TableProductView = () => import("@/views/Products/TableProductView.vue");
const CreateProductView = () => import("@/views/Products/CreateProductView.vue");
const UpdateProductView = () => import("@/views/Products/UpdateProductView.vue");
const DetailProductView = () => import("@/views/Products/DetailProductView.vue");

export default [
  {
    path: "products",
    name: "table-product",
    meta: { requiresAuth: false, breadcrumb: "products" },
    component: TableProductView,
  },
  {
    path: "products/create",
    name: "create-product",
    meta: { requiresAuth: false, breadcrumb: "create-product" },
    component: CreateProductView,
  },
  {
    path: "products/update/:id",
    name: "update-product",
    meta: { requiresAuth: false, breadcrumb: "update-product" },
    component: UpdateProductView,
  },
  {
    path: "products/detail/:id",
    name: "detail-product",
    meta: { requiresAuth: false, breadcrumb: "detail-product" },
    component: DetailProductView,
  }
];
