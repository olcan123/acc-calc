<template>
  <div class="relative" ref="autoCompleteWrapper">
    <label
      v-if="showLabel && labelName"
      :for="fieldName"
      class="block mb-1 text-xs font-medium text-gray-700 dark:text-gray-300"
    >
      {{ labelName }}
    </label>

    <input
      type="text"
      :id="fieldName"
      :placeholder="placeholderName"
      v-model="searchQuery"
      @focus="onFocus"
      @input="onInput"
      @keydown.down.prevent="onArrowDown"
      @keydown.up.prevent="onArrowUp"
      @keydown.enter.prevent="onEnter"
      @keydown.esc.prevent="closeDropdown"
      :disabled="disabled"
      class="block w-full p-1.5 text-xs text-gray-900 border border-gray-300 rounded-md focus:ring-blue-500 focus:border-blue-500 focus:outline-none dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500 disabled:bg-gray-100 disabled:text-gray-500 disabled:cursor-not-allowed dark:disabled:bg-gray-600 dark:disabled:text-gray-400"
      autocomplete="off"
    />

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
        </ul>

        <div
          v-if="filteredItems.length === 0 && searchQuery"
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
const activeIndex = ref(-1);
const autoCompleteWrapper = ref(null);
const dropdownStyle = ref({});

const filteredItems = computed(() => {
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
  activeIndex.value = -1;
};

const onFocus = () => openDropdown();

const onInput = () => {
  if (value.value !== null) value.value = null;
  openDropdown();
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
