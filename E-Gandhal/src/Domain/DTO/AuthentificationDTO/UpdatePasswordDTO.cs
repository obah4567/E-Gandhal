namespace E_Gandhal.src.Domain.DTO.AuthentificationDTO
{
    using System.ComponentModel.DataAnnotations;

    public class UpdatePasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        //[MinLength(6, ErrorMessage = "The old password must be at least 6 characters long.")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        //[MinLength(6, ErrorMessage = "The new password must be at least 6 characters long.")]
        public string NewPassword { get; set; }
    }

}
