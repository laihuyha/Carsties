import { SearchResponse } from "@/app/models/search/searchreponse";
import { agent } from "../agent";

export const searchService = {
  search: async () => {
    const res = await agent.get<SearchResponse>("/search");
    return res;
  },
};
