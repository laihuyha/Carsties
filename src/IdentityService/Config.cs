using System.Collections.Generic;
using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[] { new IdentityResources.OpenId(), new IdentityResources.Profile() };

    public static IEnumerable<ApiScope> ApiScopes => new ApiScope[] { new("auctionApp", "Auction app full access"), new("scope2") };

    public static IEnumerable<Client> Clients => new Client[]
    {
        new() {
            ClientId = "postman",
            ClientName = "Postman",
            AllowedScopes = {"openid", "profile", "auctionApp"},
            RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
            ClientSecrets = { new Secret("NotASecret".Sha256()) },
            AllowedGrantTypes = {GrantType.ResourceOwnerPassword},
        }
    };
}
