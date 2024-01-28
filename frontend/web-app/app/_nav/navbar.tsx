"use client";

import LoginButton from "../_components/LoginButton";
import { Logo } from "./logo";
import { Search } from "./search";

const Navbar = () => {
  return (
    <header className="sticky top-0 z-50 flex justify-between p-5 items-center text-gray-800 shadow-md">
      <Logo />
      <Search />
      <LoginButton />
    </header>
  );
};

export default Navbar;