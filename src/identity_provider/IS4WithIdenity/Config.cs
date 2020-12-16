// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IS4WithIdenity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                 new[]
                 {
                new IdentityResources.OpenId(),
                new IdentityResources.Phone(),
                new IdentityResources.Email(),
                 new IdentityResource(
           "profile",
             new[] { "role","email","location","phone_number" })
                 };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[]
            {
                 new ApiScope("weather.fullaccess"),
                 new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<ApiResource> Apis =>
                        new[]
                        {
                new ApiResource
            {
                Name = "weather",
                DisplayName = "weather API #1",
                Description = "Allow the application to access weather API #1 on your behalf",
                Scopes = new [] {"weather.fullaccess"},
                ApiSecrets = new [] {new Secret("ScopeSecret".Sha256())},
                UserClaims = new [] { "role", "email", "location", "phone_number" }
            },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName    )
                        };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedScopes = { "scope1" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "blazorWASM",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedGrantTypes=  GrantTypes.Code,
                    AllowedCorsOrigins= { "https://localhost:44302" },
                    RedirectUris = { "https://localhost:44302/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://localhost:44302/authentication/logout-callback" },
                    Enabled=true,
                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "weather.fullaccess",
                        IdentityServerConstants.LocalApi.ScopeName
                    }
                },
            };
    }
}
