type Props = {
  id: string;
};

const AuctionUpdate = ({ params }: { params: Props }) => {
  return <div>AuctionUpdate for {params.id}</div>;
};

export default AuctionUpdate;
