import { AuctionDTO } from "@/app/models/auctions/auctions";
import { agent } from "../agent";

export const auctionService = {
  all: async () => {
    const res = await agent.get<AuctionDTO[]>("/auctions/all");
    return res;
  },
  get: async (id: string) => {
    const res = await agent.get<AuctionDTO>(`/auctions/${id}`);
    return res;
  },
};
