using Web3_kaypic.Data;
using Web3_kaypic.Models.Finance;
using Web3_kaypic.Models.Finance.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Web3_kaypic.Services.Finance
{
    public class DashboardService
    {
        // Contexte de base de données pour les transactions financières
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================================================
        //  GÉNÉRATION COMPLÈTE DU TABLEAU DE BORD
        // ============================================================
        public async Task<DashboardResponseDto> GetDashboardAsync(DashboardRequestDto request)
        {
            // Requête de base incluant les catégories associées aux transactions
            var query = _context.Transactions
                .Include(t => t.Category)
                .AsQueryable();

            // ----- APPLICATION DES FILTRES -----
            if (request.Year != null)
                query = query.Where(t => t.Date.Year == request.Year); // Filtre par année

            if (request.Month != null)
                query = query.Where(t => t.Date.Month == request.Month); // Filtre par mois

            if (request.CategoryId != null)
                query = query.Where(t => t.CategoryId == request.CategoryId); // Filtre par catégorie

            if (!string.IsNullOrEmpty(request.MemberId))
                query = query.Where(t => t.UserId == request.MemberId); // Filtre par utilisateur

            // Exécution de la requête filtrée
            var transactions = await query.ToListAsync();

            // ----- INITIALISATION DE L'OBJET RÉPONSE -----
            var response = new DashboardResponseDto();

            // ----- CALCUL DES TOTAUX GLOBAUX -----
            response.TotalIncome = transactions
                .Where(t => t.Type == "Revenu") // Seulement les revenus
                .Sum(t => t.Montant);

            response.TotalExpense = transactions
                .Where(t => t.Type == "Dépense") // Seulement les dépenses
                .Sum(t => t.Montant);

            // Solde actuel = Revenus - Dépenses
            response.CurrentBalance = response.TotalIncome - response.TotalExpense;

            // ----- TOTAUX MENSUELS (par période yyyy-MM) -----
            response.MonthlyTotals = transactions
                .GroupBy(t => t.Date.ToString("yyyy-MM")) // Regroupe par mois/année
                .ToDictionary(
                    g => g.Key, // Clé = "2025-12"
                    g => g.Sum(t => t.Montant * (t.Type == "Revenu" ? 1 : -1)) // Revenus + / Dépenses -
                );

            // ----- TOTAUX PAR CATÉGORIE -----
            response.CategoryTotals = transactions
                .GroupBy(t => t.Category!.Nom) // Regroupe par nom de catégorie
                .ToDictionary(
                    g => g.Key, // Clé = nom de la catégorie
                    g => g.Sum(t => t.Montant) // Somme brute des montants
                );

            // ----- DERNIÈRES 5 TRANSACTIONS -----
            response.RecentTransactions = transactions
                .OrderByDescending(t => t.Date) // Plus récent en premier
                .Take(5) // Limite à 5 transactions
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Montant = t.Montant,
                    Type = t.Type,
                    Category = t.Category.Nom,
                    Date = t.Date
                })
                .ToList();

            return response; // Retourne l'objet complet du tableau de bord
        }
    }
}
