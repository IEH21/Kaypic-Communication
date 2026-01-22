using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Web3_kaypic.Services
{
    public class SchedulerHostedService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public SchedulerHostedService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var matchAuto = scope.ServiceProvider.GetRequiredService<MatchAutomationService>();
                    var sondageAuto = scope.ServiceProvider.GetRequiredService<SondageAutomationService>();

                    await matchAuto.EnvoyerRappels24hAsync();
                    await matchAuto.EnvoyerRappelsJourJAsync();
                    await sondageAuto.EnvoyerRappelsSondagesAsync();
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // vérif toutes les heures
            }
        }
    }
}
