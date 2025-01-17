using E_Gandhal.src.Domain.DTO.TeacherDTO;

namespace E_Gandhal.src.Domain.IServices
{
    public interface ISubjectRepository
    {
        Task<SubjectDTO> GetSubjectByIdAsync(int id);

        Task<IEnumerable<SubjectDTO>> GetAllSubjectsAsync();

        Task<SubjectDTO> CreateSubjectAsync(SubjectDTO SubjectDTO);

        Task<SubjectDTO> UpdateSubjectAsync(SubjectDTO SubjectDTO);

        Task<bool> DeleteSubjectAsync(int id);

        Task<IEnumerable<SubjectDTO>> GetSubjectsByClasseAsync(int classeId);
    }
}
