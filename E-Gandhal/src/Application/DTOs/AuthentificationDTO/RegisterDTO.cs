using System.ComponentModel.DataAnnotations;

namespace E_Gandhal.src.Application.DTOs.AuthentificationDTO
{
    public record RegisterDTO
    (
        [Required] string Username,
        [Required, EmailAddress] string Email,
        [Required] string Password,
        [Required] string Firstname,
        [Required] string Lastname,
        [Required] DateTime DateOfBirth,
        [Required] string Matricule
    );
}
