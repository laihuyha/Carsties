"use client";

import { getBidForAuction } from "@/app/_actions/auth-actions";
import Empty from "@/app/_components/Empty";
import Heading from "@/app/_components/Heading";
import { useBidStore } from "@/hooks/useBidStore";
import { useCommonStore } from "@/hooks/useCommonStore";
import { toUSDFormat } from "@/lib/utils";
import { Item } from "@/types/search";
import { User } from "next-auth";
import { useEffect } from "react";
import BidForm from "./BidForm";
import BidItem from "./BidItem";

type Props = {
  user: User | null;
  auction: Item;
};

const BidList = ({ user, auction }: Props) => {
  const setLoading = useCommonStore((state) => state.setLoading);
  const bids = useBidStore((state) => state.bids);
  const setBids = useBidStore((state) => state.setBids);
  const highBid = bids.reduce((prev, current) => (prev > current.amount ? prev : current.amount), 0);

  setLoading(true);

  useEffect(() => {
    getBidForAuction(auction.id)
      .then((e) => setBids(e))
      .finally(() => setLoading(false));
  }, [auction.id, setLoading, setBids]);

  const renderBidInput = () => {
    console.log(user?.username);
    console.log(auction.seller);
    if (!user) {
      return (
        <div className="flex items-center justify-center p-2 text-lg font-semibold">
          Please Login first to continue this action
        </div>
      );
    } else if (user.username === auction.seller) {
      return (
        <div className="flex items-center justify-center p-2 text-lg font-semibold">
          You cannot bid your own auction
        </div>
      );
    } else {
      return <BidForm auctionId={auction.id} highBid={highBid} />;
    }
  };

  return (
    <div className="rounded-lg shadow-md">
      <div className="py-2 px-4 bg-white">
        <div className="sticky top-0 bg-white p-2">
          <Heading title={`Current high bid is : ${toUSDFormat(highBid)}`} />
        </div>
      </div>
      <div className="overflow-auto h-[400px] flex flex-col-reverse px-2">
        {bids.length === 0 ? (
          <Empty title="No bids for this auciton" subtitle="Make a bids?" />
        ) : (
          <>
            {bids.map((e) => (
              <BidItem key={e.id} bid={e} />
            ))}
          </>
        )}
      </div>
      <div className="px-2 pb-2 text-gray-500">{renderBidInput()}</div>
    </div>
  );
};

export default BidList;
