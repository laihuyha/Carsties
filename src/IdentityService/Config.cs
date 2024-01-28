using System;
using System.Collections.Generic;
using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources => [new IdentityResources.OpenId(), new IdentityResources.Profile()];

    public static IEnumerable<ApiScope> ApiScopes => [new("auctionApp", "Auction app full access"), new("scope2")];

    public static IEnumerable<Client> Clients =>
    [
        new() {
            ClientId = "postman",
            ClientName = "Postman",
            AllowedScopes = {"openid", "profile", "auctionApp"},
            RedirectUris = { "https://www.getpostman.com/oauth2/callback" },
            ClientSecrets = { new Secret("NotASecret".Sha256()) },
            AllowedGrantTypes = {GrantType.ResourceOwnerPassword},
        },
        new() {
            ClientId = "nextApp",
            ClientName = "nextApp",
            ClientSecrets = {new Secret("secret".Sha256())},
            AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
            RequirePkce = false,
            RedirectUris = { "http://localhost:3000/api/auth/callback/id-server" },
            AllowOfflineAccess = true,
            AllowedScopes = {"openid", "profile", "auctionApp"},
            AccessTokenLifetime = (int)TimeSpan.FromDays(30).TotalSeconds,
            AlwaysIncludeUserClaimsInIdToken = true,
        }
    ];
}
