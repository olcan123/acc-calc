<template>
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-10 space-y-10">
        <ConfirmDialog></ConfirmDialog>

        <!-- Ürün Başlık & Butonlar -->
        <div class="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
            <h1 class="text-3xl sm:text-4xl font-bold text-gray-800 dark:text-white flex items-center gap-2">
                📦 Ürün Detayı
            </h1>
            <div class="flex gap-3">
                <router-link :to="{ name: 'table-product' }"
                    class="inline-flex items-center px-4 py-2 text-sm font-medium bg-gray-100 text-gray-800 hover:bg-gray-200 rounded-md border border-gray-300">
                    ← Geri
                </router-link>
                <router-link :to="{ name: 'update-product', params: { id: productId } }"
                    class="inline-flex items-center px-4 py-2 text-sm font-medium bg-yellow-400 hover:bg-yellow-500 text-gray-900 rounded-md">
                    ✏️ Düzenle
                </router-link>
            </div>
        </div>

        <!-- Ürün Bilgisi -->
        <div class="bg-white dark:bg-neutral-800 shadow-md rounded-xl p-6">
            <h2 class="text-lg font-semibold text-blue-600 mb-4">📘 Ürün Bilgisi</h2>
            <div class="grid sm:grid-cols-2 gap-6 text-gray-700 dark:text-gray-200">
                <div>
                    <p><span class="font-medium">Ad:</span> {{ product.name || '-' }}</p>
                    <p><span class="font-medium">Açıklama:</span> {{ product.description || '-' }}</p>
                    <p><span class="font-medium">Ölçü Birimi:</span> {{ product.unitOfMeasure?.name || '-' }}</p>
                </div>
                <div>
                    <p><span class="font-medium">KDV:</span> {{ product.vat?.rate ? `%${product.vat.rate}` : '-' }}</p>
                    <p><span class="font-medium">Gümrük Vergisi:</span> {{ product.customsTaxRate ?
                        `%${product.customsTaxRate}` : '-' }}</p>
                    <p><span class="font-medium">ÖTV:</span> {{ product.exciseTaxRate ? `%${product.exciseTaxRate}` :
                        '-' }}</p>
                </div>
            </div>
        </div>

        <!-- Barkodlar & Kategoriler -->
        <div class="bg-white dark:bg-neutral-800 shadow-md rounded-xl p-6">
            <h2 class="text-lg font-semibold text-green-600 mb-4">🔖 Tanımlayıcılar</h2>
            <div class="grid md:grid-cols-2 gap-6">
                <!-- Barkodlar -->
                <div>
                    <h3 class="text-md font-semibold text-gray-600 dark:text-gray-300 mb-2">📎 Barkodlar</h3>
                    <template v-if="product.barcodes?.length">
                        <ul class="space-y-2">
                            <li v-for="(barcode, index) in product.barcodes" :key="barcode.id"
                                class="flex justify-between items-center bg-gray-100 dark:bg-neutral-700 px-4 py-2 rounded-md text-sm">
                                <span>{{ barcode.code }}</span>
                                <div class="flex items-center gap-2">
                                    <span class="px-2 py-1 text-xs font-semibold bg-gray-800 text-white rounded">{{
                                        barcode.type }}</span>
                                    <button @click="deleteBarcode(barcode.id)"
                                        class="p-2 rounded-full bg-red-100 text-red-600 hover:bg-red-200 transition duration-150"
                                        aria-label="Barkod Sil">
                                        <Trash2 class="w-4 h-4" />
                                    </button>
                                </div>
                            </li>
                        </ul>
                    </template>
                    <div v-else class="text-sm text-gray-400">Barkod bulunamadı</div>
                </div>

                <!-- Kategoriler -->
                <div>
                    <h3 class="text-md font-semibold text-gray-600 dark:text-gray-300 mb-2">🗂 Kategoriler</h3>
                    <template v-if="product.productCategories?.length">
                        <ul class="space-y-2">
                            <li v-for="(cat, index) in product.productCategories" :key="cat.categoryId"
                                class="flex items-center justify-between bg-gray-100 dark:bg-neutral-700 px-4 py-2 rounded-md text-sm">
                                <!-- SOL: Kategori adı -->
                                <span class="text-gray-800 dark:text-gray-100">
                                    {{ cat.category?.name || '-' }}
                                </span>

                                <!-- SAĞ: Numara badge + sil butonu -->
                                <div class="flex items-center gap-3">
                                    <!-- Numara badge -->
                                    <span
                                        class="text-xs font-semibold bg-gray-300 text-gray-800 px-2 py-0.5 rounded-full dark:bg-gray-600 dark:text-white">
                                        #{{ index + 1 }}
                                    </span>

                                    <!-- Sil butonu -->
                                    <button @click="deleteCategory(cat.categoryId)"
                                        class="p-2 rounded-full bg-red-100 text-red-600 hover:bg-red-200 transition duration-150"
                                        aria-label="Sil">
                                        <Trash2 class="w-4 h-4" />
                                    </button>
                                </div>
                            </li>
                        </ul>
                    </template>


                    <div v-else class="text-sm text-gray-400">Kategori bulunamadı</div>
                </div>
            </div>
        </div>

        <!-- Fiyatlar -->
        <div class="bg-white dark:bg-neutral-800 shadow-md rounded-xl p-6">
            <h2 class="text-lg font-semibold text-yellow-600 mb-4">💰 Fiyatlandırma</h2>
            <div v-if="product.productPrices?.length" class="grid sm:grid-cols-2 lg:grid-cols-3 gap-4">
                <div v-for="(price, index) in product.productPrices" :key="price.id"
                    class="p-4 bg-gray-50 dark:bg-neutral-700 rounded-lg border border-gray-200 dark:border-neutral-600">
                    <div class="flex justify-between items-center mb-2">
                        <h4 class="text-sm font-semibold text-gray-700 dark:text-gray-200">
                            #{{ index + 1 }} — {{ price.unitPrice.toFixed(2) }} TL
                        </h4>
                        <button @click="deletePrice(price.id)"
                            class="p-2 rounded-full bg-red-100 text-red-600 hover:bg-red-200 transition duration-150"
                            aria-label="Barkod Sil">
                            <Trash2 class="w-4 h-4" />
                        </button>
                    </div>
                    <div class="flex gap-2 text-xs mb-2">
                        <span
                            class="px-2 py-1 rounded-full bg-gray-200 dark:bg-neutral-600 text-gray-800 dark:text-white">
                            {{ price.category === 0 ? 'Standart' : 'Promosyon' }}
                        </span>
                        <span :class="[
                            'px-2 py-1 rounded-full text-white',
                            price.side === 0 ? 'bg-green-500' : 'bg-red-500'
                        ]">
                            {{ price.side === 0 ? 'Alış' : 'Satış' }}
                        </span>
                    </div>
                    <p class="text-xs text-gray-500 dark:text-gray-400">
                        Başlangıç: {{ price.validFrom ? new Date(price.validFrom).toLocaleDateString() : '-' }}<br />
                        Bitiş: {{ price.validTo ? new Date(price.validTo).toLocaleDateString() : '-' }}
                    </p>
                </div>
            </div>
            <div v-else class="text-sm text-gray-400">Fiyat bilgisi yok</div>
        </div>
    </div>
