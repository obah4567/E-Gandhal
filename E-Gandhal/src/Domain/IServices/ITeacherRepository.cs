using E_Gandhal.src.Domain.Models.Teacher;

namespace E_Gandhal.src.Domain.IServices
{
    public interface ITeacherRepository
    {
        Task AddTeacherAsync(Teacher teacher, CancellationToken cancellationToken);
        Task DeleteTeacherAsync(string name , CancellationToken cancellationToken);    
        Task AjouterNoteAsync(Matiere matiere, double note, CancellationToken cancellationToken);


    }
}
