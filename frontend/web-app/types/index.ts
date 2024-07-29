export type PagedResult<T> = {
  results: T[];
  pageCount: number;
  totalCount: number;
};

export type SearchParams = {
  pageNumber: number;
  pageSize: number;
  searchTerm: string;
  filterBy: string;
  orderBy: string;
  seller?: string | null;
  winner?: string | null;
};

export type FetchResult<T> = {
  data: T;
  error?: {
    status: number;
    message: string;
  };
};

export type Bid = {
  id: string;
  auctionId: string;
  bidder: string;
  bidTime: string;
  amount: number;
  bidStatus: string;
};

export type AuctionFinished = {
  itemSold: boolean;
  auctionId: string;
  winner: string;
  seller: string;
  amount: number;
};
