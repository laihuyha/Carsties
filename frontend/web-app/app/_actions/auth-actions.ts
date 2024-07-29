"use server";

import { Bid } from "@/types";
import { NextApiRequest } from "next";
import { getServerSession } from "next-auth";
import { getToken } from "next-auth/jwt";
import { cookies, headers } from "next/headers";
import { toast } from "sonner";
import { agent } from "../api/agent";
import { authOptions } from "../api/auth/authOptions";

const getSession = async () => {
  return await getServerSession(authOptions);
};

const getTokenWorkaround = async () => {
  const req = {
    headers: Object.fromEntries(headers() as Headers),
    cookies: Object.fromEntries(
      cookies()
        .getAll()
        .map((c) => [c.name, c.value])
    ),
  } as NextApiRequest;

  return await getToken({ req });
};

const getCurrentUser = async () => {
  try {
    const session = await getSession();
    if (!session) return null;
    return session.user;
  } catch (error) {
    return null;
  }
};

const getBidForAuction = async (id: string): Promise<Bid[]> => {
  const response = await agent.get<Bid[]>(`/bids/${id}`);
  if (response.error) {
    toast.error(response.error.message);
    return [];
  }
  return response.data;
};

export { getBidForAuction, getCurrentUser, getSession, getTokenWorkaround };
