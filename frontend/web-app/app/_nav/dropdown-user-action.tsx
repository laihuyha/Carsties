"use client";

import { Dropdown, DropdownItem, DropdownItemProps } from "flowbite-react";

import { User } from "next-auth";
import { signOut } from "next-auth/react";
import Link from "next/link";
import { AiFillCar, AiFillTrophy, AiOutlineLogout } from "react-icons/ai";
import { HiCog, HiUser } from "react-icons/hi2";
import { UrlObject } from "url";

type Props = {
  user: Partial<User>;
};

type DropdownItemPropsExtends = DropdownItemProps & {
  isLink?: boolean;
};

type Url = string | UrlObject;

const dropdownAction: DropdownItemPropsExtends[] = [
  {
    icon: HiUser,
    href: "/",
    isLink: true,
    children: "My Auctions",
  },
  {
    icon: AiFillTrophy,
    href: "/",
    isLink: true,
    children: "Auctions won",
  },
  {
    icon: AiFillCar,
    href: "/auctions/create",
    isLink: true,
    children: "Sell my car",
  },
  {
    icon: HiCog,
    href: "/session",
    isLink: true,
    children: "Session (dev only)",
  },
  {
    icon: AiOutlineLogout,
    onClick: () => {
      signOut({ callbackUrl: "/" });
    },
    children: "Sign Out",
  },
];

export const DropdownUserAction = ({ user }: Props) => {
  return (
    <div>
      <Dropdown
        inline
        label={`Welcome ${user.name}`}
        children={dropdownAction.map((e, index) => (
          <DropdownItem
            {...e}
            key={index}
            children={
              e.isLink ? (
                <Link href={e.href as Url}>{e.children}</Link>
              ) : (
                e.children
              )
            }
          />
        ))}
      />
    </div>
  );
};
