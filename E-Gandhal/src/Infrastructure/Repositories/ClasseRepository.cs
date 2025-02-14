﻿using E_Gandhal.src.Application.DTOs.TeacherDTO;
using E_Gandhal.src.Application.DTOs.TeachersDTO;
using E_Gandhal.src.Application.IServices;
using E_Gandhal.src.Domain.Models.Teachers;
using E_Gandhal.src.Infrastructure.ApplicationDBContext;
using Microsoft.EntityFrameworkCore;

namespace E_Gandhal.Infrastructure.Repositories
{
    public class ClasseRepository : IClasseService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClasseRepository> _logger;

        public ClasseRepository(ApplicationDbContext context, ILogger<ClasseRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ClasseDTO> GetClasseByIdAsync(int classeId)
        {
            var classe = await _context.Classes
                .Include(c => c.Matieres)
                .FirstOrDefaultAsync(c => c.ClasseId == classeId);

            if (classe == null)
            {
                _logger.LogInformation($"Nous n'avons pas trouvé de classe {classeId} !");
                throw new Exception($"Cette classe {classeId} est introuvable.");
            }

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
            if (classe == null)
            {
                _logger.LogInformation($"Nous n'avons pas trouvé de classe {classeDto} !");
                throw new Exception($"Cette classe {classeDto} est introuvable.");
            }

            classe.Name = classeDto.Name;

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
            if (classe == null)
            {
                _logger.LogInformation($"Nous n'avons pas trouvé de classe {classeId} !");
                throw new Exception($"Cette classe {classeId} est introuvable.");
            }

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
                _logger.LogInformation($"Désolé, nous n'avons pas ajouté cette matiere {matiereId} à cette classe {classeId} !");
                throw new Exception($"Désolé, nous n'avons pas ajouté cette matiere {matiereId} à cette classe {classeId} !");
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

            if (classe == null)
            {
                _logger.LogInformation($"Désolé, nous ne pouvons pas supprimé cette matière {matiereId} à cette classe {classeId} !");
                throw new Exception($"Désolé, nous n'avons pas ajouté cette matière {matiereId} à cette classe {classeId} !");
            }

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
