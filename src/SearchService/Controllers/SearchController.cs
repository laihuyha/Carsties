using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.Request.Helper;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Item>>> SearchItem([FromBody] SearchParams searchParams)
        {
            // var (term, page, size, seller, winner, order, filter) = searchParams;
            // var query = DB.PagedSearch<Item>().Sort(x => x.Ascending(a => a.Make));

            // if (!string.IsNullOrEmpty(searchTerm))
            // {
            //     _ = query.Match(Search.Full, searchTerm).SortByTextScore();
            // }
            // var (Results, TotalCount, PageCount) = await query.PageNumber(pageNum).PageSize(pageSize).ExecuteAsync();
            // return Ok(new { results = Results, pageCount = PageCount, totalCount = TotalCount });
            return Ok();
        }
    }
}