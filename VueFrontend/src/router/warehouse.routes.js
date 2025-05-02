const CreateWarehouseView = () =>
  import("@/views/Warehouses/CreateWarehouseView.vue");
const TableWarehouseView = () =>
  import("@/views/Warehouses/TableWarehouseView.vue");
const UpdateWarehouseView = () =>
  import("@/views/Warehouses/UpdateWarehouseView.vue");

//Contacts
const CreateContactView = () =>
  import("@/views/Warehouses/Contacts/CreateContactView.vue");

const TableContactView = () =>
  import("@/views/Warehouses/Contacts/TableContactView.vue");

const UpdateContactView = () =>
  import("@/views/Warehouses/Contacts/UpdateContactView.vue");

export default [
  {
    path: "warehouses/create",
    name: "create-warehouse",
    meta: { requiresAuth: true, breadcrumb: "create-warehouse" },
    component: CreateWarehouseView,
  },
  {
    path: "warehouses",
    name: "table-warehouse",
    meta: { requiresAuth: true, breadcrumb: "table-warehouse" },
    component: TableWarehouseView,
  },
  {
    path: "warehouses/update/:id",
    name: "update-warehouse",
    meta: { requiresAuth: true, breadcrumb: "update-warehouse" },
    component: UpdateWarehouseView,
  },

  //Contacts
  {
    path: "warehouses/:warehouseId/contacts",
    name: "table-contact-warehouse",
    meta: { requiresAuth: true, breadcrumb: "table-contact-warehouse" },
    component: TableContactView,
  },
  {
    path: "warehouses/:warehouseId/contacts/create",
    name: "warehouse-contacts-create",
    meta: { requiresAuth: true, breadcrumb: "warehouse-contacts-create" },
    component: CreateContactView,
  },
  {
    path: "warehouses/:warehouseId/contacts/update/:contactId",
    name: "warehouse-contacts-update",
    meta: { requiresAuth: true, breadcrumb: "warehouse-contacts-update" },
    component: UpdateContactView,
  }
];
