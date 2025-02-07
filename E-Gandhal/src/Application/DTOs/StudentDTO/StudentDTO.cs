using System.ComponentModel.DataAnnotations;

namespace E_Gandhal.src.Application.DTOs.StudentDTO
{
    public record StudentDTO
    (
        int Id,
        [Required] string ImageProfil,
        [Required] string Firstname,
        [Required] string Lastname,
        [Required] DateTime DateOfBirth,
        [Required] string PlaceOfBirth


    //public Classe Classe 

    /*public string FatherName  = string.Empty;

    public string MotherName  = string.Empty;

    public string GroupeSanguin  = string.Empty;*/


    );
}
