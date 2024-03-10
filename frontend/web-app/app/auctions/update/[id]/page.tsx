import Heading from "@/app/_components/Heading";
import AuctionForm from "../../_common/AuctionForm";
import { get } from "@/app/_actions/auction-action";

type Props = {
  id: string;
};

const AuctionUpdate = async ({ params }: { params: Props }) => {
  const res = await get(params.id);
  return (
    <div className="mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg">
      <Heading
        title="Update your auction"
        subtitle="Please update the details for your car"
      />
      <AuctionForm auction={res.data} />
    </div>
  );
};

export default AuctionUpdate;
