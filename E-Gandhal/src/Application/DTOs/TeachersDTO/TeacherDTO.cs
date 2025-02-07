namespace E_Gandhal.src.Application.DTOs.TeachersDTO
{
    public class TeacherDTO
    {
        public int TeacherId { get; set; }
        public string ImageProfil { get; set; }
        public string LastName { get; set; }
        public string Firstname { get; set; }
        public string Description { get; set; }
        //
        public List<int> MatiereIds { get; set; } = new List<int>();

    }
}
