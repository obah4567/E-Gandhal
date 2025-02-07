using E_Gandhal.src.Application.DTOs.TeacherDTO;

namespace E_Gandhal.src.Application.DTOs.TeachersDTO
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string ImageProfil { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }

        public int ClasseId { get; set; }
        public ClasseDTO Classe { get; set; } // Optionnel, évitez si inutile
    }
}
