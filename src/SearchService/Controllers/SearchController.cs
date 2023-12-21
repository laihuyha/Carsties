using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Item>>> SearchItem(string searchTerm, int pageNum = 1, int pageSize = 5)
        {
            var query = DB.PagedSearch<Item>().Sort(x => x.Ascending(a => a.Make));

            if (!string.IsNullOrEmpty(searchTerm))
            {
                _ = query.Match(Search.Full, searchTerm).SortByTextScore();
            }
            var (Results, TotalCount, PageCount) = await query.PageNumber(pageNum).PageSize(pageSize).ExecuteAsync();
            return Ok(new { results = Results, pageCount = PageCount, totalCount = TotalCount });
        }
    }
}