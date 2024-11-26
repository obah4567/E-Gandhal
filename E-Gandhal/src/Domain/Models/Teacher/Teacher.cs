namespace E_Gandhal.src.Domain.Models.Teacher
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Matiere> Matieres { get; set; }
        public string Classes { get; set; }
    }
}
