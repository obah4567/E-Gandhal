using E_Gandhal.src.Domain.DTO.TeachersDTO;

namespace E_Gandhal.src.Domain.IServices
{
    public interface INoteRepository
    {
        Task<NoteDTO> GetNoteByIdAsync(int id);
        Task<IEnumerable<NoteDTO>> GetAllNotesAsync();
        Task<NoteDTO> CreateNoteAsync(NoteDTO noteDTO);
        Task<NoteDTO> UpdateNoteAsync(NoteDTO noteDTO);
        Task<bool> DeleteNoteAsync(int id);
        Task<IEnumerable<NoteDTO>> GetNotesByStudentAsync(int studentId);
        Task<IEnumerable<NoteDTO>> GetNotesByMatiereAsync(int matiereId);
        Task<double> GetAverageByStudentAsync(int studentId);
    }
}
