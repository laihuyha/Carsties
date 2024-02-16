import { Heading } from "@/app/_components/Heading";
import { AuctionForm } from "../_common/AuctionForm";

const CreateAuction = () => {
  return (
    <div className="mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg">
      <Heading title="Sell your car" subtitle="Start selling your car" />
      <AuctionForm />
    </div>
  );
};

export default CreateAuction;