</template>



<script setup>

import { storeToRefs } from 'pinia';
import { useRoute } from 'vue-router';
import { useProductStore } from '@/stores/product.store';
import { useBarcodeStore } from '@/stores/barcode.store';
import { useProductCategoryStore } from '@/stores/product-category.store';
import { useProductPriceStore } from '@/stores/product-price.store';
import { useConfirm } from 'primevue/useconfirm';
import ConfirmDialog from 'primevue/confirmdialog';
import { Trash2 } from 'lucide-vue-next';

const route = useRoute();
const productId = route.params.id;
const productStore = useProductStore();
const barcodeStore = useBarcodeStore();
const productCategoryStore = useProductCategoryStore();
const productPriceStore = useProductPriceStore();
const confirm = useConfirm();
const { fetchProduct } = productStore;

const { product } = storeToRefs(productStore);

// Barkod silme işlevi
const deleteBarcode = async (barcodeId) => {
    confirm.require({
        message: "Bu barkodu silmek istediğinize emin misiniz?",
        header: "Barkod Silme",
        icon: "pi pi-exclamation-triangle",
        acceptClass: "p-button-danger",
        accept: async () => {
            await barcodeStore.deleteBarcode(barcodeId);

        },
    });
};

// Kategori silme işlevi
const deleteCategory = async (categoryId) => {
    confirm.require({
        message: "Bu kategoriyi silmek istediğinize emin misiniz?",
        header: "Kategori Silme",
        icon: "pi pi-exclamation-triangle",
        acceptClass: "p-button-danger",
        accept: async () => {
            await productCategoryStore.deleteProductCategory(productId, categoryId);

        },
    });
};

// Fiyat silme işlevi
const deletePrice = async (priceId) => {
    confirm.require({
        message: "Bu fiyatı silmek istediğinize emin misiniz?",
        header: "Fiyat Silme",
        icon: "pi pi-exclamation-triangle",
        acceptClass: "p-button-danger",
        accept: async () => {
            await productPriceStore.deleteProductPrice(priceId);

        },
    });
};

await fetchProduct(productId);

</script>
