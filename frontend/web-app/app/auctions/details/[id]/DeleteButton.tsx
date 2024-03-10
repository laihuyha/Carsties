"use client";

import { deleteById } from "@/app/_actions/auction-action";
import { Button } from "flowbite-react";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { RiDeleteBack2Line } from "react-icons/ri";

import { toast } from "sonner";

type Props = {
  id: string;
};

const DeleteButton = ({ id }: Props) => {
  const [loading, setLoading] = useState(false);
  const router = useRouter();

  const doDelete = () => {
    setLoading(true);
    deleteById(id)
      .then((e) => {
        if (!e.error) {
          toast.success(`Auction ${id} deleted`);
        } else {
          toast.error(`Error: ${e.error?.message}, status: ${e.error?.status}`);
        }
      })
      .catch((e) => {
        toast.error(`Error: ${e.message}, status: ${e.status}`);
      })
      .finally(() => {
        setLoading(false);
        router.push("/");
      });
  };
  return (
    <>
      <Button outline color="failure" isProcessing={loading} onClick={doDelete}>
        <RiDeleteBack2Line className="h-4 w-4 mr-2" />
        Delete Auction
      </Button>
    </>
  );
};

export default DeleteButton;
