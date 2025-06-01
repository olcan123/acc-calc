<template>
  <transition name="fade">
    <div
      v-show="modelValue"
      class="fixed inset-0 z-50 flex items-center justify-center bg-white/10 backdrop-blur-sm backdrop-brightness-75"
      role="dialog"
      aria-modal="true"
      @keydown.esc="closeOnEsc"
      tabindex="0"
    >
      <!-- Background Click to Close (only if not static) -->
      <div
        class="absolute inset-0"
        @click="!staticBackdrop && $emit('update:modelValue', false)"
      ></div>

      <!-- Modal Box -->
      <div
        :class="sizeClasses"
        class="relative z-10 w-full mx-4 bg-white dark:bg-gray-800 rounded-lg shadow-lg"
        @click.stop
      >
        <!-- Header -->
        <div
          v-if="$slots.header"
          class="flex items-center justify-between p-5 border-b border-gray-200 dark:border-gray-700 rounded-t"
        >
          <slot name="header" />

          <!-- Close Button -->
          <button
            v-if="!staticBackdrop"
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
        </div>

        <!-- Body -->
        <div class="p-5 space-y-4">
          <slot />
        </div>

        <!-- Footer -->
        <div
          v-if="$slots.footer"
          class="flex items-center justify-end gap-3 p-5 border-t border-gray-200 dark:border-gray-700 rounded-b"
        >
          <slot name="footer" />
        </div>
      </div>
    </div>
  </transition>
</template>

<script setup>
import { computed } from 'vue';

const props = defineProps({
  modelValue: { type: Boolean, required: true },
  staticBackdrop: { type: Boolean, default: false },
  size: { type: String, default: 'md' }
});

const emit = defineEmits(["update:modelValue"]);

// Modal size classes
const sizeClasses = computed(() => {
  const sizes = {
    sm: 'max-w-md',
    md: 'max-w-2xl',
    lg: 'max-w-4xl',
    xl: 'max-w-6xl'
  };
  return sizes[props.size] || sizes.md;
});

function closeOnEsc(event) {
  if (!props.staticBackdrop) {
    emit("update:modelValue", false);
  }
}
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

/* Ensure modal is hidden when not visible but still in DOM */
[v-show="false"] {
  display: none !important;
}
</style>
