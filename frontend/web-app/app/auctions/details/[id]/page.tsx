import { get } from "@/app/_actions/auction-action";
import { getCurrentUser } from "@/app/_actions/auth-actions";
import DropdownMenu from "@/app/_components/DropdownMenu";
import Heading from "@/app/_components/Heading";
import { DropdownItem } from "flowbite-react";
import { User } from "next-auth";
import { revalidatePath } from "next/cache";
import Image from "next/image";
import CountDownTimer from "../../_common/CountDownTimer";
import BidList from "./BidList";
import DeleteButton from "./DeleteButton";
import DetailedSpecs from "./DetailedSpecs";
import EditButton from "./EditButton";

type Props = {
  id: string;
};

const AuctionDetails = async ({ params }: { params: Props }) => {
  revalidatePath(`/auctions/details/${params.id}`);
  const { data } = await get(params.id);
  const user = await getCurrentUser();

  const dropdownItems = (
    <>
      <DropdownItem>
        <EditButton id={params.id} />
      </DropdownItem>
      <DropdownItem>
        <DeleteButton id={params.id} />
      </DropdownItem>
    </>
  );

  return (
    <div>
      <div className="flex justify-between">
        {user?.username == data.seller && (
          <DropdownMenu>{dropdownItems}</DropdownMenu>
        )}
        <Heading title={`${data.make} ${data.model}`} />
        <div className="flex gap-3">
          <h3 className="text-2xl font-semibold">Time remaining :</h3>
          <CountDownTimer auctionEnd={data.auctionEnd} />
        </div>
      </div>
      <div className="grid grid-cols-2 gap-6 mt-3">
        <div className="w-full bg-gray-200 aspect-h-10 aspect-w-16 rounded-lg overflow-hidden">
          <Image
            src={
              data.imageUrl !== ""
                ? data.imageUrl
                : "https://cdn.pixabay.com/photo/2018/04/23/17/37/auto-3344988_1280.jpg"
            }
            alt={data.make}
            fill
            priority
            className="object-cover"
            sizes="(max-width:768px) 100vw, (max-width:1200px) 50vw, 25vw"
          />
        </div>
        <BidList user={(user as User) ?? null} auction={data} />
      </div>
      <div className="mt-3 grid grid-cols-1 rounded-lg">
        <DetailedSpecs auction={data} />
      </div>
    </div>
  );
};

export default AuctionDetails;
