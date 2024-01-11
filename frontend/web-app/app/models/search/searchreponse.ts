import { Item } from "./item";

export interface SearchResponse {
  results: Item[];
  pageCount: number;
  totalCount: number;
}
