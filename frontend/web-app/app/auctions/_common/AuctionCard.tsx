"use client";

import { Item } from "@/types/search";
import Image from "next/image";
import Link from "next/link";
import CountDownTimer from "./CountDownTimer";

type Props = {
  auction: Item;
};

export default function AuctionCard({ auction }: Props) {
  return (
    <Link href={`/auctions/details/${auction.id}`} className="group">
      <div className="w-full bg-gray-200 aspect-w-16 aspect-h-10 rounded-lg overflow-hidden">
        <div>
          <Image
            src={
              auction.imageUrl !== ""
                ? auction.imageUrl
                : "https://cdn.pixabay.com/photo/2018/04/23/17/37/auto-3344988_1280.jpg"
            }
            alt={auction.make}
            fill
            priority
            className="object-cover"
            sizes="(max-width:768px) 100vw, (max-width:1200px) 50vw, 25vw"
          />
          <div className="absolute bottom-2 left-2">
            <CountDownTimer auctionEnd={auction.auctionEnd} />
          </div>
        </div>
      </div>
      <div className="flex justify-between items-center mt-4">
        <h3 className="text-gray-700">
          {auction.make} {auction.model}
        </h3>
        <p className="font-semibold text-sm">{auction.year}</p>
      </div>
    </Link>
  );
}
