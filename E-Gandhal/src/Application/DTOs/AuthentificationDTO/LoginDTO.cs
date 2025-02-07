using System.ComponentModel.DataAnnotations;

namespace E_Gandhal.src.Application.DTOs.AuthentificationDTO
{
    public record LoginDTO
    (
        [Required, EmailAddress] string Email,
        [Required, DataType(DataType.Password)] string Password,
        bool RememberMe
    );

    //[Required, DataType(DataType.Password),
    //MinLength(6, ErrorMessage =
    //"The new password must be at least 6 characters long.")]


}
