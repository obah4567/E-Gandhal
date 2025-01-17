namespace E_Gandhal.src.Domain.DTO.TeacherDTO
{
    public class SubjectDTO
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public int ClasseId { get; set; }
        //public ClasseDTO Classe { get; set; } // Optionnel, évitez si inutile

        public int TeacherId { get; set; }
        public TeacherDTO Teacher { get; set; } // Optionnel, évitez si inutile
    }
}
