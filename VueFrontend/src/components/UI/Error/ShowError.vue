<template>
  <div v-if="error && error.length" class="text-red-600 mb-4">
    <ul class="list-disc pl-5 space-y-1">
      <li v-for="(err, index) in error" :key="index">
        {{ err.message }}
      </li>
    </ul>
  </div>
</template>

<script setup>
const props = defineProps({
  error: {
    type: Array,
    default: () => [],
  },
});
</script>

<style scoped>
/* Your styles here */
</style>
