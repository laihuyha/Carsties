using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Request.Helper;
using static SearchService.Enums.SearchEnums;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Item>>> SearchItem([FromQuery] SearchParams searchParams)
        {
            var query = DB.PagedSearch<Item, Item>();

            if (!string.IsNullOrEmpty(searchParams.SearchTerm))
            {
                _ = query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
            }
            else if (!string.IsNullOrEmpty(searchParams.Seller))
            {
                _ = query.Match(x => x.Seller == searchParams.Seller);
            }
            else if (!string.IsNullOrEmpty(searchParams.Winner))
            {
                _ = query.Match(x => x.Winner == searchParams.Winner);
            }

            query = searchParams.OrderBy?.ToUpper() switch
            {
                nameof(SearchOrderBy.MAKE) => query.Sort(x => x.Ascending(a => a.Make)).Sort(e => e.Ascending(b => b.Model)),
                nameof(SearchOrderBy.NEW) => query.Sort(x => x.Ascending(a => a.CreatedAt)),
                _ => query.Sort(x => x.Ascending(a => a.AuctionEnd)),
            };

            query = searchParams.FilterBy?.ToUpper() switch
            {
                nameof(SearchFilter.FINISHED) => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
                nameof(SearchFilter.ENDINGSOON) => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6) && x.AuctionEnd > DateTime.UtcNow),
                _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow),
            };

            _ = query.PageNumber(searchParams.PageNumber);
            _ = query.PageSize(searchParams.PageSize);

            // Cause ExcuteAsync will return ValueTuple<Results, int, int> so we can use like this.
            var (results, totalCount, pageCount) = await query.PageNumber(searchParams.PageNumber).PageSize(searchParams.PageSize).ExecuteAsync();
            return Ok(new { results, pageCount, totalCount });
        }
    }
}