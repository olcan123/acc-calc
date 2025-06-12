<template>
  <router-link
    :to="to"
    class="p-2 text-blue-600 hover:text-blue-800 hover:bg-blue-100 dark:hover:bg-blue-900 rounded-lg transition-colors"
    :title="title"
  >
    <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
      <path d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z"></path>
    </svg>
  </router-link>
</template>

<script setup>
const props = defineProps({
  to: {
    type: Object,
    required: true,
  },
  title: {
    type: String,
    default: "Düzenle",
  },
});
</script>
