using E_Gandhal.src.Domain.DomainException;
using E_Gandhal.src.Domain.DTO.TeachersDTO;
using E_Gandhal.src.Domain.IServices;
using E_Gandhal.src.Domain.Models.Teachers;
using E_Gandhal.src.Infrastructure.ApplicationDBContext;
using Microsoft.EntityFrameworkCore;

namespace E_Gandhal.Infrastructure.Repositories
{
    public class NoteRepository : INoteService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NoteRepository> _logger;

        public NoteRepository(ApplicationDbContext context, ILogger<NoteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<NoteDTO> GetNoteByIdAsync(int id)
        {
            var note = await _context.Notes
                .Include(n => n.Students)
                .Include(n => n.Matieres)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (note == null)
            {
                _logger.LogWarning($"Note with ID {id} not found.");
                return null;
            }

            return MapToDTO(note);
        }

        public async Task<IEnumerable<NoteDTO>> GetAllNotesAsync()
        {
            var notes = await _context.Notes
                .Include(n => n.Students)
                .Include(n => n.Matieres)
                .ToListAsync();

            return notes.Select(MapToDTO);
        }

        public async Task<NoteDTO> CreateNoteAsync(NoteDTO noteDTO)
        {
            var note = new Note
            {
                StudentId = noteDTO.StudentId,
                MatiereId = noteDTO.MatiereId,
                Value = noteDTO.Value,
                DateAdded = DateTime.Now
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"New note created with ID {note.Id}");

            return await GetNoteByIdAsync(note.Id);
        }

        public async Task<NoteDTO> UpdateNoteAsync(NoteDTO noteDTO)
        {
            var note = await _context.Notes.FindAsync(noteDTO.Id);

            if (note == null)
            {
                _logger.LogWarning($"Attempt to update non-existent note with ID {noteDTO.Id}");
                return null;
            }

            note.StudentId = noteDTO.StudentId;
            note.MatiereId = noteDTO.MatiereId;
            note.Value = noteDTO.Value;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Note updated with ID {note.Id}");

            return await GetNoteByIdAsync(note.Id);
        }

        public async Task<bool> DeleteNoteAsync(int id)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                _logger.LogWarning($"Attempt to delete non-existent note with ID {id}");
                return false;
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Note deleted with ID {id}");

            return true;
        }

        public async Task<IEnumerable<NoteDTO>> GetNotesByStudentAsync(int studentId)
        {
            var notes = await _context.Notes
                .Include(n => n.Students)
                .Include(n => n.Matieres)
                .Where(n => n.StudentId == studentId)
                .ToListAsync();

            return notes.Select(MapToDTO);
        }

        public async Task<IEnumerable<NoteDTO>> GetNotesByMatiereAsync(int matiereId)
        {
            var notes = await _context.Notes
                .Include(n => n.Students)
                .Include(n => n.Matieres)
                .Where(n => n.MatiereId == matiereId)
                .ToListAsync();

            return notes.Select(MapToDTO);
        }

        public async Task<double> GetAverageByStudentAsync(int studentId)
        {
            var average = await _context.Notes
                .Where(n => n.StudentId == studentId)
                .AverageAsync(n => n.Value);

            return average;
        }

        private NoteDTO MapToDTO(Note note)
        {
            return new NoteDTO
            {
                Id = note.Id,
                StudentId = note.StudentId,
                MatiereId = note.MatiereId,
                Value = note.Value,
                DateAdded = note.DateAdded,
                // StudentName = $"{note.Students.} {note.Students.LastName}",
                // MatiereName = note.Matieres.Subject
            };
        }
    }

}
