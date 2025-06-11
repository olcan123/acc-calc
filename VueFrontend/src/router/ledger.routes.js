const CreateLedgerView = () => import("@/views/Ledgers/CreateLedgerView.vue");
const UpdateLedgerView = () => import("@/views/Ledgers/UpdateLedgerView.vue");
const TableLedgerView = () => import("@/views/Ledgers/TableLedgerView.vue");

const ledgerRoutes = [
  {
    path: "/ledgers",
    name: "Ledgers",
    children: [
      {
        path: "",
        name: "table-ledger",
        component: TableLedgerView,
        meta: {
          title: "Muhasebe Fişleri"
        }
      },
      {
        path: "create",
        name: "create-ledger",
        component: CreateLedgerView,
        meta: {
          title: "Yeni Muhasebe Fişi"
        }
      },
      {
        path: ":id/edit",
        name: "update-ledger",
        component: UpdateLedgerView,
        meta: {
          title: "Muhasebe Fişi Düzenle"
        }
      }
    ]
  }
];

export default ledgerRoutes;
