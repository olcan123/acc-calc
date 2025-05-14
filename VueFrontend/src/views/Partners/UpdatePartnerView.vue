<template>
  <!-- Başlık ve Güncelle Butonu -->
  <div
    class="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4"
  >
    <h2 class="text-2xl font-semibold text-gray-800 dark:text-white">
      Partner Güncelle
    </h2>

    <button
      type="submit"
      form="partnerForm"
      :disabled="submitCount > 0 && !meta.valid"
      class="flex items-center justify-center gap-2 text-white font-medium rounded-lg text-sm px-5 py-2.5 focus:outline-none transition w-full sm:w-auto bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800 disabled:bg-green-300 disabled:cursor-not-allowed"
    >
      <span v-if="submitCount > 0 && !meta.valid">🚫</span>
      Güncelle
    </button>
  </div>

  <!-- Form -->
  <form
    @submit="onSubmit"
    id="partnerForm"
    class="grid grid-cols-1 md:grid-cols-2 gap-6"
  >
    <FieldTextInput fieldName="partner.name" labelName="Ad" />
    <FieldTextInput fieldName="partner.tradeName" labelName="Ticari Ünvan" />

    <FieldSelect
      fieldName="partner.partnerType"
      labelName="Partner Tipi"
      :options="[
        { label: 'İşletme', value: 0 },
        { label: 'Bireysel', value: 1 },
        { label: 'Çalışan', value: 2 },
      ]"
    />

    <FieldSelect
      v-if="[0, 1].includes(formValues.partner.partnerType)"
      fieldName="partner.businessPartnerType"
      labelName="İş Partner Tipi"
      :options="[
        { label: 'Tedarikçi', value: 0 },
        { label: 'Müşteri', value: 1 },
        { label: 'Her ikisi', value: 2 },
      ]"
    />

    <FieldTextInput fieldName="partner.identityNumber" labelName="Kimlik No" />
    <FieldTextInput fieldName="partner.vatNumber" labelName="Vergi No" />

    <div class="md:col-span-2 border-t pt-4">
      <h3 class="text-lg font-semibold text-gray-700 dark:text-white mb-2">
        Adres Bilgileri
      </h3>
    </div>

    <FieldTextInput fieldName="address.type" labelName="Adres Tipi" />
    <FieldTextInput fieldName="address.street" labelName="Sokak" />
    <FieldTextInput fieldName="address.city" labelName="Şehir" />
    <FieldTextInput fieldName="address.country" labelName="Ülke" />
    <FieldTextInput fieldName="address.zipCode" labelName="Posta Kodu" />
  </form>
</template>

<script setup>
import { useForm } from "vee-validate";
import { toTypedSchema } from "@vee-validate/zod";
import { watch, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import { storeToRefs } from "pinia";
import { usePartnerStore } from "@/stores/partner.store";
import FieldTextInput from "@/components/Form/FieldTextInput.vue";
import FieldSelect from "@/components/Form/FieldSelect.vue";
import { partnerValidationSchema } from "@/services/validations/partner.validation";

const router = useRouter();
const route = useRoute();
const id = route.params.id;




const partnerStore = usePartnerStore();
const { partner } = storeToRefs(partnerStore);

const {
  handleSubmit,
  values: formValues,
  submitCount,
  meta,
  setFieldValue,
  setValues,
} = useForm({
  validationSchema: toTypedSchema(partnerValidationSchema),
  initialValues: {
    partner: {
      name: "",
      tradeName: "",
      partnerType: 0,
      businessPartnerType: null,
      identityNumber: "",
      vatNumber: "",
    },
    address: {
      type: "",
      street: "",
      city: "",
      country: "",
      zipCode: "",
    },
  },
});

// PartnerType = Çalışan (2) olduğunda businessPartnerType null yapılır
watch(
  () => formValues.partner.partnerType,
  (type) => {
    if (type === 2) {
      setFieldValue("partner.businessPartnerType", null);
    }
  }
);

// Form submit işlemi
const onSubmit = handleSubmit(async (values) => {
  await partnerStore.updatePartner(values);
  router.push({ name: "table-partner" });
});

// Partner detaylarını çek ve forma yerleştir
onMounted(async () => {
  await partnerStore.getPartnerDetails(id);
  const { addressPartners, ...partnerData } = partner.value;

  setValues({
    partner: {
      ...partnerData,
      partnerType: Number(partnerData.partnerType),
      businessPartnerType: partnerData.businessPartnerType ?? null,
    },
    address: addressPartners?.[0]?.address ?? {
      type: "",
      street: "",
      city: "",
      country: "",
      zipCode: "",
    },
  });
});
</script>
