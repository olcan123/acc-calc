<template>  <!-- Header Section -->
  <div class="flex justify-between items-center mb-4">
    <!-- Yazıyı büyüt + y ekseninde hizala -->
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Hesap Planı
    </h2>

    <div class="flex gap-2">
      <button
        @click="router.push({ name: 'create-parent-account' })"
        type="button"
        class="flex items-center gap-2 text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800"
      >
        Yeni Üst Hesap Oluştur
      </button>
      
      <button
        @click="router.push({ name: 'create-account' })"
        type="button"
        class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
      >
        Yeni Hesap Oluştur
      </button>
    </div>
  </div>
  <!-- DataTable -->
  <DataTable
    :value="accounts"
    responsiveLayout="scroll"
    dataKey="id">
    <Column header="Sıra">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>
    <Column field="code" header="Hesap Kodu" sortable />
    <Column field="name" header="Hesap Adı" sortable />
    <Column field="parentAccount.name" header="Üst Hesap" sortable>
      <template #body="slotProps">
        {{ slotProps.data.parentAccount ? slotProps.data.parentAccount.name : '-' }}
      </template>
    </Column>
    <Column header="Hesap Türü" sortable>
      <template #body="slotProps">
        {{ getAccountTypeName(slotProps.data.accountType) }}
      </template>
    </Column>
    <Column header="Normal Bakiye" sortable>
      <template #body="slotProps">
        {{ getNormalBalanceName(slotProps.data.normalBalance) }}
      </template>
    </Column>
    <Column header="Aktif" sortable>
      <template #body="slotProps">
        <span
          :class="
            'px-2 py-1 text-xs font-medium rounded-full',
            slotProps.data.isActive
              ? 'bg-green-100 text-green-800'
              : 'bg-red-100 text-red-800'
          "
        >
          {{ slotProps.data.isActive ? "Evet" : "Hayır" }}
        </span>
      </template>
    </Column>
    <Column header="Kayıt Yapılabilir" sortable>
      <template #body="slotProps">
        <span
          :class="
            'px-2 py-1 text-xs font-medium rounded-full',
            slotProps.data.isPostable
              ? 'bg-green-100 text-green-800'
              : 'bg-red-100 text-red-800'
          "
        >
          {{ slotProps.data.isPostable ? "Evet" : "Hayır" }}
        </span>
      </template>
    </Column>

    <!-- İşlemler -->
    <Column header="İşlemler" :style="{ width: '180px' }">
      <template #body="slotProps">
        <div class="flex gap-2">
          <!-- Düzenle Butonu -->
          <button
            @click="updateAccount(slotProps.data.id)"
            type="button"
            class="text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:focus:ring-yellow-900"
          >
            Düzenle
          </button>

          <!-- Sil Butonu -->
          <button
            @click="confirmDeleteAccount(slotProps.data.id)"
            type="button"
            class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-1.5 focus:outline-none dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900"
          >
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
import { useAccountStore } from "@/stores/account.store";
import { useConfirm } from "primevue/useconfirm";
import { onMounted } from "vue";

// Router & Store
const router = useRouter();
const accountStore = useAccountStore();
const { accounts, loading, accountTypeOptions, normalBalanceOptions } = storeToRefs(accountStore);

// PrimeVue Confirm service
const confirm = useConfirm();

// Fetch accounts
onMounted(async () => {
  await accountStore.fetchAccounts();
});

// Silme işlemi onaylı
const confirmDeleteAccount = (id) => {
  confirm.require({
    message: "Bu hesabı silmek istediğinizden emin misiniz?",
    header: "Silme İşlemi",
    icon: "pi pi-exclamation-triangle",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    accept: () => {
      accountStore.deleteAccount(id);
    },
  });
};

// Düzenleme sayfasına yönlendirme
const updateAccount = (id) => {
  const account = accounts.value.find(acc => acc.id === id);
  
  // Hesap türüne göre farklı güncelleme sayfalarına yönlendir
  if (account && !account.isPostable) {
    // Üst hesap (isPostable=false)
    router.push({ name: "update-parent-account", params: { id } });
  } else {
    // Normal hesap (isPostable=true)
    router.push({ name: "update-account", params: { id } });
  }
};

// Hesap türü adını alma
const getAccountTypeName = (typeId) => {
  const found = accountTypeOptions.value.find((type) => type.value === typeId);
  return found ? found.label : "-";
};

// Normal bakiye adını alma
const getNormalBalanceName = (balanceId) => {
  if (balanceId === null) return "-";
  const found = normalBalanceOptions.value.find((balance) => balance.value === balanceId);
  return found ? found.label : "-";
};
</script>

<style scoped></style>
