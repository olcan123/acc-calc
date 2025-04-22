const HomeView = () => import("@/views/HomeView.vue");

export default [
  {
    path: "",
    name: "home",
    meta: { requiresAuth: false, breadcrumb: "home" },
    component: HomeView,
  },
];
