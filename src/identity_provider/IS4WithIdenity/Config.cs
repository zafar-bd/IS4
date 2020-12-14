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
                 new IdentityResource[]
                 {
                new IdentityResources.Profile(),
                new IdentityResources.Phone(),
                new IdentityResources.Email(),
                 new IdentityResource(
           IdentityServerConstants.StandardScopes.OpenId,
             new[] { "sub","role","email","location","phone_number" })
                 };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                 new ApiScope("weather.fullaccess")
            };

        public static IEnumerable<ApiResource> Apis =>
                        new ApiResource[]
                        {
                             new ApiResource("weather", "weather API")
                {
                    Scopes = { "weather.fullaccess" }
                }
                        };

        public static IEnumerable<Client> Clients =>
            new Client[]
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
                        "weather.fullaccess"
                    }
                },
            };
    }
}
