<template>
  <div class="admin-layout">
    <!-- Navigation Sidebar -->
    <nav class="fixed left-0 top-0 h-full w-64 bg-gray-800 text-white">
      <div class="p-4">
        <h2 class="text-xl font-bold">ACC-CALC Admin</h2>
      </div>
      <ul class="mt-4">
        <li><router-link to="/dashboard" class="block px-4 py-2 hover:bg-gray-700">Dashboard</router-link></li>
        <li><router-link to="/sales" class="block px-4 py-2 hover:bg-gray-700">Satışlar</router-link></li>
        <li><router-link to="/purchases" class="block px-4 py-2 hover:bg-gray-700">Satın Almalar</router-link></li>
        <li><router-link to="/products" class="block px-4 py-2 hover:bg-gray-700">Ürünler</router-link></li>
      </ul>
    </nav>

    <!-- Main Content -->
    <main class="ml-64 min-h-screen bg-gray-100">
      <header class="bg-white shadow-sm p-4">
        <h1 class="text-2xl font-semibold text-gray-800">Admin Panel</h1>
      </header>
      <div class="p-6">
        <router-view />
      </div>
    </main>
  </div>
</template>

<script setup>
// Admin layout for authenticated users
</script>

<style scoped>
.admin-layout {
  min-height: 100vh;
}
</style>
