<template>
  <div class="auth-layout min-h-screen bg-gray-50 dark:bg-gray-900">
    <div class="flex flex-col justify-center items-center min-h-screen">
      <!-- Header/Logo Section -->
      <div class="mb-8">
        <h1 class="text-3xl font-bold text-gray-900 dark:text-white">
          ACC-CALC
        </h1>
        <p class="text-gray-600 dark:text-gray-400 text-center mt-2">
          Muhasebe ve Hesaplama Sistemi
        </p>
      </div>

      <!-- Main Content -->
      <div class="w-full max-w-md">
        <div class="bg-white dark:bg-gray-800 shadow-lg rounded-lg px-8 py-10">
          <router-view />
        </div>
      </div>

      <!-- Footer -->
      <div class="mt-8 text-center">
        <p class="text-sm text-gray-500 dark:text-gray-400">
          © 2025 ACC-CALC. Tüm hakları saklıdır.
        </p>
      </div>
    </div>
  </div>
</template>

<script setup>
// Authentication layout for login/register pages
import { onMounted } from 'vue'

onMounted(() => {
  // Set dark mode based on user preference or system setting
  const isDark = localStorage.getItem('theme') === 'dark' || 
    (!localStorage.getItem('theme') && window.matchMedia('(prefers-color-scheme: dark)').matches)
  
  if (isDark) {
    document.documentElement.classList.add('dark')
  } else {
    document.documentElement.classList.remove('dark')
  }
})
</script>

<style scoped>
.auth-layout {
  background-image: 
    radial-gradient(circle at 1px 1px, rgba(59, 130, 246, 0.15) 1px, transparent 0);
  background-size: 20px 20px;
}
</style>