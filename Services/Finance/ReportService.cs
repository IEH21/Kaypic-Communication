using Web3_kaypic.Data;
using Web3_kaypic.Models.Finance;
using Web3_kaypic.Models.Finance.DTOs;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace Web3_kaypic.Services.Finance
{
    public class ReportService
    {
        // Contexte de base de données contenant les transactions et catégories
        private readonly ApplicationDbContext _db;

        public ReportService(ApplicationDbContext db)
        {
            _db = db;
        }

        // =============================== PDF ===============================
        public async Task<ReportResultDto> GeneratePdfAsync(ReportRequestDto dto)
        {
            // Récupère la liste filtrée des transactions selon les critères fournis
            var transactions = await FilterTransactions(dto).ToListAsync();

            // Création en mémoire d’un flux pour construire le PDF
            using var stream = new MemoryStream();
            using var writer = new PdfWriter(stream);
            using var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            // ===== TITRE DU RAPPORT =====
            doc.Add(
                new Paragraph("📊 Rapport Financier — Web3 Kaypic")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                    .SetFontSize(16)
            );

            // Affiche la période de génération
            doc.Add(new Paragraph($"Période : {dto.StartDate:yyyy-MM-dd} → {dto.EndDate:yyyy-MM-dd}"));
            doc.Add(new Paragraph("\n")); // saut de ligne visuel

            // ===== AJOUT DES TRANSACTIONS =====
            foreach (var t in transactions)
            {
                // Chaque transaction est écrite sur une ligne avec date, titre, catégorie et montant
                doc.Add(new Paragraph(
                    $"{t.Date:yyyy-MM-dd} | {t.Titre} | {t.Category?.Nom ?? "Sans catégorie"} | {t.Montant} CAD"
                ));
            }

            // Ferme le document et finalise le flux PDF
            doc.Close();

            // Retourne le rapport sous forme d’objet DTO contenant le fichier généré
            return new ReportResultDto
            {
                FileBytes = stream.ToArray(),
                FileName = "rapport.pdf",
                ContentType = "application/pdf"
            };
        }

        // =============================== EXCEL ===============================
        public async Task<ReportResultDto> GenerateExcelAsync(ReportRequestDto dto)
        {
            // Filtrage des transactions selon les critères (dates, catégorie, etc.)
            var transactions = await FilterTransactions(dto).ToListAsync();

            // Création du fichier Excel en mémoire avec EPPlus
            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Transactions");

            // Ajout des en-têtes de colonnes
            ws.Cells["A1"].Value = "Date";
            ws.Cells["B1"].Value = "Titre";
            ws.Cells["C1"].Value = "Catégorie";
            ws.Cells["D1"].Value = "Montant";

            int row = 2; // point de départ des données (après l’en-tête)
            foreach (var t in transactions)
            {
                // Pour chaque transaction, on remplit une ligne avec ses données
                ws.Cells[row, 1].Value = t.Date.ToShortDateString();
                ws.Cells[row, 2].Value = t.Titre;
                ws.Cells[row, 3].Value = t.Category?.Nom ?? "Sans catégorie";
                ws.Cells[row, 4].Value = t.Montant;

                row++;
            }

            // Convertit le document Excel en tableau d’octets et renvoie le résultat sous forme de DTO
            return new ReportResultDto
            {
                FileBytes = package.GetAsByteArray(),
                FileName = "rapport.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
        }

        // =============================== FILTRAGE ===============================
        private IQueryable<Transaction> FilterTransactions(ReportRequestDto dto)
        {
            // Requête de base incluant les catégories associées
            var query = _db.Transactions
                .Include(t => t.Category)
                .AsQueryable();

            // Filtrage selon la date de début
            if (dto.StartDate != null)
                query = query.Where(t => t.Date >= dto.StartDate);

            // Filtrage selon la date de fin
            if (dto.EndDate != null)
                query = query.Where(t => t.Date <= dto.EndDate);

            // Filtrage selon la catégorie si spécifiée
            if (!string.IsNullOrEmpty(dto.Category))
                query = query.Where(t => t.Category != null &&
                                         t.Category.Nom == dto.Category);

            // Trie les résultats chronologiquement avant de les renvoyer
            return query.OrderBy(t => t.Date);
        }
    }
}
