using E_Gandhal.src.Domain.DTO.TeacherDTO;

namespace E_Gandhal.src.Domain.DTO.TeachersDTO
{
    public class NoteDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int MatiereId { get; set; }
        public double Value { get; set; }
        public DateTime DateAdded { get; set; }
        //public MatiereDTO Matiere { get; set; }
    }
}
