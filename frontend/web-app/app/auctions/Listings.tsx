import { searchService } from "../api/services/search.service";
import { AuctionCard } from "./AuctionCard";

const Listings = async () => {
  const data = await searchService.search();
  return (
    <div>
      {data &&
        data.results.map((auction) => (
          <AuctionCard auction={auction} key={auction.id} />
        ))}
    </div>
  );
};

export default Listings;
