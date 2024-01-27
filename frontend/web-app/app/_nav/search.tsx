"use client";

import { useParamsStore } from "@/hooks/useParamStore";
import { FaSearch } from "react-icons/fa";

export const Search = () => {
  const store = useParamsStore((state) => ({
    setParams: state.setParams,
    setSearchValue: state.setSearchValue,
    searchValue: state.searchValue,
  }));

  const { searchValue, setParams, setSearchValue } = store;

  function onChange(event: any) {
    setSearchValue(event.target.value);
  }

  function search() {
    setParams({ searchTerm: searchValue });
  }

  return (
    <div className="flex w-[50%] items-center border-2 rounded-full py-2 shadow-sm">
      <input
        onKeyDown={(e: any) => {
          if (e.key === "Enter") search();
        }}
        value={searchValue}
        onChange={onChange}
        type="text"
        placeholder="Search for cars by make, model or color"
        className="flex-grow pl-5 bg-transparent focus:outline-none border-transparent focus:border-transparent focus:ring-0 text-sm text-gray-600"
      />
      <button onClick={search}>
        <FaSearch
          size={34}
          className="bg-red-400 text-white rounded-full p-2 cursor-pointer mx-2"
        />
      </button>
    </div>
  );
};
