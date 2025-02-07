using E_Gandhal.src.Application.DTOs.TeachersDTO;

namespace E_Gandhal.src.Application.IServices
{
    public interface ITeacherService
    {
        Task<int> CountTeachersAsync(CancellationToken cancellationToken);
        Task<TeacherDTO> GetTeacherByIdAsync(int teacherId, CancellationToken cancellationToken);
        Task<IEnumerable<TeacherDTO>> GetAllTeachersAsync(CancellationToken cancellationToken);
        Task<TeacherDTO> AddTeacherAsync(TeacherDTO teacherDto, CancellationToken cancellationToken);
        Task DeleteTeacherAsync(int teacherId, CancellationToken cancellationToken);
        Task UploadImageProfil(int teacherId, IFormFile imgProfil, CancellationToken cancellationToken);
        Task<byte[]> GetInformationPdf(int teacherId, CancellationToken cancellationToken);
        Task AttribuerNoteAsync(int studentId, int matiereId, double note, CancellationToken cancellationToken);
        Task AttribuerNoteParProfesseurAsync(int teacherId, int studentId, int matiereId, double note, CancellationToken cancellationToken);
        Task<TeacherDTO?> UpdateTeacherAsync(TeacherDTO teacherDto, CancellationToken cancellationToken);
    }
}
