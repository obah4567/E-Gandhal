using E_Gandhal.src.Application.DTOs.TeachersDTO;

namespace E_Gandhal.src.Application.IServices
{
    public interface INoteService
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
