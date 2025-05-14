import { z } from "zod";

export const addressValidationSchema = z.object({
  id: z.number().optional(),

  city: z
    .string()
    .nonempty("Şehir boş olamaz.")
    .max(150, "Şehir en fazla 150 karakterden oluşmalıdır."),

  street: z
    .string()
    .nonempty("Sokak Adı boş olamaz.")
    .max(250, "Sokak Adı en fazla 250 karakterden oluşmalıdır."),

  zipCode: z
    .string()
    .nonempty("Posta Kodu boş olamaz.")
    .length(5, "Posta Kodu 5 karakterden oluşmalıdır."),

  country: z
    .string()
    .nonempty("Ülke boş olamaz.")
    .max(150, "Ülke en fazla 150 karakterden oluşmalıdır."),
});
