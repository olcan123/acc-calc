<template>
  <li>
    <router-link :to="to" class="sidebar-menu-item">
      <slot></slot>
    </router-link>
  </li>
</template>

<script>
export default {
  name: "SidebarMenuItem",
  props: {
    to: {
      type: Object,
      required: true
    }
  }
};
</script>

<style scoped>
.sidebar-menu-item {
  text-decoration: none;
  color: inherit;
  display: block;
  padding: 10px;
}

.sidebar-menu-item:hover {
  background-color: #f0f0f0;
}
</style>
