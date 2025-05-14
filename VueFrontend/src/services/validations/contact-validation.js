// 📁 schemas/contactSchema.js
import { z } from "zod";

// ✅ Contact Schema
export const contactSchema = z.object({
  contact: z.object({
    id: z.number().optional(), // ✅ veya .nullable() ya da z.union([...])
    name: z
      .string()
      .nonempty("İsim boş olamaz.")
      .max(150, "İsim en fazla 150 karakterden oluşmalıdır."),
  }),

  contactDetails: z
    .array(
      z.object({
        // contactId: z.number().min(1, "İletişim ismi boş olamaz."),
        id: z.number().optional(),

        name: z
          .string()
          .nonempty("İsim boş olamaz.")
          .max(150, "İsim en fazla 150 karakterden oluşmalıdır."),

        value: z
          .string()
          .nonempty("Değer boş olamaz.")
          .max(250, "Değer en fazla 250 karakterden oluşmalıdır."),

        description: z
          .string()
          .max(500, "Açıklama en fazla 500 karakterden oluşmalıdır.")
          .optional(),

        isActive: z.boolean().optional(),
        isPrimary: z.boolean().optional(),
      })
    )
    .min(1, "En az bir iletişim detayı girilmelidir."),
});

export default contactSchema;
