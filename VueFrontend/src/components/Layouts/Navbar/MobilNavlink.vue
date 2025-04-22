<template>
  <li>
    <router-link :to="{ name: to }" class="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-700">
      <slot />
    </router-link>
  </li>
</template>

<script setup>
const props = defineProps({
  to: {
    type: String,
    required: true,
  },
});
</script>

<style lang="scss" scoped></style>
