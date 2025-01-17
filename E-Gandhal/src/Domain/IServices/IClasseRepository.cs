using E_Gandhal.src.Domain.DTO.TeacherDTO;

namespace E_Gandhal.src.Domain.IServices
{
    public interface IClasseRepository
    {
        Task<ClasseDTO> GetClasseByIdAsync(int classeId);

        Task<IEnumerable<ClasseDTO>> GetAllClassesAsync();

        Task<ClasseDTO> CreateClasseAsync(ClasseDTO classe);

        Task<ClasseDTO> UpdateClasseAsync(ClasseDTO classe);

        Task<bool> DeleteClasseAsync(int classeId);

        Task<bool> AddMatiereToClasseAsync(int classeId, int matiereId);

        Task<bool> RemoveMatiereFromClasseAsync(int classeId, int matiereId);

        Task<IEnumerable<SubjectDTO>> GetMatieresForClasseAsync(int classeId);
    }
}
