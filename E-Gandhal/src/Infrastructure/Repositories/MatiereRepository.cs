using E_Gandhal.src.Domain.DTO.TeacherDTO;
using E_Gandhal.src.Domain.IServices;
using E_Gandhal.src.Domain.Models.Teachers;
using E_Gandhal.src.Infrastructure.ApplicationDBContext;
using Microsoft.EntityFrameworkCore;

namespace E_Gandhal.src.Infrastructure.Repositories
{
    public class MatiereRepository : IMatiereRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MatiereRepository> _logger;

        public MatiereRepository(ApplicationDbContext context, ILogger<MatiereRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<MatiereDTO> GetMatiereByIdAsync(int id)
        {
            var matiere = await _context.Matieres
                .Include(m => m.Classe)
                .Include(m => m.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (matiere == null) return null;

            return new MatiereDTO
            {
                Id = matiere.Id,
                Subject = matiere.Subject,
                ClasseId = matiere.ClasseId,
                TeacherId = matiere.TeacherId,
                Teacher = new TeacherDTO
                {
                    TeacherId = matiere.Teacher.TeacherId,
                    LastName = matiere.Teacher.LastName,
                    Firstname = matiere.Teacher.Firstname
                }
            };
        }

        public async Task<IEnumerable<MatiereDTO>> GetAllMatieresAsync()
        {
            return await _context.Matieres
                .Include(m => m.Classe)
                .Include(m => m.Teacher)
                .Select(m => new MatiereDTO
                {
                    Id = m.Id,
                    Subject = m.Subject,
                    ClasseId = m.ClasseId,
                    TeacherId = m.TeacherId,
                    Teacher = new TeacherDTO
                    {
                        TeacherId = m.Teacher.TeacherId,
                        LastName = m.Teacher.LastName,
                        Firstname = m.Teacher.Firstname
                    }
                })
            .ToListAsync();
        }

        public async Task<MatiereDTO> CreateMatiereAsync(MatiereDTO MatiereDTO)
        {
            var matiere = new Matiere
            {
                Subject = MatiereDTO.Subject,
                ClasseId = MatiereDTO.ClasseId,
                TeacherId = MatiereDTO.TeacherId
            };
            var testerId = await _context.Teachers.FindAsync(matiere.TeacherId);

            if (testerId == null)
            {
                _logger.LogInformation("Cet enseignant n'existe pas");
                throw new Exception($"Cet enseignant {matiere.TeacherId} n'existe pas ou n'enseigne pas cette matiere ! ");
            }

            _context.Matieres.Add(matiere);
            await _context.SaveChangesAsync();

            return new MatiereDTO
            {
                Id = matiere.Id,
                Subject = matiere.Subject,
                ClasseId = matiere.ClasseId,
                TeacherId = matiere.TeacherId
            };
        }

        public async Task<MatiereDTO> UpdateMatiereAsync(MatiereDTO MatiereDTO)
        {
            var matiere = await _context.Matieres.FindAsync(MatiereDTO.Id);

            if (matiere == null) return null;

            // Mettre à jour les propriétés de la matière
            matiere.Subject = MatiereDTO.Subject;
            matiere.ClasseId = MatiereDTO.ClasseId;
            matiere.TeacherId = MatiereDTO.TeacherId;

            await _context.SaveChangesAsync();

            return new MatiereDTO
            {
                Id = matiere.Id,
                Subject = matiere.Subject,
                ClasseId = matiere.ClasseId,
                TeacherId = matiere.TeacherId
            };
        }

        public async Task<bool> DeleteMatiereAsync(int id)
        {
            var matiere = await _context.Matieres.FindAsync(id);

            if (matiere == null) return false;

            _context.Matieres.Remove(matiere);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<MatiereDTO>> GetMatieresByClasseAsync(int classeId)
        {
            return await _context.Matieres
                .Where(m => m.ClasseId == classeId)
                .Include(m => m.Teacher)
                .Select(m => new MatiereDTO
                {
                    Id = m.Id,
                    Subject = m.Subject,
                    ClasseId = m.ClasseId,
                    TeacherId = m.TeacherId,
                    Teacher = new TeacherDTO
                    {
                        TeacherId = m.Teacher.TeacherId,
                        LastName = m.Teacher.LastName,
                        Firstname = m.Teacher.Firstname
                    }
                })
                .ToListAsync();
        }
    }
}
