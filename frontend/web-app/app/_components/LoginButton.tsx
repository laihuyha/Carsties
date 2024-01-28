"use client";

import { DEFAUTLT_LOGGED_IN_REDIRECT } from "@/routes";
import { Button } from "flowbite-react";
import { signIn } from "next-auth/react";

export default function LoginButton() {
  const onClick = (provider: string, callBackUrl?: string) => {
    signIn(provider, {
      callbackUrl: callBackUrl ?? DEFAUTLT_LOGGED_IN_REDIRECT,
    });
  };

  return <Button outline children="Login" onClick={() => onClick("google")} />;
}
