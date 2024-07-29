import { toUSDFormat } from "@/lib/utils";
import { AuctionFinished } from "@/types";
import { Item } from "@/types/search";
import Image from "next/image";
import Link from "next/link";

type Props = {
  finishedAuction: AuctionFinished;
  auction: Item;
};

const AuctionFinishedToast = ({ auction, finishedAuction }: Props) => {
  return (
    <Link href={`/auctions/details/${auction.id}`} className="flex flex-col items-center">
      <div className="flex flex-col items-center gap-2">
        <Image src={auction.imageUrl} alt="image" height={80} width={80} className="rounded-lg w-auto h-auto" />
        <div className="flex flex-col">
          <span>
            Auction for {auction.make} {auction.model} has finished
          </span>
          {finishedAuction.itemSold && finishedAuction.amount ? (
            <p>
              Congrats to {finishedAuction.winner} has won this auction for {toUSDFormat(finishedAuction.amount)}
            </p>
          ) : (
            <p>This item did not sell</p>
          )}
        </div>
      </div>
    </Link>
  );
};

export default AuctionFinishedToast;
