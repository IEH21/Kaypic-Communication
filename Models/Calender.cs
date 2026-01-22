using System;
using System.ComponentModel.DataAnnotations;

namespace Web3_kaypic.Models
{
    public class Calender
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est obligatoire")]
        public string Title { get; set; }

        [Required(ErrorMessage = "La date de début est obligatoire")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "La date de fin est obligatoire")]
        public DateTime EndDate { get; set; }

        public string? Description { get; set; }
    }
}
