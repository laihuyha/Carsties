type Props = {
  id: string;
};

const AuctionDetails = ({ params }: { params: Props }) => {
  return <div>AuctionDetails for {params.id}</div>;
};

export default AuctionDetails;
