using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web3_kaypic.Services.Finance;
using Web3_kaypic.Models.Finance.DTOs;
using Microsoft.AspNetCore.Identity;
using web3_kaypic.Models;

namespace Web3_kaypic.Controllers.Api
{
    [Route("api/finance")]
    [ApiController]
    [Authorize]
    public class FinanceApiController : ControllerBase
    {
        private readonly FinanceService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public FinanceApiController(FinanceService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        private async Task<string> GetUserId()
        {
            var user = await _userManager.GetUserAsync(User);
            return user.Id;
        }

        // GET — toutes les transactions du user
        [HttpGet("transactions")]
        public async Task<IActionResult> GetAll()
        {
            var userId = await GetUserId();
            var transactions = await _service.GetTransactionsAsync(userId);

            return Ok(transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                Type = t.Type,
                Montant = t.Montant,
                Category = t.Category?.Nom,
                Note = t.Titre,
                Date = t.Date
            }));
        }

        // POST — ajouter revenu/dépense
        [HttpPost("transactions")]
        public async Task<IActionResult> Create(TransactionCreateDto dto)
        {
            var userId = await GetUserId();
            var result = await _service.AddTransactionAsync(userId, dto);
            return Ok(result);
        }

        // PUT — modifier transaction
        [HttpPut("transactions/{id}")]
        public async Task<IActionResult> Update(int id, TransactionUpdateDto dto)
        {
            var transaction = await _service.UpdateTransactionAsync(id, dto);
            return Ok(transaction);
        }

        // DELETE — supprimer transaction
        [HttpDelete("transactions/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteTransactionAsync(id);
            return success ? Ok() : NotFound();
        }
    }
}