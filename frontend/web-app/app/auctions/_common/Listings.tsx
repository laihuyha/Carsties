"use client";

import { search } from "@/app/_actions/search-action";
import AppPagination from "@/app/_components/AppPagination";
import Empty from "@/app/_components/Empty";
import Loading from "@/app/_components/Loading";
import { useCommonStore } from "@/hooks/useCommonStore";
import { useParamsStore } from "@/hooks/useParamStore";
import { PagedResult, SearchParams } from "@/types";
import { Item } from "@/types/search";
import { useCallback, useEffect, useState } from "react";
import AuctionCard from "./AuctionCard";
import Filters from "./Filters";

export const Listings = () => {
  const [data, setData] = useState<PagedResult<Item>>();
  const params = useParamsStore((state) => ({
    pageNumber: state.pageNumber,
    pageSize: state.pageSize,
    searchTerm: state.searchTerm,
    orderBy: state.orderBy,
    filterBy: state.filterBy,
    seller: state.seller,
    winner: state.winner,
  }));
  const loading = useCommonStore((state) => state.loading);

  const setParams = useParamsStore((state) => state.setParams);
  const setLoading = useCommonStore((state) => state.setLoading);

  const setPageNumber = (pageNumber: number) => {
    setParams({ pageNumber });
  };

  const searchWithParams = useCallback(
    (params: SearchParams) => {
      setLoading(true);
      search(params)
        .then((res) => {
          setData(res.data);
        })
        .finally(() => setLoading(false));
    },
    [setLoading, setData]
  );

  useEffect(() => {
    if (!data) searchWithParams(params);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [data, searchWithParams]);

  useEffect(() => {
    searchWithParams(params);
  }, [params, searchWithParams]);

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
