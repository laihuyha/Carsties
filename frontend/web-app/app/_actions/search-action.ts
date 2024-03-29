"use server";

import { PagedResult, SearchParams } from "@/types";
import { Item } from "@/types/search";
import { agent } from "../api/agent";

export const search = async (searchParams?: SearchParams) => {
  let params = new URLSearchParams();
  let res;
  if (!searchParams) {
    res = await agent.get<PagedResult<Item>>("/search");
    return res;
  }
  Object.entries(searchParams).forEach(([key, value]) => {
    if (value == null) return;
    if (value.toString() !== "") params.append(key, value.toString());
  });
  res = await agent.get<PagedResult<Item>>(`/search?${params.toString()}`, [
    "search",
  ]);
  return res;
};
