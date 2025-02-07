using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_Gandhal.src.Domain.Models.Authentification
{
    public class Register : IdentityUser
    {
        [Key]
        public int UserId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, StringLength(100)]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Matricule { get; set; } = null!;

    }
}
