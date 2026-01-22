//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Web3_kaypic.Data;
//using ExempleAuthentification.Models;

//[assembly: HostingStartup(typeof(Web3_kaypic.Areas.Identity.IdentityHostingStartup))]
//namespace Web3_kaypic.Areas.Identity
//{
//    public class IdentityHostingStartup : IHostingStartup
//    {
//        public void Configure(IWebHostBuilder builder)
//        {
//            builder.ConfigureServices((context, services) =>
//            {
//                // Ajout du DbContext Identity
//                services.AddDbContext<ApplicationDbContext>(options =>
//                    options.UseSqlServer(
//                        context.Configuration.GetConnectionString("DefaultConnect")));

//                // Configuration Identity + MFA
//                services.AddDefaultIdentity<ApplicationUser>(options =>
//                    {
//                        options.SignIn.RequireConfirmedAccount = false;
//                    })
//                    .AddEntityFrameworkStores<ApplicationDbContext>()
//                    .AddTokenProvider<PhoneNumberTokenProvider<ApplicationUser>>("Phone")
//                    .AddTokenProvider<EmailTokenProvider<ApplicationUser>>("Email");
//            });
//        }
//    }
//}