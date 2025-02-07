using E_Gandhal.src.Application.DTOs.TeachersDTO;

namespace E_Gandhal.src.Application.DTOs.TeacherDTO
{
    public class ClasseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubjectDTO> MatiereDTOs { get; set; } = new List<SubjectDTO>();
    }
}
