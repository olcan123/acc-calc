<template>
    <!-- Header Section -->
    <div class="flex justify-between items-center mb-4">
        <h2 class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight">
            KDV Tablosu
        </h2>

        <button @click="router.push({ name: 'create-vat' })" type="button"
            class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
            Yeni KDV Oluştur
        </button>
    </div>    <!-- DataTable -->
    <DataTable 
        v-model:filters="filters"
        :value="vats" 
        responsiveLayout="scroll" 
        dataKey="id"
        paginator 
        :rows="10" 
        filterDisplay="menu"
        :loading="loading"
        sortMode="multiple"
        :globalFilterFields="['name', 'rate']"
        :emptyMessage="vats?.length === 0 ? 'Henüz hiç KDV oranı eklenmemiş.' : 'Arama kriterlerinize uygun KDV oranı bulunamadı.'"
    >
        <template #header>
            <div class="flex flex-col md:flex-row md:justify-between md:items-center gap-4 p-4 bg-gray-50 dark:bg-gray-800 rounded-lg">
                <div class="flex items-center gap-2">
                    <button
                        @click="clearFilter()"
                        class="flex items-center gap-2 px-3 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-gray-300 dark:hover:bg-gray-600"
                    >
                        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4"></path>
                        </svg>
                        Filtreleri Temizle
                    </button>
                </div>
                <div class="relative">
                    <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                        <svg class="w-4 h-4 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path>
                        </svg>
                    </div>
                    <input
                        v-model="filters['global'].value"
                        type="text"
                        placeholder="Genel arama..."
                        class="block w-full pl-10 pr-3 py-2 border border-gray-300 rounded-md leading-5 bg-white placeholder-gray-500 focus:outline-none focus:placeholder-gray-400 focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
                    />
                </div>
            </div>
        </template>

        <template #loading>
            <div class="flex items-center justify-center p-8">
                <div class="flex items-center space-x-2">
                    <svg class="animate-spin -ml-1 mr-3 h-5 w-5 text-blue-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                    </svg>
                    <span class="text-gray-600 dark:text-gray-400">KDV oranları yükleniyor...</span>
                </div>
            </div>
        </template>

        <template #empty>
            <div class="flex flex-col items-center justify-center p-8 text-gray-500 dark:text-gray-400">
                <svg class="w-16 h-16 mb-4 text-gray-300 dark:text-gray-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path>
                </svg>
                <p class="text-lg font-medium mb-1">{{ vats?.length === 0 ? 'Henüz hiç KDV oranı eklenmemiş' : 'Sonuç bulunamadı' }}</p>
                <p class="text-sm">{{ vats?.length === 0 ? 'İlk KDV oranınızı oluşturmak için yukarıdaki butonu kullanın.' : 'Arama kriterlerinizi değiştirmeyi deneyin.' }}</p>
            </div>
        </template>

        <Column header="ID" style="min-width: 80px">
            <template #body="{ index }">
                {{ index + 1 }}
            </template>
        </Column>
        <Column field="name" header="Ad" sortable style="min-width: 200px">
            <template #filter="{ filterModel }">
                <input
                    v-model="filterModel.value"
                    type="text"
                    placeholder="Ad ara..."
                    class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
                />
            </template>
        </Column>
        <Column field="rate" header="Oran (%)" sortable dataType="numeric" style="min-width: 120px">
            <template #body="slotProps">
                {{ slotProps.data.rate }}%
            </template>
            <template #filter="{ filterModel }">
                <input
                    v-model="filterModel.value"
                    type="number"
                    placeholder="Oran ara..."
                    class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white"
                />
            </template>
        </Column>
        <Column field="isDefault" header="Varsayılan" sortable style="min-width: 120px">
            <template #body="slotProps">
                <span v-if="slotProps.data.isDefault" class="text-green-600">✓</span>
                <span v-else class="text-red-600">✗</span>
            </template>
            <template #filter="{ filterModel }">
                <select
                    v-model="filterModel.value"
                    class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
                >
                    <option value="">Tümü</option>
                    <option value="true">Evet</option>
                    <option value="false">Hayır</option>
                </select>
            </template>
        </Column>
        <Column field="isActive" header="Aktif" sortable style="min-width: 100px">
            <template #body="slotProps">
                <span v-if="slotProps.data.isActive" class="text-green-600">✓</span>
                <span v-else class="text-red-600">✗</span>
            </template>
            <template #filter="{ filterModel }">
                <select
                    v-model="filterModel.value"
                    class="block w-full px-3 py-1 text-sm border border-gray-300 rounded-md focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
                >
                    <option value="">Tümü</option>
                    <option value="true">Evet</option>
                    <option value="false">Hayır</option>
                </select>
            </template>
        </Column>        <!-- İşlemler -->
        <Column header="İşlemler" style="min-width: 150px">
            <template #body="{ data }">
                <div class="flex gap-2">
                    <EditButton @click="editVat(data.id)" />
                    <DeleteButton @click="confirmDeleteVat(data.id)" />
                </div>
            </template>
        </Column>
    </DataTable>

    <!-- ConfirmDialog popup -->
    <ConfirmDialog />
</template>

<script setup>
import { DataTable, Column, ConfirmDialog } from "primevue";
import { FilterMatchMode, FilterOperator } from "@primevue/core/api";
import { useRouter } from "vue-router";
import { ref } from "vue";
import { storeToRefs } from "pinia";
import { useVatStore } from "@/stores/vat.store";
import { useConfirm } from "primevue/useconfirm";
import EditButton from "@/components/UI/Buttons/EditButton.vue";
import DeleteButton from "@/components/UI/Buttons/DeleteButton.vue";

// Router & Store
const router = useRouter();
const vatStore = useVatStore();
const { vats, loading } = storeToRefs(vatStore);

// PrimeVue Confirm service
const confirm = useConfirm();

// Filters
const filters = ref();

const initFilters = () => {
    filters.value = {
        global: { value: null, matchMode: FilterMatchMode.CONTAINS },
        name: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.STARTS_WITH }] },
        rate: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] },
        isDefault: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] },
        isActive: { operator: FilterOperator.AND, constraints: [{ value: null, matchMode: FilterMatchMode.EQUALS }] }
    };
};

initFilters();

const clearFilter = () => {
    initFilters();
};

// Fetch vats
await vatStore.fetchVats();

// Silme işlemi onaylı
const confirmDeleteVat = (vatId) => {
    confirm.require({
        message: "Bu KDV oranını silmek istediğinize emin misiniz?",
        header: "Silme Onayı",
        acceptLabel: "Evet",
        rejectLabel: "Hayır",
        acceptClass: "p-button-danger",
        rejectClass: "p-button-secondary",
        accept: async () => {
            await vatStore.deleteVat(vatId);
        },
        reject: () => {
            console.log("Silme işlemi iptal edildi.");
        },
    });
};

// Düzenleme
const updateVat = (id) => {
    router.push({ name: "update-vat", params: { id } });
};

const editVat = (id) => {
    router.push({ name: "update-vat", params: { id } });
};
</script>
