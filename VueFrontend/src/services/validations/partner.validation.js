import { z } from "zod";
import { addressValidationSchema } from "./address-validation";

export const partnerValidationSchema = z.object({
  partner: z.object({
    id: z.number().optional(),

    name: z
      .string()
      .nonempty("Şirket Adı boş olamaz.")
      .max(700, "Şirket Adı en fazla 700 karakterden oluşmalıdır."),

    tradeName: z.string().optional(),

    identityNumber: z
      .string()
      .max(25, "ID (UID) numarası en fazla 25 karakterden oluşmalıdır.")
      .optional(),

    vatNumber: z
      .string()
      .max(25, "KDV numarası en fazla 25 karakterden oluşmalıdır.")
      .optional(),

    partnerType: z.number(),

    businessPartnerType: z
      .number({
        required_error: "Bireysel veya Kurumsal alanı zorunludur.",
      })
      .nullable()
      .refine(
        (val) => val !== null && val !== undefined,
        "Bireysel veya Kurumsal alanı zorunludur."
      )
      .optional(), // opsiyonel ama refine ile kontrol edilecek
  }),

  address: addressValidationSchema,
});
