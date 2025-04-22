<template>
  <router-link :to="{ name: to }" class="hover:text-blue-600 dark:hover:text-blue-400">
    <slot />
  </router-link>
</template>

<script setup>
const props = defineProps({
  to: {
    type: String,
    required: true,
  }
});
</script>

<style lang="scss" scoped></style>
