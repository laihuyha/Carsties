import React from "react";
import { AuctionDTO } from "../models/auctions/auctions";

type Props = {
  auction: AuctionDTO;
};

export const AuctionCard = (props: Props) => {
  const { auction } = props;
  return (
    <div>
      <h3>{auction.make}</h3>
     </div>
  );
};
