import { BidStatus } from "@/enums";
import { toUSDFormat } from "@/lib/utils";
import { Bid } from "@/types";
import { format } from "date-fns";

type Props = {
  bid: Bid;
};

const BidItem = ({ bid }: Props) => {
  const getBidInfo = () => {
    let bgColor = "";
    let text = "";
    switch (bid.bidStatus) {
      case BidStatus.Accepted.toString():
        bgColor = "bg-green-200";
        text = "Bid Accepted";
        break;
      case BidStatus.AcceptedBelowReserve.toString():
        bgColor = "bg-amber-500";
        text = "Reserve not met";
        break;
      case BidStatus.TooLow.toString():
        bgColor = "bg-red-200";
        text = "Too Low";
        break;
      default:
        bgColor = "bg-red-200";
        text = "Bid placed after auction finished";
        break;
    }
    return { bgColor, text };
  };

  return (
    <div
      className={`border-gray-300 border-2 px-3 py-2 rounded-lg flex justify-between items-center mb-2 ${
        getBidInfo().bgColor
      }`}
    >
      <div className="flex flex-col">
        <span>Bidder: {bid.bidder}</span>
        <span className="text-gray-700 text-sm ">
          Time: {format(new Date(bid.bidTime), "dd MMM yyyy hh:mm")}
        </span>
      </div>
      <div className="flex flex-col text-right">
        <div className="text-xl font-semibold">
          {toUSDFormat(bid.amount)}
        </div>
        <div className="flex flex-row items-center">
          <span>{getBidInfo().text}</span>
        </div>
      </div>
    </div>
  );
};

export default BidItem;