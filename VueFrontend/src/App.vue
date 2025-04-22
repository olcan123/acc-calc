<template>
  <RouterView v-slot="{ Component }">
    <template v-if="Component">
      <Suspense>
        <!-- main content -->
        <component :is="Component"></component>

        <!-- loading state -->
        <template #fallback>
          <Loading />
        </template>
      </Suspense>
    </template>
  </RouterView>
</template>

<script setup>
import { RouterView } from "vue-router";
import Loading from "@/components/loading/index.vue";
</script>
