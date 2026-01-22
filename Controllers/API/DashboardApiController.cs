using Microsoft.AspNetCore.Mvc;
using Web3_kaypic.Services.Finance;
using Web3_kaypic.Models.Finance.DTOs;

namespace Web3_kaypic.Controllers.Api
{
    // API REST pour le tableau de bord financier
    // Route de base : /api/dashboard
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardApiController : ControllerBase
    {
        // Service dédié aux calculs et statistiques du tableau de bord
        private readonly DashboardService _dashboardService;

        public DashboardApiController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // ============================================================
        //  RÉCUPÉRER LES STATISTIQUES DU TABLEAU DE BORD
        //  POST /api/dashboard/stats
        // ============================================================
        [HttpPost("stats")]
        public async Task<IActionResult> GetDashboard([FromBody] DashboardRequestDto request)
        {
            // Génère les statistiques complètes (totaux, graphiques, récent)
            // selon les filtres : année, mois, catégorie, utilisateur
            var result = await _dashboardService.GetDashboardAsync(request);

            // Retourne les données JSON pour le frontend (React/Vue/JS)
            return Ok(result);
        }
    }
}