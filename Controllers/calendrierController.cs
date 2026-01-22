using Microsoft.AspNetCore.Mvc;
using Web3_kaypic.Models;
using System.Collections.Generic;
using System.Linq;

namespace Web3_kaypic.Controllers
{
    public class CalendrierController : Controller
    {
        private static List<Calender> _events = new List<Calender>();

        [HttpGet]
        public IActionResult Index()
        {
            var sortedEvents = _events.OrderBy(e => e.StartDate).ToList();
            return View("~/Views/Home/calendrier.cshtml", sortedEvents);
        }

        [HttpPost]
        public IActionResult Ajouter(Calender model)
        {
            if (model.EndDate < model.StartDate)
            {
                ModelState.AddModelError("", "⚠️ La date de fin ne peut pas être antérieure à la date de début !");
            }

            if (ModelState.IsValid)
            {
                model.Id = _events.Count + 1;
                _events.Add(model);
                TempData["SuccessMessage"] = "✅ Événement ajouté avec succès !";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Veuillez remplir correctement tous les champs.";
            var sortedEvents = _events.OrderBy(e => e.StartDate).ToList();
            return View("~/Views/Home/calendrier.cshtml", sortedEvents);
        }

        [HttpPost]
        public IActionResult Supprimer(int id)
        {
            var ev = _events.FirstOrDefault(e => e.Id == id);
            if (ev != null)
            {
                _events.Remove(ev);
                TempData["SuccessMessage"] = "🗑️ Événement supprimé.";
            }
            return RedirectToAction("Index");
        }
    }
}
