import React from "react";
import { IoCarSportOutline } from "react-icons/io5";

const Navbar = () => {
  return (
    <header className="sticky top-0 z-50 flex justify-between p-5 items-center text-gray-800 shadow-md">
      <div className="flex items-center gap-2 text-3xl font-semibold text-violet-400">
        <IoCarSportOutline size={34} />
        <div>Carsties Auction</div>
      </div>
      <div>Search</div>
      <div>Login</div>
    </header>
  );
};

export default Navbar;
