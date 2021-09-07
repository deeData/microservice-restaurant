using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.Identity
{
    public static class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        //authentication id info
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };

        //OAuth2 resource access
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope> 
            { 
                new ApiScope("mangoServer","Mango Server"),
                new ApiScope(name: "read", displayName: "Read your data."),
                new ApiScope(name: "write", displayName: "Write your data."),
                new ApiScope(name: "delete", displayName: "Delete your data.")
            };

        //Client requests token from Identity Server
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                //generic client
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "read", "write", "profile"}
                },
                //client for Mango ptpject
                 new Client
                {
                    ClientId = "mango",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    //use sslPort, oidc is open-id-connect
                    RedirectUris = { "https://localhost:44302/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44302/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    { 
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "mangoServer"
                    }
                }

            };
    }
}
