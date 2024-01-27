import { Item } from "@/types/search";
import { agent } from "../agent";

export const auctionService = {
  all: async () => {
    const res = await agent.get<Item[]>("/auctions/all");
    return res;
  },
  get: async (id: string) => {
    const res = await agent.get<Item>(`/auctions/${id}`);
    return res;
  },
};
