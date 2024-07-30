"use client";

import { useAuctionStore } from "@/hooks/useAuctionStore";
import { useBidStore } from "@/hooks/useBidStore";
import { AuctionFinished, Bid } from "@/types";
import { Item } from "@/types/search";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { User } from "next-auth";
import { ReactNode, useEffect, useState } from "react";
import { toast } from "sonner";
import { get } from "../_actions/auction-action";
import AuctionCreatedToast from "../_components/AuctionCreatedToast";
import AuctionFinishedToast from "../_components/AuctionFinishedToast";
import { env } from "process";

type Props = {
  user: User | null;
  children: ReactNode;
};

const SignalrProvider = ({ children, user }: Props) => {
  const [connection, setConnection] = useState<HubConnection | null>(null);
  const setCurrentPrice = useAuctionStore((state) => state.setCurrentPrice);
  const addBid = useBidStore((e) => e.addBid);

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl(`${env.GATE_WAY_SERVICE_URI}/notifications`)
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          console.log("Connected to notification hub");

          connection.on("Bid Placed", (bid: Bid) => {
            console.log("Bid Placed event receive");
            if (bid.bidStatus.includes("Accepted")) {
              setCurrentPrice(bid.auctionId, bid.amount);
              addBid(bid);
            }
          });

          connection.on("Auction Created", (auction: Item) => {
            if (user?.username !== auction.seller) {
              return toast.message(<AuctionCreatedToast auction={auction} />, {
                duration: 5000,
              });
            }
          });

          connection.on("Auction Finished", (finishedAuction: AuctionFinished) => {
            const auction = get(finishedAuction.auctionId);
            return toast.promise(auction, {
              loading: "Loading......",
              success: (result) => <AuctionFinishedToast finishedAuction={finishedAuction} auction={result.data} />,
              error: (err) => "Auction Finished !",
              duration: 5000,
              icon: null,
            });
          });
        })
        .catch((e) => console.log(e));
    }

    return () => {
      connection?.stop();
    };
  }, [connection, setCurrentPrice]);

  return children;
};

export default SignalrProvider;
