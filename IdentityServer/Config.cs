using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace web3_kaypic.IdentityServer
{
    public class Config
    {
        //1- Identity Resources(infos de l'utilisateur retournées dans le token)
        public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
         new IdentityResources.OpenId(), // obligatoire → identifiant unique (sub)
         new IdentityResources.Profile() // facultatif → nom, prénom, etc.
        };
        // 2 — API Scopes (permissions que les clients peuvent demander)
        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
         new ApiScope("api1", "Mon API sécurisée") // scope utilisé par Swagger
        };
        // 3 — API Resources (représentent les APIs protégées)
        public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
         new ApiResource("api1", "Mon API sécurisée")
         {
         Scopes = { "api1" } // associe l’API à son scope
         }
        };
        // 4 — Clients (applications autorisées à se connecter à IdentityServer)
        public static IEnumerable<Client> Clients =>
        new Client[]
        {
         // ───────────────────────────────
         // CLIENT MVC (si utilisé dans le TP)
         // ───────────────────────────────
         new Client
         {
         ClientId = "mvc.client",
         ClientName = "MVC Application",
         AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
         RequirePkce = true,
         ClientSecrets = { new Secret("secret".Sha256()) },
         RedirectUris =
         {
         // URL du callback après login
         // ⚠ ADAPTER CE PORT au port HTTPS du projet MVC de l'étudiant !
         // Exemple : https://localhost:7217/signin-oidc
         "https://localhost:7217/signin-oidc"
         },
         PostLogoutRedirectUris =
         {
         // URL appelée après déconnexion
         // ⚠ ADAPTER CE PORT également
         "https://localhost:7217/signout-callback-oidc"
         },
         AllowedScopes =
         {
         "api1", // accès API
         IdentityServerConstants.StandardScopes.OpenId, // obligatoire
         IdentityServerConstants.StandardScopes.Profile // infos user
         },
         AllowOfflineAccess = true // refresh tokens
         },


         // CLIENT SWAGGER (obligatoire)
         // ───────────────────────────────
         new Client
         {
         ClientId = "swagger",
         ClientName = "Swagger UI",
         AllowedGrantTypes = GrantTypes.Code, // flow moderne sécurisé
         RequirePkce = true,
         RequireClientSecret = false, // Swagger = client public
         RedirectUris =
         {
         // Page générée automatiquement par Swagger après login
         // ADAPTER CE PORT au port HTTPS du backend API !
         // Exemple : https://localhost:44300/swagger/oauth2-redirect.html
         "https://localhost:44300/swagger/oauth2-redirect.html"
         },
         PostLogoutRedirectUris =
         {
         // Retour dans Swagger après logout
         // ADAPTER CE PORT également
         "https://localhost:44300/swagger/"
         },
         AllowedScopes =
         {
         "api1",
         IdentityServerConstants.StandardScopes.OpenId,
         IdentityServerConstants.StandardScopes.Profile
            },
         AllowAccessTokensViaBrowser = true
         }
 };
    }
}
