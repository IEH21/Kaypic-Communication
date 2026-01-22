using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using web3_kaypic.IdentityServer;
using web3_kaypic.Models;        // ApplicationUser
using web3_kaypic.service;
using Web3_kaypic.Data;          // ApplicationDbContext
using Web3_kaypic.Hub;           // SignalR Hub
using Web3_kaypic.service;
using Web3_kaypic.Settings;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Views + Razor + SignalR
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

// DbContext (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalSqlServerConnection")));

// Identity (UN SEUL enregistrement)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders()
.AddTokenProvider<PhoneNumberTokenProvider<ApplicationUser>>("Phone")
.AddTokenProvider<EmailTokenProvider<ApplicationUser>>("Email");

// Cookie Authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Twilio + Email (MailKit)
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("TwilioSettings"));
builder.Services.AddScoped<ISMSSenderService, SMSSenderService>();
builder.Services.Configure<MailKitOptions>(builder.Configuration.GetSection("Email"));
builder.Services.AddTransient<IEmailSender, MailKitEmailSender>();

// ==========================
// ✅ AUTHENTICATION (SAFE)
// ==========================
var authBuilder = builder.Services.AddAuthentication();

// 🔹 Google OAuth — uniquement si configuré
var googleClientId = builder.Configuration["GoogleAuthSettings:ClientId"];
var googleClientSecret = builder.Configuration["GoogleAuthSettings:ClientSecret"];

if (!string.IsNullOrWhiteSpace(googleClientId) &&
    !string.IsNullOrWhiteSpace(googleClientSecret))
{
    authBuilder.AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
    });
}

// 🔹 JWT Bearer (IdentityServer)
authBuilder.AddJwtBearer("Bearer", options =>
{
    options.Authority = "https://localhost:44300";
    options.Audience = "api1";
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new()
    {
        ValidateAudience = false
    };
});

// IdentityServer (Duende)
builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
})
.AddAspNetIdentity<ApplicationUser>()
.AddDeveloperSigningCredential()
.AddInMemoryIdentityResources(Config.IdentityResources)
.AddInMemoryApiScopes(Config.ApiScopes)
.AddInMemoryApiResources(Config.ApiResources)
.AddInMemoryClients(Config.Clients);

// Swagger + OAuth2 Authorization Code + PKCE
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mon API", Version = "v1" });

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                // Si IdentityServer est ici, laisse 44300. Sinon utilise l�URL d�IdentityServer.
                AuthorizationUrl = new Uri("https://localhost:44300/connect/authorize"),
                TokenUrl = new Uri("https://localhost:44300/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "api1", "API" },
                    { "openid", "Identit�" },
                    { "profile", "Profil" }
                }
            }
        }
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { "api1", "openid", "profile" }
        }
    });
});

// CORS (autorise l�origine de Swagger UI)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSwagger", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("https://localhost:44300")
            .AllowCredentials();
    });
});

//AI
builder.Services.AddHttpClient();
var app = builder.Build();

// Dev/Prod
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");

        c.OAuthClientId("swagger");
        c.OAuthUsePkce();
        c.OAuthScopeSeparator(" ");
        c.OAuthAppName("Swagger UI");

        // IMPORTANT: doit correspondre � l�URL o� Swagger est servi et d�clar�e c�t� IdentityServer client
        c.OAuth2RedirectUrl("https://localhost:44300/swagger/oauth2-redirect.html");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowSwagger");

// Ordre critique
app.UseCookiePolicy();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

// Initialisation des r�les + admin
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roleNames = { "Admin", "Coach", "Tuteur", "Jeune", "Visiteur" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
            await roleManager.CreateAsync(new IdentityRole(roleName));
    }

    var adminEmail = "admin@kaypic.com";
    var adminPassword = "Admin@123"; // � renforcer

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Kaypic",
            LastName = "Admin",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
            await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

// Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<MessageHub>("/messageHub");
app.MapRazorPages();

app.Run();
