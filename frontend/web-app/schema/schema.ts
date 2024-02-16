import { ItemDTO } from "@/types/search";
import { z } from "zod";

const formSchema = z.object({
  color: z.string(),
  imageUrl: z
    .string()
    .regex(
      /https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)/,
      "Invalid URL"
    ),
  make: z.string().min(1),
  mileage: z.coerce.number().min(0),
  model: z.string().min(1),
  year: z.coerce.number().nonnegative(),
  reservePrice: z.coerce.number().nonnegative(),
  auctionEnd: z.date().min(new Date()),
}) as z.ZodType<ItemDTO>;

export { formSchema };
