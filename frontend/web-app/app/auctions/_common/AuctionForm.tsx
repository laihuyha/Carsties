"use client";

import { create, update } from "@/app/_actions/auction-action";
import FormDatePicker from "@/app/_components/FormDatePicker";
import FormTextInput from "@/app/_components/FormTextInput";
import { Form } from "@/components/ui/form";
import { formSchema } from "@/schema/schema";
import { ActionType, FetchResult } from "@/types";
import { Item, ItemDTO } from "@/types/search";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "flowbite-react";
import { usePathname, useRouter } from "next/navigation";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { toast } from "sonner";
import { z } from "zod";

type Props = {
  auction?: Item;
};

const AuctionForm = ({ auction }: Props) => {
  const router = useRouter();
  const pathName = usePathname();
  const defaultValues = {
    color: "",
    imageUrl: "",
    make: "",
    model: "",
    status: "",
    year: 0,
    mileage: 0,
    reservePrice: 0,
    auctionEnd: new Date(),
  } as ItemDTO;

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: { ...defaultValues },
    mode: "onTouched",
  });

  const handleToast = (res: FetchResult<Item>, actionCase: ActionType) => {
    if (res && !res.error && res.data.id) {
      toast.success(`${actionCase} auction ${res.data.id}, redirecting...`);
      setTimeout(() => {
        router.push(`/auctions/details/${res.data.id}`);
      }, 4000);
    } else {
      toast.error(`Status: ${res.error?.status}: ${res.error?.message}`);
    }
  };

  const onSubmit = async (data: z.infer<typeof formSchema>) => {
    let res: FetchResult<Item>;
    if (pathName.includes("create")) {
      res = await create(data);
      handleToast(res, ActionType.CREATE);
    } else {
      if (auction) {
        res = await update(auction.id as string, data);
        res.data.id = auction.id as string;
        handleToast(res, ActionType.UPDATE);
      }
    }
  };

  useEffect(() => {
    if (auction) {
      const {
        make,
        model,
        color,
        mileage,
        year,
        imageUrl,
        auctionEnd,
        reservePrice,
      } = auction;

      const date = new Date(auctionEnd);

      form.reset({
        make,
        model,
        color,
        mileage,
        year,
        imageUrl,
        reservePrice,
        auctionEnd: date,
      });
    }
    form.setFocus("make");
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [form]);

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="flex flex-col mt-3"
      >
        <FormTextInput
          formContext={form}
          name="make"
          placeholder="Which company made it?"
          showLabel
        />
        <FormTextInput
          formContext={form}
          name="model"
          placeholder="What model is it?"
          showLabel
        />
        <FormTextInput
          formContext={form}
          name="color"
          placeholder="What color is it?"
          showLabel
        />
        <div className="grid grid-cols-2 gap-3">
          <FormTextInput
            formContext={form}
            name="year"
            type="number"
            placeholder="When was it made?"
            showLabel
          />
          <FormTextInput
            formContext={form}
            type="number"
            name="mileage"
            placeholder="How far was it travelled?"
            showLabel
          />
        </div>
        <FormTextInput
          formContext={form}
          name="imageUrl"
          placeholder="Image URL if any"
          showLabel
        />
        {pathName.includes("create") && (
          <>
            <div className="grid grid-cols-2 gap-3">
              <FormTextInput
                formContext={form}
                name="reservePrice"
                type="number"
                showLabel
              />
              <FormDatePicker
                formContext={form}
                name="auctionEnd"
                label="Auction End Date"
                showLabel
              />
            </div>
          </>
        )}
        <div className="flex justify-between">
          <Button outline color="gray" onClick={() => router.back()}>
            Cancel
          </Button>
          <Button
            type="submit"
            outline
            color="success"
            isProcessing={form.formState.isSubmitting}
            disabled={!form.formState.isValid}
          >
            Submit
          </Button>
        </div>
      </form>
    </Form>
  );
};

export default AuctionForm;
