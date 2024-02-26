using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Restyrant.Services.Identity
{
    public  static class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        public static IEnumerable<IdentityResource> IdentityResources =>

            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("pizza","PIZZA Server"),
                new ApiScope("read","Read Your Data"),
                new ApiScope("write","write Your Date"),
                new ApiScope("delete","Delete Your Date")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client()
                {
                    ClientId="client",
                    ClientSecrets= { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes={ "read", "write","profile"}
                },
                new Client()
                {
                    ClientId = "pizza",
                    ClientSecrets ={ new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris={ "https://localhost:44387/signin-oidc" },
                    PostLogoutRedirectUris={"https://localhost:44387/signout-callback-oidc" },
                    AllowedScopes =new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Phone,
                        "pizza"
                    }
                }
            };
    }
}
