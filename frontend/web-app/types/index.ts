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
};
