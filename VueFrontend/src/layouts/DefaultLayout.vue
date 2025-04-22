<script setup>
import { ref } from "vue";
import AppNavbar from "@/components/Layouts/Navbar/index.vue";
import AppSidebar from "@/components/Layouts/Sidebar/index.vue";

const sidebarOpen = ref(false);
const navbarRef = ref(null); // Navbar referansı

const toggleSidebar = () => {
  sidebarOpen.value = !sidebarOpen.value;
};

const closeAllMenus = () => {
  sidebarOpen.value = false;

  // Navbar içindeki menüleri kapat
  if (navbarRef.value?.closeMenus) {
    navbarRef.value.closeMenus();
  }
};
</script>

<template>
  <div class="min-h-screen bg-white dark:bg-gray-900" @click.self="closeAllMenus">
    <AppNavbar ref="navbarRef" @toggle-sidebar="toggleSidebar" />
    <AppSidebar :sidebarOpen="sidebarOpen" />
    <main class="p-4 pt-20 sm:ml-64">
      <RouterView />
    </main>
  </div>
</template>
