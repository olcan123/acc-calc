<template>
  <div class="dropdown-container" ref="dropdownRef">    <!-- Modern Icon-based Trigger Button with Enhanced Styling -->
    <button
      @click="toggle"
      type="button"
      class="p-2 text-gray-600 hover:text-gray-800 hover:bg-gray-100 dark:text-gray-400 dark:hover:text-gray-200 dark:hover:bg-gray-900 rounded-lg transition-colors"
      :title="buttonText"
    >
      <!-- Three dots (menu) icon -->
      <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
        <path d="M10 12a2 2 0 100-4 2 2 0 000 4zM10 4a2 2 0 100-4 2 2 0 000 4zM10 20a2 2 0 100-4 2 2 0 000 4z"/>
      </svg>
    </button>

    <!-- Enhanced Dropdown Menu -->
    <Teleport to="body">
      <div
        v-if="open"
        ref="dropdownMenuRef"
        class="z-[9999] fixed w-56 rounded-2xl bg-white/95 dark:bg-slate-800/95 backdrop-blur-xl shadow-2xl ring-1 ring-slate-900/5 dark:ring-white/10 overflow-hidden border border-slate-200/50 dark:border-slate-700/50"
        :style="dropdownStyles"
        @click="close"
      >
        <!-- Elegant header with gradient -->
        <div class="px-4 py-3 bg-gradient-to-r from-slate-50 to-slate-100 dark:from-slate-800 dark:to-slate-700 border-b border-slate-200/50 dark:border-slate-600/50">
          <p class="text-xs font-medium text-slate-600 dark:text-slate-300 uppercase tracking-wider">{{ buttonText }}</p>
        </div>
        
        <!-- Menu items container -->
        <div class="py-2">
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

/* Enhanced dropdown item styles */
::v-deep(.dropdown-item) {
  display: flex;
  align-items: center;
  padding: 0.75rem 1rem;
  margin: 0.125rem 0.5rem;
  font-size: 0.875rem;
  font-weight: 500;
  color: #475569; /* slate-600 */
  cursor: pointer;
  transition: all 0.2s ease-out;
  border-radius: 0.75rem;
  text-decoration: none;
  position: relative;
  overflow: hidden;

  &:hover {
    background: linear-gradient(135deg, #f1f5f9 0%, #e2e8f0 100%);
    color: #334155; /* slate-700 */
    transform: translateY(-1px);
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  }

  &:active {
    transform: translateY(0);
  }

  /* Add subtle shine effect on hover */
  &::before {
    content: '';
    position: absolute;
    top: 0;
    left: -100%;
    width: 100%;
    height: 100%;
    background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.3), transparent);
    transition: left 0.5s ease-out;
  }

  &:hover::before {
    left: 100%;
  }

  @media (prefers-color-scheme: dark) {
    color: #cbd5e1; /* slate-300 */
    
    &:hover {
      background: linear-gradient(135deg, #334155 0%, #475569 100%);
      color: #f1f5f9; /* slate-100 */
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
    }

    &::before {
      background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.1), transparent);
    }
  }
}

/* Add icon support for dropdown items */
::v-deep(.dropdown-item-icon) {
  width: 1rem;
  height: 1rem;
  margin-right: 0.75rem;
  opacity: 0.7;
  transition: opacity 0.2s ease-out;
}

::v-deep(.dropdown-item:hover .dropdown-item-icon) {
  opacity: 1;
}

/* Separator styling */
::v-deep(.dropdown-separator) {
  height: 1px;
  margin: 0.5rem 1rem;
  background: linear-gradient(90deg, transparent, #e2e8f0, transparent);
}

@media (prefers-color-scheme: dark) {
  ::v-deep(.dropdown-separator) {
    background: linear-gradient(90deg, transparent, #475569, transparent);
  }
}
</style>
