<template>
  <transition name="fade">
    <div
      v-if="modelValue"
      class="fixed inset-0 z-50 flex items-center justify-center bg-white/10 backdrop-blur-sm backdrop-brightness-75"
      role="dialog"
      aria-modal="true"
      @keydown.esc="$emit('update:modelValue', false)"
      tabindex="0"
    >
      <!-- Background Click to Close -->
      <div
        class="absolute inset-0"
        @click="$emit('update:modelValue', false)"
      ></div>

      <!-- Modal Box - Made larger with max-w-6xl -->
      <div
        class="relative z-10 w-full max-w-6xl mx-4 bg-white dark:bg-gray-800 rounded-lg shadow-lg"
        @click.stop
      >
        <!-- Header -->
        <div class="flex items-center justify-between p-5 border-b border-gray-200 dark:border-gray-700 rounded-t">
          <h3 class="text-lg font-semibold text-gray-900 dark:text-white">
            Defter Kayıtları
          </h3>

          <!-- Close Button -->
          <button
            @click="$emit('update:modelValue', false)"
            class="text-gray-400 hover:text-gray-900 dark:hover:text-white transition"
          >
            <span class="sr-only">Kapat</span>
            <svg
              class="w-4 h-4"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>        <!-- Body -->
        <div class="p-5 space-y-4 max-h-[calc(100vh-200px)] overflow-y-auto">
          <slot />
        </div>

        <!-- Footer -->
        <div class="flex items-center justify-end gap-3 p-5 border-t border-gray-200 dark:border-gray-700 rounded-b">
          <button
            @click="$emit('update:modelValue', false)"
            type="button"
            class="text-gray-500 bg-white hover:bg-gray-100 focus:ring-4 focus:outline-none focus:ring-blue-300 rounded-lg border border-gray-200 text-sm font-medium px-5 py-2.5 hover:text-gray-900 focus:z-10 dark:bg-gray-700 dark:text-gray-300 dark:border-gray-500 dark:hover:text-white dark:hover:bg-gray-600 dark:focus:ring-gray-600"
          >
            Kapat
          </button>
        </div>
      </div>
    </div>
  </transition>
</template>

<script setup>
defineProps({
  modelValue: { type: Boolean, required: true }
});

defineEmits(['update:modelValue']);
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
