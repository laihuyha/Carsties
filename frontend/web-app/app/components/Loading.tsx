"use client";

import { Spinner } from "flowbite-react";

type Props = {
  loading: boolean;
};

export const Loading = ({ loading }: Props) => {
  return (
    loading && (
      <div className="flex flex-wrap gap-2 justify-center items-center">
        <div className="text-center">
          <Spinner aria-label="Center-aligned spinner example" size="xl" />
          <div className="mt-4 text-center select-none">
            <span className="font-extrabold text-2xl pl-4">Loading....</span>
          </div>
        </div>
      </div>
    )
  );
};
