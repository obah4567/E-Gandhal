using E_Gandhal.src.Application.DTOs.TeachersDTO;

namespace E_Gandhal.src.Application.IServices
{
    public interface ISubjectService
    {
        Task<SubjectDTO> GetSubjectByIdAsync(int id);

        Task<IEnumerable<SubjectDTO>> GetAllSubjectsAsync();

        Task<SubjectDTO> CreateSubjectAsync(SubjectDTO SubjectDTO);

        Task<SubjectDTO> UpdateSubjectAsync(SubjectDTO SubjectDTO);

        Task<bool> DeleteSubjectAsync(int id);

        Task<IEnumerable<SubjectDTO>> GetSubjectsByClasseAsync(int classeId);
    }
}
