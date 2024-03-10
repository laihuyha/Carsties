"use client";

import { useParamsStore } from "@/hooks/useParamStore";
import { usePathname, useRouter } from "next/navigation";
import { AiOutlineCar } from "react-icons/ai";

const Logo = () => {
  const router = useRouter();
  const pathname = usePathname();
  const reset = useParamsStore((state) => state.reset);

  const doReset = () => {
    if (pathname !== "/") {
      router.push("/");
    }
    reset();
  };

  return (
    <div
      onClick={doReset}
      className="cursor-pointer flex items-center gap-2 text-2xl font-semibold text-violet-500"
    >
      <AiOutlineCar size={34} />
      <div>Carsties Auctions</div>
    </div>
  );
};

export default Logo;
