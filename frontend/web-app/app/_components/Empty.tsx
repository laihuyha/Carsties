"use client";

import { useParamsStore } from "@/hooks/useParamStore";
import { Button } from "flowbite-react";
import { signIn } from "next-auth/react";
import Heading from "./Heading";

type Props = {
  title?: string;
  subtitle?: string;
  showReset?: boolean;
  showLogin?: boolean;
  callbackUrl?: string;
};

const Empty = ({
  title = "No match for this filter",
  subtitle = "Try changing or removing some of your filters",
  showReset,
  showLogin,
  callbackUrl,
}: Props) => {
  const reset = useParamsStore((state) => state.reset);
  return (
    <div className="h-[40vh] flex flex-col gap-2 justify-center items-center shadow-lg mt-2">
      <Heading title={title} subtitle={subtitle} center />
      <div className="mt-4">
        {showReset && (
          <Button outline onClick={reset}>
            Remove filter
          </Button>
        )}
        {showLogin && (
          <Button onClick={() => signIn("id-server", { callbackUrl })}>
            Login
          </Button>
        )}
      </div>
    </div>
  );
};

export default Empty;
