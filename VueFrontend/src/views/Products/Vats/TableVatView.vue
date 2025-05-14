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
    </div>

    <!-- DataTable -->
    <DataTable :value="vats" responsiveLayout="scroll" dataKey="id">
        <Column header="ID">
            <template #body="{ index }">
                {{ index + 1 }}
            </template>
        </Column>
        <Column field="name" header="Ad" />
        <Column field="rate" header="Oran (%)">
            <template #body="slotProps">
                {{ slotProps.data.rate }}%
            </template>
        </Column>
        <Column field="isDefault" header="Varsayılan">
            <template #body="slotProps">
                <span v-if="slotProps.data.isDefault" class="text-green-600">✓</span>
                <span v-else class="text-red-600">✗</span>
            </template>
        </Column>
        <Column field="isActive" header="Aktif">
            <template #body="slotProps">
                <span v-if="slotProps.data.isActive" class="text-green-600">✓</span>
                <span v-else class="text-red-600">✗</span>
            </template>
        </Column>

        <!-- İşlemler -->
        <Column header="İşlemler" style="width: 180px">
            <template #body="slotProps">
                <div class="flex gap-2">
                    <!-- Düzenle Butonu -->
                    <button @click="updateVat(slotProps.data.id)" type="button"
                        class="text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:focus:ring-yellow-900">
                        Düzenle
                    </button>

                    <!-- Sil Butonu -->
                    <button @click="confirmDeleteVat(slotProps.data.id)" type="button"
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
import { useVatStore } from "@/stores/vat.store";
import { useConfirm } from "primevue/useconfirm";

// Router & Store
const router = useRouter();
const vatStore = useVatStore();
const { vats } = storeToRefs(vatStore);

// PrimeVue Confirm service
const confirm = useConfirm();

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
</script>
