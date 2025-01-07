using E_Gandhal.src.Domain.Models.Students;
using System.ComponentModel.DataAnnotations;

namespace E_Gandhal.src.Domain.Models.Teachers
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }
        public Student Students { get; set; }

        [Required]
        public int MatiereId { get; set; }
        public Matiere Matieres { get; set; }

        [Required]
        public double Value { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }
    }

}
