const TableWarehouseView = () =>
  import("@/views/companies/warehouses/TableWarehouseView.vue");
const CreateWarehouseView = () =>
  import("@/views/companies/warehouses/CreateWarehouseView.vue");
const UpdateWarehouseView = () =>
  import("@/views/companies/warehouses/UpdateWarehouseView.vue");

export default [
  {
    path: "companies/id/:id/warehouses",
    name: "table-warehouse",
    meta: { requiresAuth: false, breadcrumb: "warehouses" },
    component: TableWarehouseView,
  },
  {
    path: "companies/id/:id/warehouses/create",
    name: "create-warehouse",
    meta: { requiresAuth: false, breadcrumb: "create-warehouse" },
    component: CreateWarehouseView,
  },
  {
    path: "companies/id/:id/warehouses/update/:warehouseId",
    name: "update-warehouse",
    meta: { requiresAuth: false, breadcrumb: "update-warehouse" },
    component: UpdateWarehouseView,
  },
];
