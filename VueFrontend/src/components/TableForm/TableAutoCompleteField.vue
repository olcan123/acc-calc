<template>
  <div class="relative" ref="autoCompleteWrapper">
    <label
      v-if="showLabel && labelName"
      :for="fieldName"
      class="block mb-1 text-xs font-medium text-gray-700 dark:text-gray-300"
    >
      {{ labelName }}
    </label>    <div class="relative">      <input
        type="text"
        :id="fieldName"
        :placeholder="placeholderName"
        v-model="searchQuery"
        @click="toggleDropdown"
        @focus="onFocus"
        @input="onInput"
        @keydown.down.prevent="onArrowDown"
        @keydown.up.prevent="onArrowUp"
        @keydown.enter.prevent="onEnter"
        @keydown.esc.prevent="closeDropdown"
        :disabled="disabled"
        class="block w-full p-1.5 pr-8 text-xs text-gray-900 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500 focus:outline-none dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500 disabled:cursor-not-allowed dark:disabled:bg-gray-600 dark:disabled:text-gray-400"
        autocomplete="off"
      />
      
      <!-- Dropdown Toggle Button -->
      <button
        type="button"
        @click="toggleDropdown"
        :disabled="disabled"
        class="absolute inset-y-0 right-0 flex items-center justify-center w-6 text-gray-400 hover:text-gray-600 dark:text-gray-500 dark:hover:text-gray-300 disabled:cursor-not-allowed disabled:opacity-50"
      >
        <svg
          :class="{ 'transform rotate-180': isOpen }"
          class="w-3 h-3 transition-transform duration-150"
          fill="currentColor"
          viewBox="0 0 20 20"
        >
          <path
            fill-rule="evenodd"
            d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z"
            clip-rule="evenodd"
          />
        </svg>
      </button>
    </div>

    <Teleport to="body">
      <div v-if="isOpen">
        <ul
          v-if="filteredItems.length > 0"
          :style="dropdownStyle"
          class="absolute z-50 mt-1 overflow-auto text-xs bg-white border border-gray-300 rounded-md shadow-lg max-h-60 dark:bg-gray-700 dark:border-gray-600"
        >
          <li
            v-for="(item, index) in filteredItems"
            :key="item.value"
            @click="selectItem(item)"
            class="px-3 py-1.5 cursor-pointer hover:bg-blue-50 dark:hover:bg-blue-600"
            :class="{ 'bg-blue-100 dark:bg-blue-800': index === activeIndex }"
          >
            {{ item.label }}
          </li>
        </ul>        <div
          v-if="filteredItems.length === 0 && searchQuery && !showAllItems"
          :style="dropdownStyle"
          class="absolute z-50 p-2 mt-1 text-xs text-gray-500 bg-white border border-gray-300 rounded-md dark:bg-gray-700 dark:border-gray-600 dark:text-gray-400"
        >
          Sonuç bulunamadı.
        </div>
      </div>
    </Teleport>
  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted, onUnmounted, toRefs, nextTick } from 'vue';
import { useField } from 'vee-validate';

const props = defineProps({
  fieldName: {
    type: String,
    required: true,
  },
  labelName: {
    type: String,
    default: '',
  },
  showLabel: {
    type: Boolean,
    default: true,
  },
  placeholderName: {
    type: String,
    default: 'Aramaya başlayın...',
  },
  options: {
    type: Array,
    required: true,
    default: () => [],
  },
  disabled: {
    type: Boolean,
    default: false,
  },
});

const { fieldName, options } = toRefs(props);
const { value } = useField(fieldName.value);

// Emit definition - matches TableFieldSelect pattern
const emit = defineEmits(['change']);

const searchQuery = ref('');
const isOpen = ref(false);
const showAllItems = ref(false);
const activeIndex = ref(-1);
const autoCompleteWrapper = ref(null);
const dropdownStyle = ref({});

const filteredItems = computed(() => {
  // If showing all items (dropdown button clicked), show all options
  if (showAllItems.value) {
    return options.value;
  }
  
  // Otherwise, filter based on search query
  if (!searchQuery.value) return [];
  return options.value.filter(item =>
    item.label.toLowerCase().includes(searchQuery.value.toLowerCase())
  );
});

const updateDropdownPosition = () => {
  if (!autoCompleteWrapper.value || !isOpen.value) return;
  const rect = autoCompleteWrapper.value.getBoundingClientRect();
  dropdownStyle.value = {
    top: `${rect.bottom + window.scrollY}px`,
    left: `${rect.left + window.scrollX}px`,
    width: `${rect.width}px`,
  };
};

const selectItem = (item) => {
  value.value = item.value;
  searchQuery.value = item.label;
  
  // Emit change event that matches TableFieldSelect pattern
  // Create a mock event object with target.value property
  const mockEvent = {
    target: {
      value: item.value
    }
  };
  
  emit('change', mockEvent);
  closeDropdown();
};

const openDropdown = async () => {
  if (props.disabled) return;
  isOpen.value = true;
  await nextTick();
  updateDropdownPosition();
};

const closeDropdown = () => {
  isOpen.value = false;
  showAllItems.value = false;
  activeIndex.value = -1;
};

const toggleDropdown = () => {
  if (props.disabled) return;
  
  if (isOpen.value) {
    closeDropdown();
  } else {
    showAllItems.value = true;
    openDropdown();
  }
};

const onFocus = () => {
  // Don't auto-open on focus, let click handle it
  // This prevents the dropdown from opening when navigating with Tab
};

const onInput = () => {
  if (value.value !== null) value.value = null;
  showAllItems.value = false; // Reset to search mode when typing
  
  // Only open dropdown if it's not already open
  if (!isOpen.value) {
    openDropdown();
  }
};

const onArrowDown = () => {
  if (activeIndex.value < filteredItems.value.length - 1) activeIndex.value++;
};

const onArrowUp = () => {
  if (activeIndex.value > 0) activeIndex.value--;
};

const onEnter = () => {
  if (activeIndex.value >= 0 && filteredItems.value[activeIndex.value]) {
    selectItem(filteredItems.value[activeIndex.value]);
  } else if (filteredItems.value.length > 0) {
    selectItem(filteredItems.value[0]);
  }
};

const handleClickOutside = (event) => {
  if (autoCompleteWrapper.value && !autoCompleteWrapper.value.contains(event.target)) {
    closeDropdown();
  }
};

// Watch for value changes to update search query display
watch(value, (newValue) => {
  if (newValue) {
    const selectedItem = options.value.find(item => item.value === newValue);
    if (selectedItem) searchQuery.value = selectedItem.label;
  } else {
    searchQuery.value = '';
  }
}, { immediate: true });

onMounted(() => {
  document.addEventListener('click', handleClickOutside);
  window.addEventListener('scroll', updateDropdownPosition, true);
  window.addEventListener('resize', updateDropdownPosition);
});

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside);
  window.removeEventListener('scroll', updateDropdownPosition, true);
  window.removeEventListener('resize', updateDropdownPosition);
});
</script>
