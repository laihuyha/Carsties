"use client";

import { Dropdown } from "flowbite-react";
import React from "react";

type Props = {
  children: React.ReactNode;
};

const DropdownMenu = ({ children }: Props) => {
  return (
    <Dropdown label="Dropdown" placement="right-start">
      {children}
    </Dropdown>
  );
};

export default DropdownMenu;
