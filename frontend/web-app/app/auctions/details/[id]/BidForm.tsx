"use client";

import { placeBidForAuction } from "@/app/_actions/auction-action";
import FormTextInput from "@/app/_components/FormTextInput";
import { Form } from "@/components/ui/form";
import { useBidStore } from "@/hooks/useBidStore";
import { toUSDFormat } from "@/lib/utils";
import { bidFormSchema } from "@/schema/schema";
import { Bid, FetchResult } from "@/types";
import { zodResolver } from "@hookform/resolvers/zod";
import { FieldValues, useForm } from "react-hook-form";
import { toast } from "sonner";
import { z } from "zod";

type Props = {
  auctionId: string;
  highBid: number;
};
const BidForm = ({ auctionId, highBid }: Props) => {
  const schema = bidFormSchema(highBid);
  const form = useForm<z.infer<typeof schema>>({
    resolver: zodResolver(schema),
    defaultValues: { amount: 0 },
    mode: "onTouched",
  });
  const { handleSubmit, reset } = form;
  const addBid = useBidStore((state) => state.addBid);
  const onSubmit = (data: FieldValues) => {
    placeBidForAuction(auctionId, +data.amount)
      .then((res: FetchResult<any>) => {
        if (res.error) {
          throw res.error;
        } else {
          addBid(res.data);
          reset();
        }
      })
      .catch((e) => toast.error(e.message));
  };
  return (
    <Form {...form}>
      <form onSubmit={handleSubmit(onSubmit)} className="flex items-center order-2 rounded-lg py-2">
        <FormTextInput
          formContext={form}
          type="number"
          name="amount"
          formItemClassName="input-custom text-sm text-gray-600"
          placeholder={`Enter your bid (min amout is ${toUSDFormat(highBid + 1)})`}
          showLabel
        />
      </form>
    </Form>
  );
};

export default BidForm;
