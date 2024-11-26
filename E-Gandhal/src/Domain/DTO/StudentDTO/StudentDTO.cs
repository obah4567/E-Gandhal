namespace E_Gandhal.src.Domain.DTO.StudentDTO
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string ImageProfil { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; } = string.Empty;
        public string Classe { get; set; } = string.Empty;

        /*public string FatherName { get; set; } = string.Empty;

        public string MotherName { get; set; } = string.Empty;

        public string GroupeSanguin { get; set; } = string.Empty;*/


    }
}
