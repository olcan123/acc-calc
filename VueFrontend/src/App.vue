<template>
  <RouterView v-slot="{ Component }">
    <template v-if="Component">
      <Suspense>
        <!-- main content -->
        <component :is="Component"></component>

        <!-- loading state -->
        <template #fallback>
          <div class="flex items-center justify-center w-full h-full py-8">
            <svg
              class="animate-spin h-10 w-10 text-blue-500"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
            >
              <circle
                class="opacity-25"
                cx="12"
                cy="12"
                r="10"
                stroke="currentColor"
                stroke-width="4"
              />
              <path
                class="opacity-75"
                fill="currentColor"
                d="M4 12a8 8 0 018-8v4a4 4 0 00-4 4H4z"
              />
            </svg>
            <span
              class="ml-4 text-gray-700 dark:text-gray-300 text-lg font-medium"
            >
              Yükleniyor...
            </span>
          </div>
        </template>
      </Suspense>
    </template>
  </RouterView>
</template>

<script setup>
import { RouterView } from "vue-router";
</script>
