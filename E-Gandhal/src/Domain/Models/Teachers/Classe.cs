using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_Gandhal.src.Domain.Models.Teachers
{
    public class Classe
    {
        [Key]
        public int ClasseId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Liste des matières enseignées dans cette classe

        [JsonIgnore]
        public ICollection<Matiere> Matieres { get; set; }
    }
}