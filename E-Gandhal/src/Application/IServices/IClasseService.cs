using E_Gandhal.src.Application.DTOs.TeacherDTO;
using E_Gandhal.src.Application.DTOs.TeachersDTO;

namespace E_Gandhal.src.Application.IServices
{
    public interface IClasseService
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
