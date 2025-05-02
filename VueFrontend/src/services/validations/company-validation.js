import { z } from "zod";

// Zod Schema
export const companyValidationSchema = z.object({
    name: z.string()
      .nonempty({ message: "Şirket Adı Boş Olamaz." })
      .max(700, { message: "Şirket Adı En Fazla 700 Karakterden Oluşmalıdır." }),
  
    tradeName: z.string()
      .max(700, { message: "Ticari Şirket Adı En Fazla 700 Karakterden Oluşmalıdır." })
      .optional()
      .or(z.literal("")),
  
      uidNumber: z.string()
    .nonempty({ message: "UID Numara Boş Olamaz." })
    .refine(val => val.startsWith("8"), {
      message: "UID Numara 8 ile başlamalıdır.",
    })
    .refine(val => val.length === 9, {
      message: "UID Numara 9 karakterden oluşmalıdır.",
    }),
  
  
    vatNumber: z.string()
      .optional()
      .refine(
        (val) => !val || (val.startsWith("3") && val.length <= 9),
        { message: "Vergi Numarası 3 ile başlamalı ve en fazla 9 hane olmalıdır." }
      ),
  
    period: z.string()
      .length(4, { message: "Dönem 4 Karakterden Oluşmalıdır." }),
  });