"use client";

import { Button } from "flowbite-react";
import Link from "next/link";
import { RiEdit2Fill } from "react-icons/ri";

type Props = {
  id: string;
};
const EditButton = ({ id }: Props) => {
  return (
    <>
      <Button outline>
        <RiEdit2Fill className="h-4 w-4 mr-2" />
        <Link href={`/auctions/update/${id}`}>Update Auction</Link>
      </Button>
    </>
  );
};

export default EditButton;
