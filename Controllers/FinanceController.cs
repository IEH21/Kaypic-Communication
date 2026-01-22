using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web3_kaypic.Services.Finance;
using Web3_kaypic.Models.Finance.DTOs;

namespace Web3_kaypic.Controllers
{
    [Authorize]
    public class FinanceController : Controller
    {
        private readonly ReportService _reportService;

        public FinanceController(ReportService reportService)
        {
            _reportService = reportService;
        }

        // ------------------- PAGE RAPPORTS -------------------
        public IActionResult Reports()
        {
            return View();
        }

        // ------------------- PDF -------------------
        [HttpPost]
        public async Task<IActionResult> ExportPdf([FromBody] ReportRequestDto dto)
        {
            var pdf = await _reportService.GeneratePdfAsync(dto);
            return File(pdf.FileBytes, pdf.ContentType, pdf.FileName);
        }

        // ------------------- EXCEL -------------------
        [HttpPost]
        public async Task<IActionResult> ExportExcel([FromBody] ReportRequestDto dto)
        {
            var excel = await _reportService.GenerateExcelAsync(dto);
            return File(excel.FileBytes, excel.ContentType, excel.FileName);
        }
    }
}