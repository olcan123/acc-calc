<template>
  <aside
    :class="[
      'fixed top-0 left-0 z-40 w-64 h-screen pt-20 transition-transform bg-white dark:bg-gray-800 border-r border-gray-200 dark:border-gray-700',
      sidebarOpen ? 'translate-x-0' : '-translate-x-full sm:translate-x-0',
    ]"
    aria-label="Sidebar"
  >
    <div class="h-full px-3 pb-4 overflow-y-auto">
      <ul class="space-y-2 font-medium">
        <li>
          <button
            @click="toggleGroup('urunler')"
            class="flex items-center w-full p-2 text-gray-900 rounded-lg dark:text-white hover:bg-gray-100 dark:hover:bg-gray-700"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="w-4 h-4 mr-2"
              :class="expandedGroups.urunler ? 'rotate-180' : ''"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M19 9l-7 7-7-7"
              />
            </svg>
            <span>Ürünler</span>
          </button>
          <ul v-if="expandedGroups.urunler" class="pl-4 space-y-2">
            <SidebarMenuItem
              v-for="(route, index) in urunlerRoutes"
              :key="index"
              :to="{ name: route.name }"
            >
              <div class="flex items-center w-full">
                <span class="mr-2">-</span>
                <span>{{ route.label }}</span>
              </div>
            </SidebarMenuItem>
          </ul>
        </li>
        <SidebarMenuItem
          v-for="(route, index) in otherRoutes"
          :key="index"
          :to="{ name: route.name }"
        >
          <div class="flex items-center w-full">
            <span class="mr-2">-</span>
            <span>{{ route.label }}</span>
          </div>
        </SidebarMenuItem>
      </ul>
    </div>
  </aside>
</template>

<script setup>
import { reactive } from "vue";
import SidebarMenuItem from "./SidebarMenuItem.vue";

defineProps({
  sidebarOpen: Boolean,
});

const urunlerRoutes = [
  { name: "table-vat", label: "KDV" },
  { name: "table-unit-of-measure", label: "Ölçü Birimleri" },
  { name: "table-product", label: "Ürünler" },
  { name: "table-category", label: "Kategoriler" },
];

const otherRoutes = [
  { name: "table-warehouse", label: "Depolar" },
  { name: "table-sale", label: "Satışlar" },
  { name: "table-purchase", label: "Satın Alımlar" },
  { name: "table-partner", label: "Partnerler" },
  { name: "table-ledger", label: "Defterler" },
  { name: "table-currency", label: "Dövizler" },
  { name: "table-company", label: "Şirketler" },
  { name: "table-bank", label: "Bankalar" },
  { name: "table-account", label: "Hesaplar" },
];

const expandedGroups = reactive({});

const toggleGroup = (group) => {
  expandedGroups[group] = !expandedGroups[group];
};
</script>
