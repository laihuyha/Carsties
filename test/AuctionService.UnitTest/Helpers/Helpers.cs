using System.Collections.Generic;
using System.Security.Claims;

namespace AuctionService.UnitTest.Helpers
{
    public static class Helpers
    {
        public static ClaimsPrincipal GetClaimsPrincipal()
        {
            var claims = new List<Claim> { new(ClaimTypes.Name, "test") };
            var identity = new ClaimsIdentity(claims, "testing");
            return new ClaimsPrincipal(identity);
        }
    }
}