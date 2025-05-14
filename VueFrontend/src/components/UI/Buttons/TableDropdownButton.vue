<template>
  <div class="dropdown-container" ref="dropdownRef">
    <!-- Trigger Button -->
    <button
      @click="toggle"
      type="button"
      class="inline-flex justify-center w-full rounded-lg bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700 focus:outline-none focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-900"
    >
      {{ buttonText }}
      <svg
        class="-mr-1 ml-2 h-5 w-5"
        xmlns="http://www.w3.org/2000/svg"
        viewBox="0 0 20 20"
        fill="currentColor"
        aria-hidden="true"
      >
        <path
          fill-rule="evenodd"
          d="M5.23 7.21a.75.75 0 011.06.02L10 11.293l3.71-4.06a.75.75 0 111.08 1.04l-4.25 4.65a.75.75 0 01-1.08 0L5.21 8.27a.75.75 0 01.02-1.06z"
          clip-rule="evenodd"
        />
      </svg>
    </button>

    <!-- Dropdown Menu -->
    <Teleport to="body">
      <div
        v-if="open"
        ref="dropdownMenuRef"
        class="z-[9999] fixed w-48 rounded-md bg-white dark:bg-gray-800 shadow-lg ring-1 ring-black ring-opacity-5 overflow-y-auto max-h-[300px]"
        :style="dropdownStyles"
      >
        <div class="py-1 divide-y divide-gray-100 dark:divide-gray-700">
          <slot />
        </div>
      </div>
    </Teleport>
  </div>
</template>

<script setup>
import { ref, onMounted, onBeforeUnmount, nextTick } from "vue";

const props = defineProps({
  buttonText: {
    type: String,
    default: "İşlemler",
  },
});

const open = ref(false);
const dropdownRef = ref(null);
const dropdownMenuRef = ref(null);
const dropdownStyles = ref({});

const toggle = async () => {
  open.value = !open.value;

  if (open.value) {
    await nextTick();

    const rect = dropdownRef.value.getBoundingClientRect();
    const menuHeight = dropdownMenuRef.value?.offsetHeight || 200;
    const screenHeight = window.innerHeight;

    const openUpward = rect.bottom + menuHeight > screenHeight;

    dropdownStyles.value = {
      top: openUpward
        ? `${rect.top - menuHeight + window.scrollY}px`
        : `${rect.bottom + window.scrollY}px`,
      left: `${Math.min(rect.left, window.innerWidth - 200) + window.scrollX}px`,
    };
  }
};

const close = () => (open.value = false);

const onClickOutside = (event) => {
  if (
    dropdownRef.value &&
    !dropdownRef.value.contains(event.target) &&
    dropdownMenuRef.value &&
    !dropdownMenuRef.value.contains(event.target)
  ) {
    close();
  }
};

onMounted(() => {
  document.addEventListener("click", onClickOutside);
});

onBeforeUnmount(() => {
  document.removeEventListener("click", onClickOutside);
});
</script>

<style scoped>
.dropdown-container {
  display: inline-block;
  position: relative;
}

/* Optional: slot içerisine eklenen öğeler için default stil */
::v-deep(.dropdown-item) {
  display: block;
  padding: 0.5rem 1rem;
  font-size: 0.875rem;
  color: #374151; /* gray-700 */
  cursor: pointer;
  transition: background 0.2s;

  &:hover {
    background-color: #e0f2fe; /* tailwind blue-100 */
  }

  @media (prefers-color-scheme: dark) {
    color: #d1d5db; /* gray-300 */
    &:hover {
      background-color: #1f2937; /* gray-800 */
    }
  }
}
</style>
