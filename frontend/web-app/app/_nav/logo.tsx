"use client";

import { useParamsStore } from "@/hooks/useParamStore";
import { AiOutlineCar } from "react-icons/ai";

export const Logo = () => {
  const reset = useParamsStore((state) => state.reset);

  return (
    <div
      onClick={reset}
      className="cursor-pointer flex items-center gap-2 text-2xl font-semibold text-violet-500"
    >
      <AiOutlineCar size={34} />
      <div>Carsties Auctions</div>
    </div>
  );
};
