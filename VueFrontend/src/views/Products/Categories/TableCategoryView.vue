<template>
    <!-- Header Section -->
    <div class="flex justify-between items-center mb-4">
        <h2 class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight">
            Kategori Tablosu
        </h2>

        <button @click="router.push({ name: 'create-category' })" type="button"
            class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
            Yeni Kategori Oluştur
        </button>
    </div>

    <!-- DataTable -->
    <DataTable :value="categories" responsiveLayout="scroll" dataKey="id">
        <Column header="ID">
            <template #body="{ index }">
                {{ index + 1 }}
            </template>
        </Column>
        <Column field="name" header="Kategori Adı" />

        <!-- İşlemler -->
        <Column header="İşlemler" style="width: 180px">
            <template #body="slotProps">
                <div class="flex gap-2">
                    <!-- Düzenle Butonu -->
                    <button @click="updateCategory(slotProps.data.id)" type="button"
                        class="text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:focus:ring-yellow-900">
                        Düzenle
                    </button>

                    <!-- Sil Butonu -->
                    <button @click="confirmDeleteCategory(slotProps.data.id)" type="button"
                        class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900">
                        Sil
                    </button>
                </div>
            </template>
        </Column>
    </DataTable>

    <!-- ConfirmDialog popup -->
    <ConfirmDialog />
</template>

<script setup>
import { DataTable, Column, ConfirmDialog } from "primevue";
import { useRouter } from "vue-router";
import { storeToRefs } from "pinia";
import { useCategoryStore } from "@/stores/category.store";
import { useConfirm } from "primevue/useconfirm";

// Router & Store
const router = useRouter();
const categoryStore = useCategoryStore();
const { categories } = storeToRefs(categoryStore);

// PrimeVue Confirm service
const confirm = useConfirm();

// Fetch categories
await categoryStore.fetchCategories();

// Silme işlemi onaylı
const confirmDeleteCategory = (categoryId) => {
    confirm.require({
        message: "Bu kategoriyi silmek istediğinize emin misiniz?",
        header: "Silme Onayı",
        acceptLabel: "Evet",
        rejectLabel: "Hayır",
        acceptClass: "p-button-danger",
        rejectClass: "p-button-secondary",
        accept: async () => {
            await categoryStore.deleteCategory(categoryId);
        },
        reject: () => {
            console.log("Silme işlemi iptal edildi.");
        },
    });
};

// Düzenleme
const updateCategory = (id) => {
    router.push({ name: "update-category", params: { id } });
};
</script>
