import { PagedResult } from "@/types";
import { Item } from "@/types/search";
import { shallow } from "zustand/shallow";
import { createWithEqualityFn } from "zustand/traditional";

type State = {
  auctions: Item[];
  totalCount: number;
  pageCount: number;
};

type Action = {
  setData: (data: PagedResult<Item>) => void;
  setCurrentPrice: (auctionId: string, amount: number) => void;
};

const initialState: State = {
  auctions: [],
  pageCount: 0,
  totalCount: 0,
};

export const useAuctionStore = createWithEqualityFn<State & Action>()(
  (set) => ({
    ...initialState,
    setData: (data: PagedResult<Item>) => {
      set(() => ({
        auctions: data.results,
        pageCount: data.pageCount,
        totalCount: data.totalCount,
      }));
    },
    setCurrentPrice: (auctionId: string, amount: number) => {
      set((state) => ({
        auctions: state.auctions.map((auction) =>
          auction.id === auctionId
            ? {
                ...auction,
                currentHighBid: amount,
              }
            : auction
        ),
      }));
    },
  }),
  shallow
);
