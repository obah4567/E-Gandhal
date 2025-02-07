using System.ComponentModel.DataAnnotations;

namespace E_Gandhal.src.Application.DTOs.AuthentificationDTO
{
    public record AuthSchoolDTO
    (
        [Required] string Name,
        string? Logo
    );
}
