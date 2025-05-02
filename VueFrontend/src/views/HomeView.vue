<script setup>
import { ref } from "vue";
import BaseModal from "@/components/UI/Modal/BaseModal.vue";

const isModalOpen = ref(false);
</script>

<template>
  <button
    @click="isModalOpen = true"
    class="px-5 py-2.5 bg-blue-700 text-white rounded-lg text-sm hover:bg-blue-800"
  >
    Toggle Modal
  </button>

  <BaseModal v-model="isModalOpen" :staticBackdrop="true">
    <template #header>
      <h3 class="text-xl font-semibold text-gray-900 dark:text-white">
        Static Modal
      </h3>
    </template>

    <template #default>
      <p class="text-gray-600 dark:text-gray-300">
        Yeni Avrupa Birliği gizlilik yasaları yürürlüğe giriyor.
      </p>
      <p class="text-gray-600 dark:text-gray-300">
        GDPR, kullanıcıların verilerinin işlenmesine dair yeni haklar getiriyor.
      </p>
    </template>

    <template #footer>
      <button
        class="text-white bg-blue-700 hover:bg-blue-800 rounded-lg text-sm px-5 py-2.5"
        @click="isModalOpen = false"
      >
        Kabul Et
      </button>
      <button
        class="px-5 py-2.5 text-sm text-gray-800 bg-white border border-gray-300 hover:bg-gray-100 dark:bg-gray-800 dark:text-gray-300 dark:border-gray-600 dark:hover:bg-gray-700"
        @click="isModalOpen = false"
      >
        Reddet
      </button>
    </template>
  </BaseModal>
</template>
