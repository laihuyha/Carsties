"use server";

import { Item, ItemDTO } from "@/types/search";
import { revalidatePath, revalidateTag } from "next/cache";
import { agent } from "../api/agent";

const all = async () => {
  const res = await agent.get<Item[]>("/auctions/all");
  return res;
};

const get = async (id: string) => {
  const res = await agent.get<Item>(`/auctions/${id}`);
  return res;
};

const create = async (data: ItemDTO) => {
  const res = await agent.post<Item>("/auctions", data);
  return res;
};

const update = async (id: string, data?: ItemDTO) => {
  const res = await agent.put<Item>(`/auctions/${id}`, data as {});
  revalidatePath(`/auctions/${id}`);
  return res;
};

const deleteById = async (id: string) => {
  const res = await agent.del<any>(`/auctions/${id}`);
  revalidatePath("/");
  revalidateTag("search");
  return res;
};

const placeBidForAuction = async (auctionId: string, amount: number) => {
  const res = await agent.post(`/bids?auctionId=${auctionId}&amount=${amount}`, {});
  return res;
};

export { all, create, get, update, deleteById, placeBidForAuction };
