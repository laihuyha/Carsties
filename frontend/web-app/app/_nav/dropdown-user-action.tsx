"use client";

import { useParamsStore } from "@/hooks/useParamStore";
import { Dropdown, DropdownItem } from "flowbite-react";

import { User } from "next-auth";
import { signOut } from "next-auth/react";
import Link from "next/link";
import { usePathname, useRouter } from "next/navigation";
import { AiFillCar, AiFillTrophy, AiOutlineLogout } from "react-icons/ai";
import { HiCog, HiUser } from "react-icons/hi2";

type Props = {
  user: Partial<User>;
};

export const DropdownUserAction = ({ user }: Props) => {
  const router = useRouter();
  const pathname = usePathname();
  const setParams = useParamsStore((state) => state.setParams);

  const setWinner = () => {
    setParams({ winner: user.name, seller: undefined });
    if (pathname !== "/") router.push("/");
  };

  const setSeller = () => {
    setParams({ seller: user.name, winner: undefined });
    if (pathname !== "/") router.push("/");
  };

  return (
    <div>
      <Dropdown inline label={`Welcome ${user.name}`}>
        <DropdownItem
          key={"1"}
          onClick={setSeller}
          icon={HiUser}
          children="My Auctions"
        />
        <DropdownItem
          key={"2"}
          onClick={setWinner}
          icon={AiFillTrophy}
          children="Auctions won"
        />
        <DropdownItem
          key={"3"}
          icon={AiFillCar}
          children={<Link href={"/auctions/create"}>Sell My Car</Link>}
        />
        <DropdownItem
          key={"4"}
          icon={HiCog}
          children={<Link href={"/session"}>Session (dev only)</Link>}
        />
        <DropdownItem
          key={"5"}
          onClick={() => signOut({ callbackUrl: "/" })}
          icon={AiOutlineLogout}
          children="Sign Out"
        />
      </Dropdown>
    </div>
  );
};
