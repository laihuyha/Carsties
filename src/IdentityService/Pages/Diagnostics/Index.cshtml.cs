using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

namespace IdentityService.Pages.Diagnostics;

[SecurityHeaders]
[Authorize]
public class Index : PageModel
{
    public ViewModel View { get; set; }

    public async Task<IActionResult> OnGet()
    {
        using var scope = HttpContext.RequestServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var cfg = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var allowedIPAddress = cfg.GetSection("AllowedIPAddress").Value.Split(";").ToArray();
        var localAddresses = new string[] { "127.0.0.1", "::1", HttpContext.Connection.LocalIpAddress.ToString() }.Union(allowedIPAddress);
        if (!localAddresses.Contains(HttpContext.Connection.RemoteIpAddress.ToString()))
        {
            return NotFound();
        }

        View = new ViewModel(await HttpContext.AuthenticateAsync());

        return Page();
    }
}