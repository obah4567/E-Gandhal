using E_Gandhal.src.Domain.DTO.TeacherDTO;
using E_Gandhal.src.Domain.IServices;
using E_Gandhal.src.Domain.Models.Teachers;
using E_Gandhal.src.Infrastructure.ApplicationDBContext;
using Microsoft.EntityFrameworkCore;

namespace E_Gandhal.Infrastructure.Repositories
{
    public class ClasseRepository : IClasseService
    {
        private readonly ApplicationDbContext _context;

        public ClasseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClasseDTO> GetClasseByIdAsync(int classeId)
        {
            var classe = await _context.Classes
                .Include(c => c.Matieres)
                .FirstOrDefaultAsync(c => c.ClasseId == classeId);

            if (classe == null) return null;

            return new ClasseDTO
            {
                Id = classe.ClasseId,
                Name = classe.Name,
                MatiereDTOs = classe.Matieres.Select(m => new SubjectDTO
                {
                    Id = m.Id,
                    Subject = m.Subject
                }).ToList()
            };
        }

        public async Task<IEnumerable<ClasseDTO>> GetAllClassesAsync()
        {
            return await _context.Classes
            .Include(c => c.Matieres)
                .Select(c => new ClasseDTO
                {
                    Id = c.ClasseId,
                    Name = c.Name,
                    MatiereDTOs = c.Matieres.Select(m => new SubjectDTO
                    {
                        Id = m.Id,
                        Subject = m.Subject
                    }).ToList()
                })
            .ToListAsync();
        }

        public async Task<ClasseDTO> CreateClasseAsync(ClasseDTO classeDto)
        {
            var classe = new Classe
            {
                Name = classeDto.Name,
                Matieres = new List<Matiere>() // Vous pouvez ajouter des matières ici si nécessaire
            };

            _context.Classes.Add(classe);
            await _context.SaveChangesAsync();

            return new ClasseDTO
            {
                Id = classe.ClasseId,
                Name = classe.Name,
                MatiereDTOs = classe.Matieres.Select(m => new SubjectDTO
                {
                    Id = m.Id,
                    Subject = m.Subject
                }).ToList()
            };
        }

        public async Task<ClasseDTO> UpdateClasseAsync(ClasseDTO classeDto)
        {
            var classe = await _context.Classes.FindAsync(classeDto.Id);
            if (classe == null) return null;

            // Mettre à jour les propriétés de la classe
            classe.Name = classeDto.Name;

            // Si vous souhaitez mettre à jour les matières, vous pouvez le faire ici.

            await _context.SaveChangesAsync();

            return new ClasseDTO
            {
                Id = classe.ClasseId,
                Name = classe.Name,
                MatiereDTOs = classe.Matieres.Select(m => new SubjectDTO
                {
                    Id = m.Id,
                    Subject = m.Subject
                }).ToList()
            };
        }

        public async Task<bool> DeleteClasseAsync(int classeId)
        {
            var classe = await _context.Classes.FindAsync(classeId);
            if (classe == null) return false;

            _context.Classes.Remove(classe);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddMatiereToClasseAsync(int classeId, int matiereId)
        {
            var classe = await _context.Classes
                .Include(c => c.Matieres)
                .FirstOrDefaultAsync(c => c.ClasseId == classeId);

            var matiere = await _context.Matieres.FindAsync(matiereId);

            if (classe == null || matiere == null)
            {
                return false;
            }

            // Ajout de la matière à la liste des matières de la classe
            if (!classe.Matieres.Any(m => m.Id == matiereId))
            {
                classe.Matieres.Add(matiere);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> RemoveMatiereFromClasseAsync(int classeId, int matiereId)
        {
            var classe = await _context.Classes
                .Include(c => c.Matieres)
                .FirstOrDefaultAsync(c => c.ClasseId == classeId);

            if (classe == null) return false;

            var matiereToRemove = classe.Matieres.FirstOrDefault(m => m.Id == matiereId);

            if (matiereToRemove != null)
            {
                classe.Matieres.Remove(matiereToRemove);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<SubjectDTO>> GetMatieresForClasseAsync(int classeId)
        {
            var classe = await _context.Classes
                .Include(c => c.Matieres)
                .FirstOrDefaultAsync(c => c.ClasseId == classeId);

            if (classe == null) return new List<SubjectDTO>();

            return classe.Matieres.Select(m => new SubjectDTO
            {
                Id = m.Id,
                Subject = m.Subject,
            }).ToList();
        }
    }
}
