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

const DropdownUserAction = ({ user }: Props) => {
  const router = useRouter();
  const pathname = usePathname();
  const setParams = useParamsStore((state) => state.setParams);

  const setWinner = () => {
    setParams({ winner: user.username, seller: undefined });
    if (pathname !== "/") router.push("/");
  };

  const setSeller = () => {
    setParams({ seller: user.username, winner: undefined });
    if (pathname !== "/") router.push("/");
  };

  return (
    <div>
      <Dropdown inline label={`Welcome ${user.name}`}>
        <DropdownItem key={"1"} onClick={setSeller} icon={HiUser}>
          My Auction
        </DropdownItem>
        <DropdownItem key={"2"} onClick={setWinner} icon={AiFillTrophy}>
          Auction Won
        </DropdownItem>
        <DropdownItem key={"3"} icon={AiFillCar}>
          <Link href={"/auctions/create"}>Sell My Car</Link>
        </DropdownItem>
        <DropdownItem key={"4"} icon={HiCog}>
          <Link href={"/session"}>Session (dev only)</Link>
        </DropdownItem>
        <DropdownItem
          key={"5"}
          onClick={() => signOut({ callbackUrl: "/" })}
          icon={AiOutlineLogout}
        >
          Sign Out
        </DropdownItem>
      </Dropdown>
    </div>
  );
};
export default DropdownUserAction;
