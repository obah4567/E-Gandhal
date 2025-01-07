using System.ComponentModel.DataAnnotations;

namespace E_Gandhal.src.Domain.Models.Students
{
    public class Student
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string ImageProfil { get; set; }

        [Required]
        [StringLength(100)]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Lastname { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(100)]
        public string PlaceOfBirth { get; set; } = string.Empty;

        //[Required]
        public int ClasseId { get; set; }

        /*[ForeignKey("ClasseId")]
        public Classe Classe { get; set; }*/

        /*public string FatherName { get; set; } = string.Empty;

        public string MotherName { get; set; } = string.Empty;

        public string GroupeSanguin { get; set; } = string.Empty;*/


    }
}
