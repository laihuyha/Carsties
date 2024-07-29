import { type ClassValue, clsx } from "clsx";
import { twMerge } from "tailwind-merge";

const USDollar = new Intl.NumberFormat("en-US", {
  style: "currency",
  currency: "USD",
  maximumFractionDigits: 3,
});

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export const toastOptions = {
  classNames: {
    closeButton: "toast-force-top-right",
  },
};

export const toUSDFormat = (amount: number) => {
  return USDollar.format(amount);
};
