using Microsoft.EntityFrameworkCore;
using Web3_kaypic.Data;
using Web3_kaypic.Models.Finance;
using Web3_kaypic.Models.Finance.DTOs;

namespace Web3_kaypic.Services.Finance
{
    public class FinanceService
    {
        // Contexte EF Core utilisé pour accéder et manipuler les données financières
        private readonly ApplicationDbContext _context;

        public FinanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================================================
        //  AJOUTER UNE TRANSACTION (Revenu ou Dépense)
        // ============================================================
        public async Task<Transaction> AddTransactionAsync(string userId, TransactionCreateDto dto)
        {
            // --- Règles de validation métier ---
            if (dto.Amount <= 0)
                throw new Exception("Le montant doit être positif.");

            if (dto.Type != "revenu" && dto.Type != "depense")
                throw new Exception("Le type doit être 'revenu' ou 'depense'.");

            if (dto.Date > DateTime.Now.AddDays(1))
                throw new Exception("La date ne peut pas être dans le futur.");

            // Création de l’objet Transaction à partir des données reçues
            var transaction = new Transaction
            {
                UserId = userId,
                // Si c’est une dépense, le montant est enregistré en négatif
                Montant = dto.Type == "depense" ? -dto.Amount : dto.Amount,
                Type = dto.Type,
                CategoryId = dto.CategoryId,
                Titre = dto.Note,
                Date = dto.Date
            };

            // Ajout et sauvegarde dans la base de données
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction; // renvoie la transaction nouvellement créée
        }

        // ============================================================
        //  OBTENIR TOUTES LES TRANSACTIONS D’UN UTILISATEUR
        // ============================================================
        public async Task<List<Transaction>> GetTransactionsAsync(string userId)
        {
            // Récupère les transactions de l’utilisateur et inclut les catégories associées
            // Trie les résultats du plus récent au plus ancien
            return await _context.Transactions
                .Include(c => c.Category)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
        }

        // ============================================================
        //  MODIFIER UNE TRANSACTION EXISTANTE
        // ============================================================
        public async Task<Transaction> UpdateTransactionAsync(int id, TransactionUpdateDto dto)
        {
            // Recherche de la transaction à mettre à jour
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
                throw new Exception("Transaction introuvable.");

            // Mise à jour des champs modifiables
            transaction.Montant = dto.Amount;
            transaction.Titre = dto.Note;
            transaction.CategoryId = dto.CategoryId;
            transaction.Date = dto.Date;

            await _context.SaveChangesAsync();
            return transaction;
        }

        // ============================================================
        //  SUPPRIMER UNE TRANSACTION
        // ============================================================
        public async Task<bool> DeleteTransactionAsync(int id)
        {
            // Recherche de la transaction à supprimer
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
                return false; // Retourne false si la transaction n’existe pas

            // Suppression de la transaction dans la base
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return true; // Retourne true si la suppression a réussi
        }
    }
}
