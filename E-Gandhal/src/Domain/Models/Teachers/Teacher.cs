using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_Gandhal.src.Domain.Models.Teachers
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        public string ImageProfil { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Firstname { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [JsonIgnore]
        public ICollection<Matiere> Matieres { get; set; }
    }
}
