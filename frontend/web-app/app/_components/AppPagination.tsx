"use client";

import { Pagination } from "flowbite-react";

type Props = {
  currentPage: number;
  pageCount: number;
  // eslint-disable-next-line no-unused-vars
  pageChanged: (page: number) => void;
};

export default function AppPagination({
  currentPage,
  pageCount,
  pageChanged,
}: Props) {
  return (
    <Pagination
      currentPage={currentPage}
      onPageChange={(page) => pageChanged(page)} // Utilize the 'page' parameter
      totalPages={pageCount}
      layout="pagination"
      showIcons={true}
      className="text-blue-500 mb-5"
    />
  );
}
