using Microsoft.AspNetCore.Mvc;
using Web3_kaypic.Models.Finance.DTOs;
using Web3_kaypic.Services.Finance;

namespace web3_kaypic.Controllers.API
{
    // Contrôleur API REST pour la génération de rapports (PDF/Excel)
    [Route("api/[controller]")] // Route de base : /api/ReportApi
    [ApiController]             // Active les fonctionnalités API (validation auto, etc.)
    public class ReportApiController : ControllerBase
    {
        // Service de génération de rapports (PDF et Excel)
        private readonly ReportService _reportService;

        public ReportApiController(ReportService reportService)
        {
            _reportService = reportService;
        }

        // ============================================================
        //  GÉNÉRER RAPPORT PDF
        //  POST /api/ReportApi/pdf
        // ============================================================
        [HttpPost("pdf")]
        public async Task<IActionResult> GeneratePdf([FromBody] ReportRequestDto dto)
        {
            // Génère le fichier PDF selon les critères de filtrage (dates, catégorie)
            var result = await _reportService.GeneratePdfAsync(dto);

            // Retourne le fichier PDF directement en téléchargement
            // Headers automatiques : Content-Type, Content-Disposition
            return File(
                result.FileBytes,     // Contenu binaire du PDF
                result.ContentType,   // "application/pdf"
                result.FileName       // "rapport.pdf"
            );
        }

        // ============================================================
        //  GÉNÉRER RAPPORT EXCEL
        //  POST /api/ReportApi/excel
        // ============================================================
        [HttpPost("excel")]
        public async Task<IActionResult> GenerateExcel([FromBody] ReportRequestDto dto)
        {
            // Génère le fichier Excel avec les transactions filtrées
            var result = await _reportService.GenerateExcelAsync(dto);

            // Retourne le fichier Excel en téléchargement automatique
            return File(
                result.FileBytes,     // Contenu binaire XLSX
                result.ContentType,   // MIME type Excel moderne
                result.FileName       // "rapport.xlsx"
            );
        }
    }
}
