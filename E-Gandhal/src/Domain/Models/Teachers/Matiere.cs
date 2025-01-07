using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_Gandhal.src.Domain.Models.Teachers
{
    public class Matiere
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Subject { get; set; }

        // Matière enseignée dans une classe
        [Required]
        public int ClasseId { get; set; }

        [JsonIgnore]
        public Classe Classe { get; set; }

        // Professeur qui enseigne cette matière
        [Required]
        public int TeacherId { get; set; }

        [JsonIgnore]
        public Teacher Teacher { get; set; }

    }

}