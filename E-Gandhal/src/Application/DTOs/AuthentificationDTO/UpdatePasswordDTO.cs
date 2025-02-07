namespace E_Gandhal.src.Application.DTOs.AuthentificationDTO
{
    using System.ComponentModel.DataAnnotations;

    public record UpdatePasswordDTO
    (
        [Required, EmailAddress] string Email,
        [Required, DataType(DataType.Password)] string OldPassword,
        [Required, DataType(DataType.Password)] string NewPassword
    //[MinLength(6, ErrorMessage = "The new password must be at least 6 characters long.")]
    );

}
