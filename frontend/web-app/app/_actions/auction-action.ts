"use server";

import { Item, ItemDTO } from "@/types/search";
import { agent } from "../api/auth/agent";

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

const update = async (id: string) => {
  // test
  const data = {
    mileage: Math.floor(Math.random() * 100000) + 1,
  };
  const res = await agent.put<Item>(`/auctions/${id}`, data);
  return res;
};

export { all, create, get, update };

