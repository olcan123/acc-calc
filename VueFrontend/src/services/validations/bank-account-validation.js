import { z } from "zod";

export const bankAccountSchema = z.object({
  id: z.number().optional(),
  bankId: z.number().min(1).nullable(),
  branchName: z
    .string()
    .max(200, "Şube adı en fazla 200 karakter olabilir.")
    .optional()
    .or(z.literal("")),

  accountNumber: z
    .string()
    .nonempty("Hesap numarası boş olamaz.")
    .max(50, "Hesap numarası en fazla 50 karakter olabilir."),

  iban: z
    .string()
    .max(30, "IBAN en fazla 30 karakter olabilir.")
    .optional()
    .or(z.literal("")),

  swiftCode: z
    .string()
    .max(20, "SWIFT kodu en fazla 20 karakter olabilir.")
    .optional()
    .or(z.literal("")),

  currency: z
    .string()
    .nonempty("Para birimi boş olamaz.")
    .max(30, "Para birimi en fazla 30 karakter olabilir."),
});
