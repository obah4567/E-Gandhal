using System.ComponentModel.DataAnnotations;

namespace E_Gandhal.src.Domain.Models.Authentification
{
    public class AuthSchool
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Logo { get; set; }

        [Required]
        public string Matricule { get; set; } = null!;
    }
}
