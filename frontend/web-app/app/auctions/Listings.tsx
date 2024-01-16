"use client";

import { useParamsStore } from "@/hooks/useParamStore";
import { PagedResult } from "@/types";
import { Item } from "@/types/search";
import { useEffect, useState } from "react";
import { search } from "../api/services/search.service";
import { AppPagination } from "../components/AppPagination";
import { Empty } from "../components/Empty";
import { AuctionCard } from "./AuctionCard";
import { Filters } from "./Filters";
import { Loading } from "../components/Loading";

export const Listings = () => {
  const [data, setData] = useState<PagedResult<Item>>();
  const [loading, setLoading] = useState(true);
  const params = useParamsStore((state) => ({
    pageNumber: state.pageNumber,
    pageSize: state.pageSize,
    searchTerm: state.searchTerm,
    orderBy: state.orderBy,
    filterBy: state.filterBy,
  }));

  const setParams = useParamsStore((state) => state.setParams);

  const setPageNumber = (pageNumber: number) => {
    setParams({ pageNumber });
  };

  useEffect(() => {
    search(params)
      .then((data) => {
        setData(data);
      })
      .finally(() => setLoading(false));
  }, [params, setData]);

  if (loading) return <Loading loading={loading} />;

  return (
    <>
      <Filters />
      {data && data.totalCount > 0 ? (
        <>
          <div className="grid grid-cols-4 gap-6">
            {data.results.map((auction) => (
              <AuctionCard auction={auction} key={auction.id} />
            ))}
          </div>
          <div className="flex justify-center mt-2">
            <AppPagination
              pageChanged={setPageNumber}
              currentPage={params.pageNumber}
              pageCount={data.pageCount}
            />
          </div>
        </>
      ) : (
        <>
          <Empty showReset />
        </>
      )}
    </>
  );
};

export default Listings;
