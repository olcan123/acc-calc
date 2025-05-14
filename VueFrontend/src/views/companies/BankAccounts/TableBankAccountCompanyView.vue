<template>
  <!-- Header Section -->
  <div class="flex justify-between items-center mb-4">
    <h2
      class="text-4xl font-semibold text-gray-800 dark:text-white leading-tight"
    >
      Şirkete Ait Banka Hesapları
    </h2>

    <button
      @click="
        router.push({
          name: 'create-bank-account-company',
          params: { companyId },
        })
      "
      type="button"
      class="flex items-center gap-2 text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
    >
      Yeni Banka Hesabı
    </button>
  </div>

  <!-- ConfirmDialog -->
  <ConfirmDialog />

  <!-- DataTable -->
  <DataTable
    :value="bankAccounts"
    dataKey="id"
    responsiveLayout="scroll"
  >
    <Column header="#" style="width: 60px">
      <template #body="{ index }">
        {{ index + 1 }}
      </template>
    </Column>

    <Column header="Banka">
      <template #body="{ data }">
        {{
          banks.find((bank) => bank.id === data.bankAccount.bankId)?.name || "—"
        }}
      </template>
    </Column>

    <Column field="bankAccount.branchName" header="Şube" />
    <Column field="bankAccount.accountNumber" header="Hesap No" />
    <Column field="bankAccount.iban" header="IBAN" />
    <Column field="bankAccount.swiftCode" header="SWIFT Kodu" />
    <Column field="bankAccount.currency" header="Para Birimi" />

    <Column header="İşlemler" style="width: 180px">
      <template #body="{ data }">
        <div class="flex gap-2">
          <button
            @click="
              router.push({
                name: 'update-bank-account-company',
                params: { companyId, bankAccountId: data.bankAccountId },
              })
            "
            type="button"
            class="text-white bg-yellow-500 hover:bg-yellow-600 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-4 py-2 dark:bg-yellow-400 dark:hover:bg-yellow-500 dark:focus:ring-yellow-600"
          >
            Düzenle
          </button>
          <button
            @click="
              confirmDelete({ companyId, bankAccountId: data.bankAccountId })
            "
            type="button"
            class="text-white bg-red-600 hover:bg-red-700 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-4 py-2 dark:bg-red-500 dark:hover:bg-red-600 dark:focus:ring-red-800"
          >
            Sil
          </button>
        </div>
      </template>
    </Column>
  </DataTable>
</template>

<script setup>
import { useRoute, useRouter } from "vue-router";
import { storeToRefs } from "pinia";
import { useConfirm } from "primevue/useconfirm";
import ConfirmDialog from "primevue/confirmdialog";
import { useBankAccountCompanyStore } from "@/stores/bank-account-company.store";
import { useBankStore } from "@/stores/bank.store";
import DataTable from "primevue/datatable";
import Column from "primevue/column";

const route = useRoute();
const router = useRouter();
const confirm = useConfirm();
const { companyId } = route.params;

const bankAccountStore = useBankAccountCompanyStore();
const bankStore = useBankStore();

const { bankAccounts } = storeToRefs(bankAccountStore);
const { banks } = storeToRefs(bankStore);

const confirmDelete = ({ companyId, bankAccountId }) => {
  confirm.require({
    message: "Bu banka hesabını silmek istediğinize emin misiniz?",
    header: "Onay",
    icon: "pi pi-exclamation-triangle",
    acceptClass: "p-button-danger",
    acceptLabel: "Evet",
    rejectLabel: "Hayır",
    accept: async () => {
      await bankAccountStore.deleteBankAccount(companyId, bankAccountId);
    },
  });
};

await bankStore.fetchBanks();
await bankAccountStore.fetchBankAccounts(companyId);
</script>
