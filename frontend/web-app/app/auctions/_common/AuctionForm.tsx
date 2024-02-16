"use client";

import { FormDatePicker } from "@/app/_components/FormDatePicker";
import { FormTextInput } from "@/app/_components/FormTextInput";
import { create } from "@/app/_actions/auction-action";
import { Form } from "@/components/ui/form";
import { formSchema } from "@/schema/schema";
import { ItemDTO } from "@/types/search";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "flowbite-react";
import { useRouter } from "next/navigation";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";

export const AuctionForm = () => {
  const router = useRouter();
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

  const onSubmit = async (data: z.infer<typeof formSchema>) => {
    try {
      const res = await create(data);
      router.push(`/auctions/details/${res.id}`);
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    form.setFocus("make");
  }, [form.setFocus]);

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
        <div className="flex justify-between">
          <Button outline color="gray">
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
