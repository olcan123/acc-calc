<template>
  <nav class="fixed top-0 z-50 w-full bg-white border-b border-gray-200 dark:bg-gray-800 dark:border-gray-700">
    <div class="px-3 py-3 lg:px-5 lg:pl-3">
      <div class="flex items-center justify-between">
        <!-- Sol taraf -->
        <div class="flex items-center">
          <button
            type="button"
            class="inline-flex items-center p-2 text-sm text-gray-500 rounded-lg sm:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600"
            @click.stop="$emit('toggle-sidebar')"
          >
            <span class="sr-only">Open sidebar</span>
            <svg class="w-6 h-6" fill="currentColor" viewBox="0 0 20 20">
              <path
                clip-rule="evenodd"
                fill-rule="evenodd"
                d="M2 4.75A.75.75 0 012.75 4h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 4.75zm0 10.5a.75.75 0 01.75-.75h7.5a.75.75 0 010 1.5h-7.5a.75.75 0 01-.75-.75zM2 10a.75.75 0 01.75-.75h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 10z"
              />
            </svg>
          </button>
          <a href="#" class="flex items-center ms-2">
            <img src="https://flowbite.com/docs/images/logo.svg" class="h-8 me-3" />
            <span class="self-center text-xl font-semibold dark:text-white">Flowbite</span>
          </a>
        </div>

        <!-- Orta Menü -->
        <div class="hidden md:flex gap-6 text-sm font-medium text-gray-700 dark:text-white">
          <nav-link to="table-company"> Sirketler </nav-link>
          <a href="#" class="hover:text-blue-600 dark:hover:text-blue-400">About</a>
          <a href="#" class="hover:text-blue-600 dark:hover:text-blue-400">Services</a>
          <a href="#" class="hover:text-blue-600 dark:hover:text-blue-400">Contact</a>
        </div>

        <!-- Sağ taraf: Menü -->
        <div class="flex items-center gap-4">
          <button
            @click="toggleMobileMenu"
            class="md:hidden p-2 text-gray-500 dark:text-gray-400 hover:bg-gray-100 dark:hover:bg-gray-700 rounded"
          >
            <svg v-if="!mobileMenuOpen" class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
            </svg>
            <svg v-else class="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>

          <div class="relative">
            <button
              @click="userMenuOpen = !userMenuOpen"
              class="flex text-sm bg-gray-800 rounded-full focus:ring-4 focus:ring-gray-300 dark:focus:ring-gray-600"
            >
              <span class="sr-only">Open user menu</span>
              <img class="w-8 h-8 rounded-full" src="https://flowbite.com/docs/images/people/profile-picture-5.jpg" />
            </button>
            <div
              v-if="userMenuOpen"
              class="absolute right-0 mt-2 w-48 bg-white rounded-md shadow-lg dark:bg-gray-700 z-50"
            >
              <div class="px-4 py-3">
                <p class="text-sm text-gray-900 dark:text-white">Neil Sims</p>
                <p class="text-sm font-medium text-gray-900 truncate dark:text-gray-300">neil.sims@flowbite.com</p>
              </div>
              <ul class="py-1">
                <li>
                  <a
                    href="#"
                    class="block px-4 py-2 text-sm hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-300"
                    >Dashboard</a
                  >
                </li>
                <li>
                  <a
                    href="#"
                    class="block px-4 py-2 text-sm hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-300"
                    >Settings</a
                  >
                </li>
                <li>
                  <a
                    href="#"
                    class="block px-4 py-2 text-sm hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-300"
                    >Earnings</a
                  >
                </li>
                <li>
                  <a
                    href="#"
                    class="block px-4 py-2 text-sm text-red-600 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-red-400"
                    >Sign out</a
                  >
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>

      <!-- Mobil Menü -->
      <div v-if="mobileMenuOpen" class="mt-4 md:hidden">
        <ul class="flex flex-col gap-2 text-sm font-medium border-t pt-4 border-gray-200 dark:border-gray-700">
          <mobil-navlink to="table-company"> Sirketler </mobil-navlink>
          <li><a href="#" class="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-700">About</a></li>
          <li><a href="#" class="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-700">Services</a></li>
          <li><a href="#" class="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-700">Contact</a></li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script setup>
import { ref } from "vue";
import NavLink from "./NavLink.vue";
import MobilNavlink from "./MobilNavlink.vue";

const mobileMenuOpen = ref(false);
const userMenuOpen = ref(false);

const toggleMobileMenu = () => {
  mobileMenuOpen.value = !mobileMenuOpen.value;
  userMenuOpen.value = false;
};

const closeMenus = () => {
  mobileMenuOpen.value = false;
  userMenuOpen.value = false;
};

defineExpose({
  closeMenus,
});
</script>
