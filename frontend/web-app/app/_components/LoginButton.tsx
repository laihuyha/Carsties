"use client";

import { Button } from "flowbite-react";
import { signIn } from "next-auth/react";

export const LoginButton = () => {
  return (
    <Button
      outline
      children="Login"
      onClick={() => {
        signIn("id-server", { callbackUrl: "/" });
      }}
    />
  );
};
